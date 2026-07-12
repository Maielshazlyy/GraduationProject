import json
import re
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
from collections import defaultdict
from pathlib import Path
from datetime import datetime


def iter_items(items, folder: str | None = None):
    for it in items:
        if isinstance(it, dict) and "item" in it and isinstance(it["item"], list):
            new_folder = ((folder + "/" if folder else "") + it.get("name", "")).strip("/")
            yield from iter_items(it["item"], new_folder)
        else:
            yield (folder or ""), it


def deep_get(d, path, default=None):
    cur = d
    for p in path:
        if not isinstance(cur, dict) or p not in cur:
            return default
        cur = cur[p]
    return cur


def substitute_vars(s: str, vars_: dict[str, str]) -> str:
    def repl(m):
        key = m.group(1)
        val = vars_.get(key)
        # If variable is missing OR empty, keep the placeholder so the request is skipped.
        # This avoids generating misleading URLs like `/api/Customer/` when `customerId` is not known.
        return val if val else m.group(0)

    return re.sub(r"\{\{([^}]+)\}\}", repl, s)


def build_request(item: dict, vars_: dict[str, str]):
    req = item.get("request", {}) or {}
    method = (req.get("method") or "GET").upper()
    url = req.get("url") or {}
    raw = url.get("raw") if isinstance(url, dict) else str(url)
    if not raw:
        return None

    raw = substitute_vars(raw, vars_)
    headers = {h.get("key"): h.get("value") for h in (req.get("header") or []) if isinstance(h, dict)}

    body = req.get("body") or {}
    data = None
    content_type = None
    body_for_docs = None

    mode = body.get("mode")
    if mode == "raw":
        raw_body = body.get("raw") or ""
        raw_body = substitute_vars(raw_body, vars_)
        data = raw_body.encode("utf-8")
        content_type = "application/json"
        body_for_docs = raw_body
    elif mode == "urlencoded":
        params = {}
        for p in body.get("urlencoded") or []:
            if not isinstance(p, dict):
                continue
            k = p.get("key")
            v = p.get("value")
            if k is None:
                continue
            params[k] = substitute_vars(str(v or ""), vars_)
        data = urllib.parse.urlencode(params).encode("utf-8")
        content_type = "application/x-www-form-urlencoded"
        body_for_docs = params

    if content_type and not any(k.lower() == "content-type" for k in headers.keys()):
        headers["Content-Type"] = content_type

    return method, raw, headers, data, body_for_docs


def is_public(url: str) -> bool:
    u = url.lower()
    if "/api/auth/register" in u or "/api/auth/login" in u:
        return True
    if "/api/business/onboard" in u:
        return True
    return False


def http_call(method: str, url: str, headers: dict[str, str], data: bytes | None):
    req = urllib.request.Request(url=url, data=data, method=method)
    for k, v in (headers or {}).items():
        req.add_header(k, v)
    try:
        with urllib.request.urlopen(req, timeout=30) as resp:
            status = resp.getcode()
            raw = resp.read().decode("utf-8", errors="replace")
            return status, raw
    except urllib.error.HTTPError as e:
        status = e.code
        raw = e.read().decode("utf-8", errors="replace")
        return status, raw
    except Exception as e:
        return 0, json.dumps({"error": str(e)})


def try_parse_json(text: str):
    text = (text or "").strip()
    if not text:
        return None
    try:
        return json.loads(text)
    except Exception:
        return None


def extract_vars_from_response(url: str, status: int, payload):
    out: dict[str, str] = {}
    if status <= 0 or payload is None:
        return out

    u = url.lower()
    # If we fetched businesses list, prefer using an existing business id for downstream calls.
    # This avoids cases where newly created/onboarded businesses are not available in the current DB/environment.
    if "/api/business" in u and isinstance(payload, list) and payload:
        first = payload[0]
        if isinstance(first, dict) and first.get("id"):
            out["businessId"] = str(first["id"])

    # Auth endpoints (register/login/etc.) often return token directly
    if "/api/auth/" in u and isinstance(payload, dict):
        token = payload.get("token") or payload.get("Token")
        if token:
            out["token"] = str(token)
        user = payload.get("user") or payload.get("User")
        if isinstance(user, dict) and user.get("id"):
            out["userId"] = str(user["id"])
        # IMPORTANT: Do NOT capture/overwrite collection variable `businessId` from auth responses.
        # In this codebase, most endpoints expect the internal Business.Id, while auth may return Business.BusinessId (different value).

    if "/api/business/onboard" in u and isinstance(payload, dict):
        # IMPORTANT: downstream endpoints expect internal business "id"
        bid = payload.get("id") or payload.get("businessId") or payload.get("businessID")
        if bid:
            out["businessId"] = str(bid)

    # Create Business usually returns { id: ... }
    if "/api/business" in u and isinstance(payload, dict):
        # IMPORTANT: Many endpoints expect the internal business "id" (not the public "businessId")
        bid = payload.get("id") or payload.get("businessId")
        if bid:
            out["businessId"] = str(bid)

    # Generic: if response has "*Id" fields, capture common ones
    if isinstance(payload, dict):
        for k in ("businessId", "customerId", "ticketId", "orderId", "interactionId", "menuItemId", "menuCategoryId"):
            # Do not overwrite values we already derived (e.g., businessId should prefer internal id)
            if k in out:
                continue
            if k in payload and payload[k]:
                out[k] = str(payload[k])

    return out


def main():
    repo_root = Path(__file__).resolve().parents[1]
    coll_path = repo_root / "DigitalEmployee_Postman_Collection.json"
    coll = json.loads(coll_path.read_text(encoding="utf-8"))

    # Load base variables from collection
    vars_ = {v.get("key"): str(v.get("value") or "") for v in (coll.get("variable") or []) if isinstance(v, dict)}
    base_url = vars_.get("baseUrl") or "http://localhost:9875"

    # Normalize baseUrl (avoid trailing slash)
    vars_["baseUrl"] = base_url.rstrip("/")

    # Use unique emails per run so Auth endpoints return success (not "already registered")
    stamp = datetime.utcnow().strftime("%Y%m%d%H%M%S")
    vars_["__runStamp"] = stamp
    vars_["__adminEmail"] = f"admin+{stamp}@app.com"
    vars_["__ownerEmail"] = f"owner+{stamp}@app.com"
    vars_["__agentEmail"] = f"agent+{stamp}@app.com"

    results_success_by_folder: dict[str, list[dict]] = defaultdict(list)
    results_all_by_folder: dict[str, list[dict]] = defaultdict(list)

    items = list(iter_items(coll.get("item", []) or []))

    for folder, it in items:
        name = it.get("name", "")
        built = build_request(it, vars_)
        if built is None:
            continue
        method, url, headers, data, body_for_docs = built

        # default auth header for non-public endpoints if token is known
        if not is_public(url) and vars_.get("token"):
            headers = dict(headers)
            headers.setdefault("Authorization", f"Bearer {vars_['token']}")

        # If URL still contains unresolved {{var}}, skip (store as skipped)
        if "{{" in url and "}}" in url:
            # skip endpoints we can't execute
            continue

        # Patch known JSON bodies to use unique emails (so we capture SUCCESS responses)
        if data and method in ("POST", "PUT", "PATCH"):
            try:
                body_json = json.loads(data.decode("utf-8"))
                u = url.lower()
                if "/api/auth/register-admin" in u and isinstance(body_json, dict):
                    body_json["email"] = vars_["__adminEmail"]
                    data = json.dumps(body_json).encode("utf-8")
                    body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
                elif "/api/auth/register-owner" in u and isinstance(body_json, dict):
                    body_json["email"] = vars_["__ownerEmail"]
                    data = json.dumps(body_json).encode("utf-8")
                    body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
                elif "/api/auth/register" in u and isinstance(body_json, dict) and "businessId" in body_json:
                    body_json["email"] = vars_["__agentEmail"]
                    data = json.dumps(body_json).encode("utf-8")
                    body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
                elif "/api/auth/login" in u and isinstance(body_json, dict):
                    em = str(body_json.get("email") or "")
                    if "owner@app.com" in em:
                        body_json["email"] = vars_["__ownerEmail"]
                        data = json.dumps(body_json).encode("utf-8")
                        body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
                    if "admin@app.com" in em:
                        body_json["email"] = vars_["__adminEmail"]
                        data = json.dumps(body_json).encode("utf-8")
                        body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
                elif "/api/customer" in u and isinstance(body_json, dict):
                    # Avoid "already exists" so we can capture 201 + customerId
                    if method == "POST" and "email" in body_json:
                        body_json["email"] = f"customer+{vars_['__runStamp']}@app.com"
                    if method in ("PUT", "PATCH") and "email" in body_json:
                        body_json["email"] = f"customer.updated+{vars_['__runStamp']}@app.com"
                    data = json.dumps(body_json).encode("utf-8")
                    body_for_docs = json.dumps(body_json, indent=2, ensure_ascii=False)
            except Exception:
                pass

        status, raw_text = http_call(method, url, headers, data)
        payload = try_parse_json(raw_text)
        if payload is None:
            payload = {"raw": raw_text}

        # capture variables for later requests
        new_vars = extract_vars_from_response(url, status, payload)
        vars_.update({k: v for k, v in new_vars.items() if v})

        rec = {
            "name": name,
            "method": method,
            "url": url,
            "requestBody": body_for_docs,
            "status": status,
            "response": payload,
        }
        results_all_by_folder[folder].append(rec)

        # Only keep SUCCESS responses (200/201) in the success-only output document
        if status in (200, 201):
            results_success_by_folder[folder].append(rec)

        # light throttle to avoid overwhelming the server
        time.sleep(0.05)

    def render_md(title: str, note: str, results_by_folder: dict[str, list[dict]]) -> str:
        md: list[str] = []
        md.append(f"## {title}")
        md.append("")
        md.append(f"- **Source collection**: `{coll_path.name}`")
        md.append(f"- **Base URL used**: `{vars_['baseUrl']}`")
        md.append(f"- **Note**: {note}")
        md.append("")

        for folder in sorted(results_by_folder.keys()):
            md.append(f"## {folder if folder else '(root)'}")
            md.append("")
            for r in results_by_folder[folder]:
                md.append(f"### {r['name']}")
                md.append("")
                md.append(f"- **Method**: `{r['method']}`")
                md.append(f"- **URL**: `{r['url']}`")

                if r.get("requestBody") is not None and r["method"] in ("POST", "PUT", "PATCH"):
                    md.append("- **Request body (from Postman)**:")
                    md.append("")
                    md.append("```json")
                    md.append(json.dumps(r["requestBody"], indent=2, ensure_ascii=False) if isinstance(r["requestBody"], (dict, list)) else str(r["requestBody"]))
                    md.append("```")

                md.append(f"- **Status**: `{r.get('status')}`")
                md.append("- **Response (real)**:")
                md.append("")
                md.append("```json")
                md.append(json.dumps(r.get("response"), indent=2, ensure_ascii=False))
                md.append("```")
                md.append("")
            md.append("")
        return "\n".join(md)

    out_success = repo_root / "FRONTEND_API_DOCS_SUCCESS_200_REAL.md"
    out_success.write_text(
        render_md(
            "Frontend API Reference (REAL SUCCESS responses only)",
            "This document only includes endpoints that returned **200/201** during capture.",
            results_success_by_folder,
        ),
        encoding="utf-8",
    )
    print(f"Wrote {out_success}")

    # GET + POST only (success 200/201), for frontend consumption
    get_post_success: dict[str, list[dict]] = defaultdict(list)
    for folder, items in results_success_by_folder.items():
        for r in items:
            if r.get("method") in ("GET", "POST"):
                get_post_success[folder].append(r)

    out_get_post_success = repo_root / "FRONTEND_API_DOCS_SUCCESS_GET_POST_REAL.md"
    out_get_post_success.write_text(
        render_md(
            "Frontend API Reference (SUCCESS GET + POST only, REAL responses)",
            "This document includes only **GET** and **POST** endpoints that returned **200/201** during capture.",
            get_post_success,
        ),
        encoding="utf-8",
    )
    print(f"Wrote {out_get_post_success}")

    # GET + POST only (ALL statuses), for accuracy / debugging prerequisites
    get_post_all: dict[str, list[dict]] = defaultdict(list)
    for folder, items in results_all_by_folder.items():
        for r in items:
            if r.get("method") in ("GET", "POST"):
                get_post_all[folder].append(r)

    out_get_post_all = repo_root / "FRONTEND_API_DOCS_GET_POST_ALL_REAL.md"
    out_get_post_all.write_text(
        render_md(
            "Frontend API Reference (GET + POST only, ALL statuses, REAL responses)",
            "This document includes **GET** and **POST** endpoints with their **real captured responses**, including non-200 statuses.",
            get_post_all,
        ),
        encoding="utf-8",
    )
    print(f"Wrote {out_get_post_all}")

    out_all = repo_root / "FRONTEND_API_DOCS_ALL_REAL.md"
    out_all.write_text(
        render_md(
            "Frontend API Reference (ALL endpoints, REAL responses)",
            "This document includes **all endpoints** (even if non-200) captured from the Postman collection.",
            results_all_by_folder,
        ),
        encoding="utf-8",
    )
    print(f"Wrote {out_all}")


if __name__ == "__main__":
    main()

