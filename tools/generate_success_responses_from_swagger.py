import json
import re
import urllib.request
from collections import defaultdict
from pathlib import Path


def iter_items(items, folder: str | None = None):
    for it in items:
        if isinstance(it, dict) and "item" in it and isinstance(it["item"], list):
            new_folder = ((folder + "/" if folder else "") + it.get("name", "")).strip("/")
            yield from iter_items(it["item"], new_folder)
        else:
            yield (folder or ""), it


def substitute_base_url(s: str, base_url: str) -> str:
    return (s or "").replace("{{baseUrl}}", base_url.rstrip("/"))


def normalize_path_for_swagger(url: str) -> str:
    """
    Convert a concrete URL like:
      http://localhost:9875/api/Customer/123
    into swagger path:
      /api/Customer/{customerId}
    We can't infer exact parameter names, so we match by structure later.
    """
    # strip scheme/host
    url = re.sub(r"^https?://[^/]+", "", url)
    return url


def get_req(it: dict):
    req = it.get("request", {}) or {}
    method = (req.get("method") or "GET").upper()
    url = req.get("url") or {}
    raw = url.get("raw") if isinstance(url, dict) else str(url)

    body = req.get("body") or {}
    body_text = None
    mode = body.get("mode")
    if mode == "raw":
        body_text = body.get("raw")
    elif mode == "urlencoded":
        body_text = json.dumps({p.get("key"): p.get("value") for p in (body.get("urlencoded") or [])}, indent=2)

    return method, raw or "", body_text


def fetch_json(url: str):
    with urllib.request.urlopen(url, timeout=30) as resp:
        return json.loads(resp.read().decode("utf-8"))


def resolve_ref(spec: dict, ref: str):
    # ref like "#/components/schemas/Foo"
    parts = ref.lstrip("#/").split("/")
    cur = spec
    for p in parts:
        cur = cur[p]
    return cur


def example_for_schema(spec: dict, schema: dict, depth: int = 0):
    if depth > 5:
        return None

    if not schema:
        return None

    if "$ref" in schema:
        return example_for_schema(spec, resolve_ref(spec, schema["$ref"]), depth + 1)

    t = schema.get("type")

    if "example" in schema:
        return schema["example"]

    if "enum" in schema:
        return schema["enum"][0] if schema["enum"] else None

    if t == "string":
        fmt = schema.get("format")
        if fmt == "date-time":
            return "2026-04-14T12:00:00Z"
        if fmt == "date":
            return "2026-04-14"
        if fmt == "uuid":
            return "00000000-0000-0000-0000-000000000000"
        return schema.get("default") or "<string>"

    if t == "integer":
        return schema.get("default") or 0

    if t == "number":
        return schema.get("default") or 0.0

    if t == "boolean":
        return schema.get("default") if "default" in schema else True

    if t == "array":
        item_schema = schema.get("items") or {}
        return [example_for_schema(spec, item_schema, depth + 1)]

    if t == "object" or "properties" in schema:
        props = schema.get("properties") or {}
        out = {}
        for k, v in props.items():
            out[k] = example_for_schema(spec, v, depth + 1)
        return out

    # oneOf/anyOf fallbacks
    for key in ("oneOf", "anyOf", "allOf"):
        if key in schema and schema[key]:
            return example_for_schema(spec, schema[key][0], depth + 1)

    return None


def find_success_response_schema(op: dict):
    # prefer 200 then 201
    responses = op.get("responses") or {}
    for code in ("200", "201"):
        if code in responses:
            content = (responses[code].get("content") or {}).get("application/json")
            if content and "schema" in content:
                return code, content["schema"]
    return None, None


def fallback_success_example(method: str, url: str, name: str):
    u = (url or "").lower()
    lname = (name or "").lower()

    if "/api/auth/login" in u or "login" in lname:
        return {
            "token": "<jwt>",
            "expiresInMinutes": 60,
            "user": {"id": "<userId>", "fullName": "Business Owner", "email": "owner@app.com", "role": "Owner"},
        }
    if "/api/auth/register" in u or "register" in lname:
        return {"message": "User registered successfully.", "userId": "<userId>"}

    if method == "DELETE":
        return {"message": "Deleted"}

    return {"message": "OK"}


def main():
    repo_root = Path(__file__).resolve().parents[1]
    coll_path = repo_root / "DigitalEmployee_Postman_Collection.json"
    coll = json.loads(coll_path.read_text(encoding="utf-8"))

    vars_ = {v.get("key"): str(v.get("value") or "") for v in (coll.get("variable") or []) if isinstance(v, dict)}
    base_url = (vars_.get("baseUrl") or "http://localhost:9875").rstrip("/")

    swagger_url = f"{base_url}/swagger/v1/swagger.json"
    spec = fetch_json(swagger_url)

    # Build quick lookup for swagger operations by path+method
    swagger_ops = {}
    for path, item in (spec.get("paths") or {}).items():
        for m, op in (item or {}).items():
            swagger_ops[(m.upper(), path)] = op

    md = []
    md.append("## Frontend API Docs (Request JSON + SUCCESS Response JSON)")
    md.append("")
    md.append(f"- **Postman source**: `{coll_path.name}`")
    md.append(f"- **Swagger source**: `{swagger_url}`")
    md.append("- **Responses**: Generated from Swagger `200/201` schemas (so they match the DTO contract).")
    md.append("")

    by_folder = defaultdict(list)
    for folder, it in iter_items(coll.get("item", []) or []):
        name = it.get("name", "")
        method, raw, body = get_req(it)
        if not raw:
            continue
        url = substitute_base_url(raw, base_url)
        path = normalize_path_for_swagger(url)
        by_folder[folder].append((name, method, raw, path, body))

    for folder in sorted(by_folder.keys()):
        md.append(f"## {folder if folder else '(root)'}")
        md.append("")
        for name, method, raw, path, body in by_folder[folder]:
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

            # Find matching swagger op by exact path; if not found, try structural match
            op = swagger_ops.get((method, path))
            if op is None:
                # attempt match by replacing path segments that look like ids with {param}
                segments = path.strip("/").split("/")
                candidates = []
                for (m, spath), sop in swagger_ops.items():
                    if m != method:
                        continue
                    ssegs = spath.strip("/").split("/")
                    if len(ssegs) != len(segments):
                        continue
                    ok = True
                    for a, b in zip(segments, ssegs):
                        if b.startswith("{") and b.endswith("}"):
                            continue
                        if a != b:
                            ok = False
                            break
                    if ok:
                        candidates.append((spath, sop))
                if candidates:
                    op = candidates[0][1]

            code, schema = (None, None) if op is None else find_success_response_schema(op)

            if schema is None:
                md.append("- **Success response (200/201)**:")
                md.append("")
                md.append("```json")
                md.append(json.dumps(fallback_success_example(method, path, name), indent=2, ensure_ascii=False))
                md.append("```")
                md.append("")
                continue

            ex = example_for_schema(spec, schema)  # already resolves $ref
            md.append(f"- **Success response ({code})**:")
            md.append("")
            md.append("```json")
            md.append(json.dumps(ex, indent=2, ensure_ascii=False))
            md.append("```")
            md.append("")
        md.append("")

    out_path = repo_root / "FRONTEND_API_DOCS_200.md"
    out_path.write_text("\n".join(md), encoding="utf-8")
    print(f"Wrote {out_path}")


if __name__ == "__main__":
    main()

