import json
import re
from pathlib import Path


def iter_items(items, folder: str | None = None):
    for it in items:
        if isinstance(it, dict) and "item" in it and isinstance(it["item"], list):
            new_folder = ((folder + "/" if folder else "") + it.get("name", "")).strip("/")
            yield from iter_items(it["item"], new_folder)
        else:
            yield (folder or ""), it


def extract_real_blocks(real_md: str):
    """
    Parses FRONTEND_API_DOCS_REAL.md into a lookup by (method,url)-> {status, response_json_text}.
    Assumes the format we generated in capture_real_responses.py.
    """
    lookup = {}
    lines = real_md.splitlines()
    i = 0
    cur_method = cur_url = cur_status = None
    while i < len(lines):
        line = lines[i].strip()
        if line.startswith("- **Method**:"):
            m = re.search(r"`([^`]+)`", line)
            cur_method = (m.group(1) if m else "").upper()
        elif line.startswith("- **URL**:"):
            m = re.search(r"`([^`]+)`", line)
            cur_url = m.group(1) if m else ""
        elif line.startswith("- **Status**:"):
            m = re.search(r"`([^`]+)`", line)
            cur_status = m.group(1) if m else ""
        elif line.startswith("```json") and cur_method and cur_url and cur_status is not None:
            # capture json block
            j = i + 1
            buf = []
            while j < len(lines) and lines[j].strip() != "```":
                buf.append(lines[j])
                j += 1
            lookup[(cur_method, cur_url)] = {"status": cur_status, "json": "\n".join(buf).strip()}
            i = j
        i += 1
    return lookup


def get_req(it: dict):
    req = it.get("request", {}) or {}
    method = (req.get("method") or "GET").upper()
    url = req.get("url") or {}
    raw = url.get("raw") if isinstance(url, dict) else str(url)

    headers = req.get("header") or []
    body = req.get("body") or {}
    body_text = None
    mode = body.get("mode")
    if mode == "raw":
        body_text = body.get("raw")
    elif mode == "urlencoded":
        body_text = json.dumps({p.get("key"): p.get("value") for p in (body.get("urlencoded") or [])}, indent=2)

    return method, raw or "", headers, body_text


def main():
    repo_root = Path(__file__).resolve().parents[1]
    coll_path = repo_root / "DigitalEmployee_Postman_Collection.json"
    real_path = repo_root / "FRONTEND_API_DOCS_REAL.md"

    coll = json.loads(coll_path.read_text(encoding="utf-8"))
    real_lookup = extract_real_blocks(real_path.read_text(encoding="utf-8"))

    # Determine baseUrl var (for raw URLs in the merged doc)
    vars_ = {v.get("key"): str(v.get("value") or "") for v in (coll.get("variable") or []) if isinstance(v, dict)}
    base_url = (vars_.get("baseUrl") or "{{baseUrl}}").rstrip("/")

    md = []
    md.append("## Frontend API Docs (Request + REAL Response)")
    md.append("")
    md.append(f"- **Collection**: `{coll_path.name}`")
    md.append(f"- **Real responses source**: `{real_path.name}`")
    md.append(f"- **Base URL**: `{{{{baseUrl}}}}` (captured base in real run was `{base_url}`)")
    md.append("")

    for folder, it in iter_items(coll.get("item", []) or []):
        name = it.get("name", "")
        method, raw, headers, body = get_req(it)
        if not raw:
            continue

        # Expand baseUrl for matching real capture (it used http://localhost:9875)
        real_url = raw.replace("{{baseUrl}}", base_url)

        md.append(f"## {folder}" if folder else "## (root)")
        md.append("")
        md.append(f"### {name}")
        md.append("")
        md.append(f"- **Method**: `{method}`")
        md.append(f"- **URL**: `{raw}`")

        if body and method in ("POST", "PUT", "PATCH"):
            md.append("- **Request body**:")
            md.append("")
            md.append("```json")
            md.append(body.strip())
            md.append("```")

        hit = real_lookup.get((method, real_url))
        if hit:
            md.append(f"- **Response (real)**: status `{hit['status']}`")
            md.append("")
            md.append("```json")
            md.append(hit["json"])
            md.append("```")
        else:
            md.append("- **Response (real)**: _not captured_ (missing vars/setup or endpoint not called in capture run)")

        md.append("")

    out_path = repo_root / "FRONTEND_API_DOCS_FULL.md"
    out_path.write_text("\n".join(md), encoding="utf-8")
    print(f"Wrote {out_path}")


if __name__ == "__main__":
    main()

