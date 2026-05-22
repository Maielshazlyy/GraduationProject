import json
from collections import defaultdict
from pathlib import Path


def iter_items(items, folder: str | None = None):
    for it in items:
        if isinstance(it, dict) and "item" in it and isinstance(it["item"], list):
            new_folder = ((folder + "/" if folder else "") + it.get("name", "")).strip("/")
            yield from iter_items(it["item"], new_folder)
        else:
            yield (folder or ""), it


def get_req(it: dict):
    req = it.get("request", {})
    method = req.get("method", "GET")
    url = req.get("url", {})
    raw = url.get("raw") if isinstance(url, dict) else str(url)
    body = req.get("body", {}) or {}

    body_text = None
    mode = body.get("mode")
    if mode == "raw":
        body_text = body.get("raw")
    elif mode == "urlencoded":
        body_text = json.dumps(
            {p.get("key"): p.get("value") for p in (body.get("urlencoded") or [])},
            indent=2,
            ensure_ascii=False,
        )

    return method, raw or "", body_text


def response_example(method: str, raw_url: str, name: str):
    url = raw_url or ""
    lname = (name or "").lower()

    # ---- Auth ----
    if "/api/auth/login" in url or "login" in lname:
        return {
            "token": "<jwt>",
            "expiresInMinutes": 60,
            "user": {"id": "<userId>", "fullName": "Business Owner", "email": "owner@app.com", "role": "Owner"},
        }
    if "/api/auth/register" in url or "register" in lname:
        return {"message": "User registered successfully.", "userId": "<userId>"}

    # ---- Dashboard ----
    if "/api/dashboard/analytics" in url:
        return {
            "businessName": "My Business",
            "businessType": "Restaurant",
            "totalOrders": 120,
            "totalRevenue": 5400.0,
            "averageOrderValue": 45.0,
            "pendingOrders": 5,
            "completedOrders": 110,
            "totalCustomers": 300,
            "newCustomersLast30Days": 25,
            "totalTickets": 12,
            "openTickets": 3,
            "closedTickets": 7,
            "inProgressTickets": 2,
            "averageTicketResolutionTime": 6.5,
            "averageRating": 4.3,
            "totalFeedbacks": 80,
            "positiveFeedbacks": 60,
            "negativeFeedbacks": 10,
            "positiveSentiments": 120,
            "negativeSentiments": 30,
            "neutralSentiments": 50,
            "averageSentimentScore": 0.42,
            "totalInteractions": 200,
            "activeInteractions": 6,
            "totalMenuItems": 40,
            "availableMenuItems": 36,
            "topOrderedProducts": [
                {
                    "menuItemId": "<menuItemId>",
                    "name": "Burger",
                    "totalQuantity": 120,
                    "ordersCount": 80,
                    "revenue": 1800.0,
                }
            ],
            "lastOrderDate": "2026-04-14T12:00:00Z",
            "lastTicketDate": "2026-04-13T09:00:00Z",
            "lastFeedbackDate": "2026-04-12T08:00:00Z",
        }
    if "/api/dashboard/summary" in url:
        return {
            "totalOrders": 120,
            "totalRevenue": 5400.0,
            "pendingOrders": 5,
            "completedOrders": 110,
            "cancelledOrders": 5,
            "totalCustomers": 300,
        }

    # ---- Generic CRUD heuristics ----
    if method == "GET":
        if any(p in url for p in ("/{", "{{")):
            return {"id": "<id>", "message": "Details object (see Swagger for full fields)."}
        return [{"id": "<id>"}]

    if method in ("POST", "PUT", "PATCH"):
        # Common success wrappers in this codebase
        if "suspend" in url or "activate" in url or "verify" in url:
            return {"message": "Action completed successfully."}
        return {"message": "Success"}

    if method == "DELETE":
        return {"message": "Deleted"}

    return {"message": "OK"}


def main():
    repo_root = Path(__file__).resolve().parents[1]
    coll_path = repo_root / "DigitalEmployee_Postman_Collection.json"

    coll = json.loads(coll_path.read_text(encoding="utf-8"))

    rows = []
    for folder, it in iter_items(coll.get("item", []) or []):
        name = it.get("name", "")
        method, raw, body = get_req(it)
        rows.append((folder, name, method, raw, body))

    by: dict[str, list[tuple[str, str, str, str | None]]] = defaultdict(list)
    for folder, name, method, raw, body in rows:
        by[folder].append((name, method, raw, body))

    md: list[str] = []
    md.append("## Frontend API Reference (from Postman)")
    md.append("")
    md.append(f"- **Source**: `{coll_path.name}`")
    md.append("- **Base URL**: `{{baseUrl}}`")
    md.append("- **Auth**: Most protected endpoints require `Authorization: Bearer {{token}}`.")
    md.append("")
    md.append(
        "> Note: Postman collections usually don't store example responses.\n"
        "> This document includes request bodies from the collection, and **best-effort response examples** (based on URL/method + common DTO patterns).\n"
        "> For exact response fields, use Swagger or capture real responses from a running backend."
    )
    md.append("")

    for folder in sorted(by.keys()):
        md.append(f"## {folder if folder else '(root)'}")
        md.append("")
        for name, method, raw, body in by[folder]:
            md.append(f"### {name}")
            md.append("")
            md.append(f"- **Method**: `{method}`")
            if raw:
                md.append(f"- **URL**: `{raw}`")

            if body and method in ("POST", "PUT", "PATCH"):
                txt = body
                try:
                    j = json.loads(body)
                    txt = json.dumps(j, indent=2, ensure_ascii=False)
                except Exception:
                    pass

                md.append("- **Body example**:")
                md.append("")
                md.append("```json")
                md.append(txt)
                md.append("```")

            # Response example (best-effort)
            md.append("- **Response example**:")
            md.append("")
            md.append("```json")
            md.append(json.dumps(response_example(method, raw, name), indent=2, ensure_ascii=False))
            md.append("```")
            md.append("")
        md.append("")

    out_path = repo_root / "FRONTEND_API_DOCS.md"
    out_path.write_text("\n".join(md), encoding="utf-8")
    print(f"Wrote {out_path} ({len(rows)} endpoints)")


if __name__ == "__main__":
    main()

