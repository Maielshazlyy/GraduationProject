# AI Chatbot ↔ Backend Integration Contract

**Audience:** AI / Chatbot team
**Purpose:** Define exactly what the AI service must implement so the .NET backend can integrate with it.

---

## 1. Architecture principle

The **AI owns the conversation**; the **backend owns the database**.

- The AI handles: language understanding, intent detection, cart building, complaint/escalation detection, reply generation, and (for Voice) speech-to-text + text-to-speech.
- The backend handles: creating orders, tickets, and escalations in the database — driven entirely by the **signal flags** the AI returns.

> The AI must **not** create orders, tickets, or escalations on its own side. It only *signals* what it detected. The backend creates the records. Any `system_events` style side effects are ignored by the backend.

The backend does **no** intent detection, keyword matching, STT, or TTS. The AI is the single brain.

---

## 2. Endpoints the AI must expose

### 2.1 `POST /api/v1/session/init`  — (NEW)

Called **once** by the backend when a new conversation starts. It primes the AI with the business's menu and knowledge base so the AI can answer questions and produce correct order item names.

**Request body:**
```json
{
  "session_id": "8f3c...-guid",
  "business_id": "biz-123",
  "business_name": "Akila",
  "menu": [
    {
      "menu_item_id": "mi-1",
      "name": "Classic Burger",
      "description": "Beef patty, cheese, lettuce, pickles",
      "price": 110,
      "category": "Burgers",
      "is_available": true
    }
  ],
  "knowledge_base": [
    {
      "question": "ساعات العمل؟",
      "answer": "من 10 صباحاً حتى 2 بعد منتصف الليل",
      "is_faq": true
    }
  ]
}
```

**Required behavior:**
- Cache this payload **keyed by `session_id`**.
- Use `menu` to answer product/menu questions and to build orders.
- Use `knowledge_base` to answer business/context questions.
- Respect `is_available` on each item — see **Rule 4 (out of stock)** below.
- Respond `200 OK` (no body required).

---

### 2.2 `POST /api/v1/chat`  — (existing, must be updated)

Called by the backend for **every** customer message.

**Request body:**
```json
{
  "session_id": "8f3c...-guid",
  "business_id": "biz-123",
  "channel": "WebChat",
  "message": "عايز برجر"
}
```

For **Voice**, the request omits `message` and sends audio instead:
```json
{
  "session_id": "8f3c...-guid",
  "business_id": "biz-123",
  "channel": "Voice",
  "audio_data": "<base64 audio>",
  "audio_format": "audio/wav"
}
```

| Field | When | Meaning |
|---|---|---|
| `session_id` | always | Conversation id (matches the one from session/init) |
| `business_id` | always | Which business this conversation belongs to |
| `channel` | always | `"WebChat"` or `"Voice"` — tells the AI whether to do STT/TTS |
| `message` | WebChat | Customer text |
| `audio_data` | Voice | Base64 audio to transcribe |
| `audio_format` | Voice | e.g. `"audio/wav"`, `"audio/webm"` |

**Response body (the contract the backend reads):**
```json
{
  "session_id": "8f3c...-guid",
  "reply": "تمام يا فندم، سجلت طلبك",

  "transcript": "عايز برجر",
  "reply_audio": "<base64 audio>",
  "reply_audio_format": "audio/wav",

  "order_detected": true,
  "order_finalized": false,
  "order_details": {
    "items": [
      { "name": "Classic Burger", "quantity": 1, "price": 110, "notes": null }
    ],
    "total_amount": 110
  },

  "ticket_detected": false,
  "ticket_details": {
    "subject": "Customer Complaint",
    "description": "الأوردر وصل بارد",
    "priority": "high",
    "category": "complaint"
  },

  "escalation_requested": false,
  "feedback_requested": false
}
```

---

## 3. Response fields — meaning and when to set them

### Always
| Field | Type | Notes |
|---|---|---|
| `reply` | string | The natural-language reply shown to the customer. **Required.** |

### Voice only
| Field | Type | Notes |
|---|---|---|
| `transcript` | string | Speech-to-text of what the customer said. The backend stores this as the customer message. |
| `reply_audio` | string (base64) | Text-to-speech of `reply`. |
| `reply_audio_format` | string | e.g. `"audio/wav"`. |

### Order signals
| Field | Type | Notes |
|---|---|---|
| `order_detected` | bool | `true` while the customer is building a cart (browsing/adding). Informational. |
| `order_finalized` | bool | **`true` only when the customer confirms the final order** (e.g. "تمام كدة" / "confirm"). The backend creates the order in the DB on this flag. |
| `order_details.items[]` | array | The cart. Each item: `name` (exact menu name), `quantity`. `price`/`notes` optional. |
| `order_details.total_amount` | number | Informational — backend recomputes from its own menu prices. |

### Ticket signals
| Field | Type | Notes |
|---|---|---|
| `ticket_detected` | bool | `true` when a complaint/issue is detected. Backend creates a support ticket. |
| `ticket_details.subject` | string | Short subject. |
| `ticket_details.description` | string | The customer's issue in their words. |
| `ticket_details.priority` | string | `low` / `normal` / `high` / `critical`. |
| `ticket_details.category` | string | One of: `complaint`, `quality`, `delivery`, `wrong_order`, `missing`, `payment`. |

### Escalation & feedback
| Field | Type | Notes |
|---|---|---|
| `escalation_requested` | bool | `true` when the customer asks for a human agent OR high frustration is detected. Backend creates a HumanEscalation ticket. |
| `feedback_requested` | bool | `true` when the AI wants the customer prompted for a rating. Backend forwards this flag to the frontend. |

---

## 4. CRITICAL RULES

### Rule 1 — Add `order_finalized`
The current responses only have `order_detected`. **That is not enough.** `order_detected` stays `true` for the whole ordering conversation (every "add this", "add that"). If the backend created an order every time, it would create many duplicate orders.

- `order_detected: true` → cart is being built (no DB action)
- `order_finalized: true` → customer confirmed → **backend saves the order**

Without `order_finalized`, **no order is ever saved.**

### Rule 2 — Echo exact menu names
`order_details.items[].name` **must be the exact `name` string** from the menu sent in `session/init`. The backend matches names exactly (case-insensitive, trimmed). A paraphrased, translated, or partial name will cause that item to be **silently dropped** from the order.

✅ `"Classic Burger"` (matches menu)
❌ `"classic burger sandwich"`, `"كلاسيك برجر"`, `"Burger"`

### Rule 3 — Do not create records on the AI side
The AI must only **signal** intent through the flags above. It must **not** create orders, tickets, or escalations in its own system. The backend is the single source of truth for those records.

### Rule 4 — Out of stock: recommend alternatives
Each menu item in `session/init` carries an `is_available` flag.

When the customer asks to order an item where `is_available` is `false`:
- **Do not** add it to the cart and **do not** include it in a finalized order.
- Tell the customer it's currently unavailable and **recommend available alternatives** — preferably items from the **same `category`** that have `is_available: true`.

Example — customer asks for "Classic Burger" but it's out of stock:
```json
{
  "reply": "معلش يا فندم، الـ Classic Burger خلص دلوقتي. تحب تجرب الـ Crispy Chicken Burger أو الـ Grilled Chicken Burger؟",
  "order_detected": true,
  "order_finalized": false,
  "order_details": { "items": [], "total_amount": 0 }
}
```

> The backend uses `is_available` as a safety net (it will not save an unavailable item even if it somehow appears in a finalized order), but the customer experience — recommending an alternative — is the **AI's responsibility**, because the AI holds the menu + availability for the session.

---

## 5. Worked examples (from the agreed test scenarios)

**Customer browsing menu** — no DB action:
```json
{ "reply": "عندنا برجر لحم وفراخ ...", "order_detected": false }
```

**Adding items (cart building)** — no DB action yet:
```json
{
  "reply": "تمام، ضفت Classic Burger",
  "order_detected": true,
  "order_finalized": false,
  "order_details": { "items": [ {"name":"Classic Burger","quantity":1} ], "total_amount": 110 }
}
```

**Customer confirms order** — backend creates the order:
```json
{
  "reply": "تمام يا فندم، جاري تحضير طلبك",
  "order_detected": true,
  "order_finalized": true,
  "order_details": {
    "items": [
      {"name":"Classic Burger","quantity":1},
      {"name":"Onion Rings","quantity":1},
      {"name":"Pepsi / Pepsi Diet","quantity":1}
    ],
    "total_amount": 185
  }
}
```

**Complaint** — backend creates a ticket:
```json
{
  "reply": "معلش يا فندم، هسجل الموضوع لفريق الجودة",
  "ticket_detected": true,
  "ticket_details": {
    "subject": "Customer Complaint",
    "description": "الاوردر وصل بارد مع انى مستنى ساعة",
    "priority": "high",
    "category": "delivery"
  }
}
```

**Customer wants a manager + bad rating** — backend escalates + frontend asks for rating:
```json
{
  "reply": "حد من الإدارة هيرد على حضرتك دلوقتي",
  "escalation_requested": true,
  "feedback_requested": true
}
```

---

## 6. Summary checklist for the AI team

- [ ] Implement `POST /api/v1/session/init`; cache menu + KB by `session_id`.
- [ ] Update `POST /api/v1/chat` to read `business_id`, `channel`, and (Voice) `audio_data`/`audio_format`.
- [ ] Return the full response shape in section 2.2.
- [ ] **Add `order_finalized`** — true only on final confirmation.
- [ ] **Echo exact menu names** in `order_details.items[].name`.
- [ ] **Stop creating orders/tickets** on the AI side — signal only.
- [ ] **Out of stock** — never order an `is_available: false` item; recommend available alternatives (same category).
- [ ] Voice: return `transcript` (STT) and `reply_audio` (TTS).
