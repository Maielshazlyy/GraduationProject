# Chat Contract — Final (Backend ↔ AI)

> Last updated after full contract review + AI team Q&A.
> This file is the **single source of truth** for the Chat integration.
> Ignore all previous contract files.

---

## Base URL

Configured in backend `appsettings.json` under `AiService:BaseUrl`.
All paths below are relative to that base URL.

---

## 1. Knowledge Base Sync

**Direction:** Backend → AI
**Trigger:** Automatically called whenever:
- A new business is created
- Any menu item is added / updated / deleted
- Any knowledge base entry is added / updated / deleted

**Endpoint:** `POST /api/v1/business/knowledge-base/sync`

### Request Body

```json
{
  "business_id": "string",
  "business_name": "string",

  "knowledge_base": {
    "menu_items": [
      {
        "menu_item_id": "string",
        "name": "string",
        "description": "string | null",
        "price": 49.99,
        "category": "string | null",
        "is_available": true
      }
    ],
    "faqs": [
      {
        "question": "string",
        "answer": "string",
        "is_faq": true
      }
    ]
  }
}
```

### Field Notes

| Field | Notes |
|-------|-------|
| `business_id` | AI must index and cache this data by `business_id` |
| `menu_items[].name` | **Exact canonical name** — AI must echo this exactly in `order_details.items[].name` |
| `menu_items[].is_available` | AI uses this to filter what it offers customers |
| `faqs[].is_faq` | `true` = explicit FAQ / `false` = general KB entry. Both must be used for answering |

### Expected Response

```
HTTP 200 OK
(no body required)
```

### Rules

- No `session_id` — this is not session-based.
- AI caches data by `business_id` and uses it during every chat for that business.
- Each call **replaces** the entire KB for that business (full sync, not delta).

---

## 2. Chat Message

**Direction:** Backend → AI (per message)
**Endpoint:** `POST /api/v1/chat`

### Request Body

```json
{
  "session_id": "string",
  "business_id": "string",
  "message": "string"
}
```

| Field | Notes |
|-------|-------|
| `session_id` | Maps to `Interaction.InteractionId` in backend DB. Same value for the **entire** conversation. |
| `business_id` | AI uses this to load the correct cached KB |
| `message` | Customer's text message |

---

## 3. Chat Response

**Direction:** AI → Backend

```json
{
  "session_id": "string",
  "reply": "string",

  "order_detected": false,
  "order_finalized": false,
  "order_details": {
    "intent": "string | null",
    "items": [
      {
        "name": "string",
        "quantity": 1,
        "price": 49.99,
        "notes": "string | null"
      }
    ],
    "total_amount": 149.97
  },

  "ticket_detected": false,
  "ticket_details": {
    "subject": "string",
    "description": "string | null",
    "priority": "low | normal | high | critical",
    "category": "string | null"
  },

  "escalation_requested": false,
  "feedback_requested": false,

  "processing_time_ms": 120
}
```

---

## 4. Response Field Rules

### Order Fields

| Field | When | Backend action |
|-------|------|----------------|
| `order_detected: true` | Customer is building a cart | **No DB action** — just show reply |
| `order_finalized: true` | Customer confirmed the order | **Create Order in DB** |
| `order_detected` when `order_finalized: true` | Always `true` as well | `order_finalized` implies `order_detected` |
| `order_details` | Present when `order_detected` OR `order_finalized` is true | Backend matches `items[].name` → `MenuItem.Name` to resolve `MenuItemId` |
| `order_details.items[].price` | Always present | **Informational only** — backend uses menu DB price |
| `order_details.total_amount` | Always present | **Informational only** — backend recomputes from menu DB prices |

### Ticket Fields

| Field | When | Backend action |
|-------|------|----------------|
| `ticket_detected: true` | Complaint / issue detected | **Create Complaint Ticket in DB** |
| `ticket_details.priority` | Values: `low`, `normal`, `high`, `critical` | Maps to `Ticket.PriorityLevel` |
| `ticket_details.category` | e.g. `complaint`, `quality`, `delivery`, `payment`, `wrong_order` | Maps to `Ticket.TicketType` |

### Escalation & Feedback

| Field | Backend action |
|-------|----------------|
| `escalation_requested: true` | Create **HumanEscalation** ticket + set `Interaction.Status = "Escalated"` |
| `feedback_requested: true` | Prompt customer for a rating (1–5). Backend stores feedback in DB. |

### Combined Signals

`ticket_detected` and `escalation_requested` **can both be `true`** in the same response.
Example: customer complains AND explicitly asks for a human agent.

```json
{ "ticket_detected": true, "escalation_requested": true }
```

Backend creates both a Complaint Ticket and a HumanEscalation Ticket in this case.

---

## 5. Session Rules (Confirmed)

| Rule | Detail |
|------|--------|
| **History ownership** | AI maintains conversation history internally by `session_id`. Backend does NOT need to resend previous messages. |
| **session_id reuse** | A closed interaction's `session_id` must **never** be reused. Each new interaction gets a new `session_id`. If the same `session_id` is sent after closure, the AI treats it as a continuation of the same conversation. |
| **Session expiry (AI side)** | AI auto-clears session history after **2 hours of inactivity**. After expiry, the same `session_id` is treated as a new session by the AI. This is safe — backend is the source of truth for message storage. |

---

## 6. Full Flow

```
1. Business setup / data change
   └─► Backend → POST /api/v1/business/knowledge-base/sync
       └─► AI indexes KB by business_id

2. Customer sends a message
   └─► Backend → POST /api/v1/chat  { session_id, business_id, message }
       └─► AI looks up KB by business_id, uses session_id to recall history
       └─► AI returns response

3. Backend reads signals:
   ├── order_finalized = true        → Create Order in DB
   ├── ticket_detected = true        → Create Complaint Ticket in DB
   └── escalation_requested = true   → Create Escalation Ticket + update Interaction.Status

4. Same session_id for all messages in one conversation.
   New conversation = new session_id (never reuse a closed one).

5. Conversation ends (EndInteraction):
   └─► Backend triggers post-session Analysis
       (see AI_ANALYSIS_CONTRACT_FINAL.md)
```
