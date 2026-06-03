# AI Integration — Endpoints & Testing Report
**Date:** 2026-06-03  
**IRIS URL:** `https://anyway-remix-puzzling.ngrok-free.dev`  
**Backend URL:** `https://localhost:44361`

---

## 1. Endpoints

### Customer Chat
| Method | Endpoint | Auth | Body |
|--------|----------|------|------|
| POST | `/api/CustomerChat/message` | None | `{ businessId, message, interactionId? }` |
| POST | `/api/CustomerChat/interaction/{id}/end` | None | — |
| GET | `/api/CustomerChat/capabilities/{businessId}` | None | — |

### Owner Analytics Chat
| Method | Endpoint | Auth | Body |
|--------|----------|------|------|
| POST | `/api/OwnerChat/message` | Owner/Admin | `{ "message": "..." }` |
| GET | `/api/OwnerChat/report` | Owner/Admin | — |
| POST | `/api/OwnerChat/reload` | Owner/Admin | — |

### Knowledge Base / FAQ
| Method | Endpoint | Auth | Body |
|--------|----------|------|------|
| POST | `/api/KnowledgeBase` | Owner/Admin | `{ question, answer, businessId, isFAQ, displayOrder, isActive }` |
| PUT | `/api/KnowledgeBase/{id}` | Owner/Admin | same fields |
| DELETE | `/api/KnowledgeBase/{id}` | Owner/Admin | — |

### Dashboard
| Method | Endpoint | Auth | Body |
|--------|----------|------|------|
| GET | `/api/Dashboard/overview/{businessId}` | Owner/Admin | query params: `insightsPeriod`, `revenuePeriod`, `chatAnalysisPeriod`, `feedbacksPeriod`, etc. |

---

## 2. IRIS Endpoints Used

| Method | Path | Triggered By |
|--------|------|-------------|
| POST | `/api/v1/business/knowledge-base/sync` | Auto on menu/FAQ create/update/delete |
| POST | `/api/v1/chat` | Every `/api/CustomerChat/message` call |
| POST | `/api/v1/analysis/chat-batch` | Auto after `/interaction/{id}/end` |
| POST | `/api/v1/owner/chat` | `/api/OwnerChat/message` |
| GET | `/api/v1/owner/report` | `/api/OwnerChat/report` |
| POST | `/api/v1/owner/reload` | `/api/OwnerChat/reload` |

---

## 3. Test Data

**Business:** Test Restaurant  
**BusinessId:** `d52f7fcc-8b2e-4b03-ac4c-c16760b80468`  
**Token:** Owner JWT (expires 2026-06-06)

**Menu Items:**
| Name | Price | ID |
|------|-------|-----|
| Classic Burger | 120 EGP | `c080e3d6-02f5-46b0-ba9b-c1c054f6f9c0` |
| Crispy Chicken Burger | 110 EGP | `0036b8eb-1e47-4945-8e49-c895ba2c36b6` |
| Lemon Mint | 45 EGP | `900982cf-d798-4878-8998-f9b7bc5024f9` |
| Pepsi | 25 EGP | `7b15737c-0a91-4214-abe2-b429c4c65984` |

**FAQs:**
- "كام ساعة التوصيل؟" → "التوصيل بياخد من 30 لـ 45 دقيقة."
- "في حد أوردر minimum؟" → "أقل أوردر 50 جنيه."

---

## 4. Testing Results

### Test 1 — General Question ✓
```
POST /api/CustomerChat/message
{ "businessId": "...", "message": "عندكم ايه؟" }

Response:
{
  "interactionId": "8d0d2c2d-...",
  "replyText": "عندنا مشروبات وأكلات متنوعة. ممكن تطلب Lemon Mint بسعر 45 جنيه، أو Classic Burger بسعر 120 جنيه..."
}
```

---

### Test 2 — FAQ Question ✓
```
POST /api/CustomerChat/message
{ "businessId": "...", "message": "كام ساعة التوصيل؟" }

Response:
{ "replyText": "التوصيل بياخد من 30 لـ 45 دقيقة." }
```

---

### Test 3 — Order Flow ✓
```
Message 1: "عايز Classic Burger"
→ replyText: "تحب أضيف Classic Burger بسعر 120 جنيه لطلبك؟"
→ orderId: null

Message 2: "أيوه كدة تمام"
→ replyText: "تمام يا فندم، كده الطلب اتأكد."
→ orderId: "38264e71-3588-44eb-8ace-26ec982be64d"
→ cart: { totalPrice: 120, items: [Classic Burger x1 @ 120] }
```

---

### Test 4 — Complaint → Ticket ✓
```
Message: "الأوردر وصل بارد"
→ replyText: "معلش يا فندم، هسجل المشكلة لفريق الدعم."
→ ticketId: "2f4017f9-d4e1-446e-b730-e266f10d6660"
→ type: QualityIssue, priority: High
```

---

### Test 5 — Escalation ✓
```
Message: "عايز أكلم المدير"
→ replyText: "هحوّل حضرتك لحد من الإدارة."
→ ticketId: "7787e5e3-a5f7-42f5-a743-2f0bfd78e15b"
→ type: HumanEscalation, priority: High
```

---

### Test 6 — End Interaction + Analysis ✓
```
POST /api/CustomerChat/interaction/8d0d2c2d-.../end

→ status: Closed
→ InteractionAnalysis created in DB:
  {
    "mainIntent": "Complaint",
    "sentimentLabel": "Negative",
    "sentimentScore": -0.5,
    "summary": "Customer ordered a Classic Burger but received it cold. They requested to speak to a manager.",
    "summaryAr": "العميل طلب برجر كلاسيك لكن وصله بارد. طلب يتكلم مع المدير.",
    "topIntents": ["Complaint x3", "RequestHumanAgent x1", "CreateOrder x1"],
    "topTopics": ["order issue", "cold food", "manager request"],
    "keyMoments": ["Customer ordered Classic Burger", "Received cold order", "Requested to speak to manager"]
  }
```

---

### Test 7 — Revenue After Order Status Update ✓
```
PUT /api/Order/38264e71-.../status
{ "status": "Delivered" }

Dashboard result:
→ totalRevenue: 120 EGP
→ revenueTrend 06-03: { revenue: 120, orderCount: 1 }
→ topProducts: [Classic Burger - 120 EGP - 1 unit]
→ customerLeads[0].totalSpend: 120 EGP
```

---

### Test 8 — Full Dashboard ✓
```
GET /api/Dashboard/overview/d52f7fcc-...

→ totalRevenue: 120 EGP ✓
→ totalInteractions: 3 ✓
→ avgOrderValue: 120 EGP ✓
→ newCustomers: 3 ✓
→ openTickets: 1 ✓
→ channelDistribution: WebChat 100% ✓
→ recentAlerts: HumanEscalation + QualityIssue ✓
→ chatAnalysis.totalAnalyzedSessions: 1 ✓
→ chatAnalysis.topIntents: Complaint, RequestHumanAgent, CreateOrder ✓
→ chatAnalysis.recentSessions: full session with EN + AR summary ✓
→ revenueTrend: 06-03 = 120 EGP ✓
→ topProducts: Classic Burger ✓
```

---

## 5. Notes

- Revenue counts only orders with status `Delivered` or `Paid`
- IRIS is in-memory — KB sync must re-run after IRIS restart
- Analysis runs in background after interaction ends (~5 seconds)
- Ticket priority normalized from lowercase (`"high"` → `"High"`) automatically
