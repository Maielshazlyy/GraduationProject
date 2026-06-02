# Frontend ↔ Backend Contract — V2
# All endpoints the frontend needs, with exact request/response shapes

**Version:** 2.0  
**Last Updated:** 2026-05-28  
**Base URL:** `https://graduationproject.fly.dev`  
**Audience:** Frontend Team

---

## General Rules

**Auth:** Every request (except login/register) must include:
```
Authorization: Bearer {jwt_token}
```

**Error shape (always):**
```json
{ "message": "Human readable error", "errors": [] }
```

**HTTP status codes:**
- `200` OK
- `201` Created
- `204` No Content (delete/end operations)
- `400` Validation failed
- `401` Not authenticated → redirect to login
- `403` Not authorized → show "Access Denied"
- `404` Not found → show empty state
- `500` Server error → show retry button

**Null handling:** All optional fields may be `null`. Never assume a field exists.

---

## 1. Authentication

### Register Owner
```
POST /api/Auth/register-owner
```
```json
{ "fullName": "string", "email": "string", "password": "string" }
```
Response `200`:
```json
{
  "userId": "guid",
  "email": "string",
  "fullName": "string",
  "role": "Owner",
  "token": "jwt_string",
  "expiration": "2026-05-29T14:00:00Z",
  "businessId": null
}
```

### Login
```
POST /api/Auth/login
```
```json
{ "email": "string", "password": "string" }
```
Response `200`: same shape as register. Store `token`, `role`, `businessId`.

### Get Current User Profile
```
GET /api/Auth/profile
```
Response `200`:
```json
{
  "userId": "guid",
  "email": "string",
  "fullName": "string",
  "role": "Owner | Admin | Agent",
  "businessId": "guid | null"
}
```

---

## 2. Business Setup

### Create Business (Owner — first time only)
```
POST /api/Business
```
```json
{
  "name": "string",
  "description": "string",
  "businessType": "string",
  "phone": "string",
  "email": "string",
  "address": "string"
}
```
Response `201`: Full business object. Save returned `id` as `businessId`.

### Get My Business
```
GET /api/Business/my-business
```
Response `200`:
```json
{
  "id": "guid",
  "name": "string",
  "description": "string",
  "businessType": "string",
  "phone": "string",
  "email": "string",
  "address": "string",
  "isActive": true
}
```

### Update Business
```
PUT /api/Business/{id}
```
Same body as create. Response `200`: updated object.

---

## 3. Owner Dashboard

### Summary Card
```
GET /api/Dashboard/summary
```
Response `200`:
```json
{
  "businessName": "string",
  "businessType": "string",
  "isSetupComplete": true,
  "setupStepsCompleted": ["string"],
  "setupStepsPending": ["string"],
  "hasSettings": true,
  "chatbotEnabled": true,
  "welcomeMessage": "string",
  "agentVoice": "string"
}
```

### Analytics KPIs
```
GET /api/Dashboard/analytics
```
Response `200`:
```json
{
  "totalOrders": 120,
  "totalRevenue": 6500.0,
  "averageOrderValue": 54.16,
  "pendingOrders": 5,
  "completedOrders": 110,
  "totalCustomers": 84,
  "newCustomersLast30Days": 12,
  "totalTickets": 22,
  "openTickets": 4,
  "closedTickets": 16,
  "inProgressTickets": 2,
  "averageTicketResolutionTime": "04:32:00",
  "averageRating": 4.3,
  "totalFeedbacks": 30,
  "positiveFeedbacks": 24,
  "negativeFeedbacks": 3,
  "positiveSentiments": 95,
  "negativeSentiments": 8,
  "neutralSentiments": 17,
  "averageSentimentScore": 0.63,
  "totalInteractions": 240,
  "activeInteractions": 3,
  "totalMenuItems": 18,
  "availableMenuItems": 15
}
```

### Full Dashboard (Summary + Analytics in one call)
```
GET /api/Dashboard/full
```
Response `200`: merged object of both above responses.

---

## 4. Conversation Sessions (NEW — Voice AI Data)

### List All Sessions for This Business
```
GET /api/conversations/sessions?from=2026-05-01&to=2026-05-28&page=1&pageSize=20
```
Response `200`:
```json
{
  "sessions": [
    {
      "sessionId": "guid",
      "startedAt": "2026-05-28T14:00:00Z",
      "endedAt": "2026-05-28T14:12:30Z",
      "durationSeconds": 750,
      "channel": "VoiceDesktop",
      "status": "Closed",
      "customerIdentifier": "anon | phone:+201001234567",
      "overallSentimentScore": 0.72,
      "overallSentimentLabel": "Positive",
      "totalTurns": 12,
      "customerTurns": 6,
      "escalationRequired": false,
      "actionsPerformed": ["OrderCreated"]
    }
  ],
  "total": 42,
  "page": 1,
  "pageSize": 20
}
```

### Get Full Transcript of One Session
```
GET /api/conversations/sessions/{sessionId}/transcript
```
Response `200`:
```json
{
  "sessionId": "guid",
  "status": "Closed",
  "startedAt": "2026-05-28T14:00:00Z",
  "endedAt": "2026-05-28T14:12:30Z",
  "durationSeconds": 750,
  "turns": [
    {
      "turnIndex": 1,
      "timestamp": "2026-05-28T14:00:05Z",
      "speaker": "Customer",
      "transcript": "عايز أطلب برجر كبير",
      "audioLengthMs": 2400,
      "language": "ar",
      "dialect": "Egyptian",
      "sentiment": { "score": 0.55, "label": "Positive" },
      "intentDetected": "CreateOrder",
      "confidence": 0.93,
      "actionTaken": null
    },
    {
      "turnIndex": 2,
      "timestamp": "2026-05-28T14:00:09Z",
      "speaker": "Agent",
      "transcript": "تمام! سجلت لك برجر كبير. عايز تضيف بطاطس أو مشروب؟",
      "audioLengthMs": 3200,
      "language": "ar",
      "dialect": "Egyptian",
      "sentiment": null,
      "intentDetected": null,
      "confidence": null,
      "actionTaken": {
        "type": "OrderCreated",
        "referenceId": "ord-456",
        "details": { "items": ["Big Burger"], "totalPrice": 50 }
      }
    }
  ],
  "analysis": {
    "summaryEn": "Customer ordered a large burger. Satisfied throughout.",
    "summaryAr": "العميل طلب برجر كبير. كان راضياً طوال المحادثة.",
    "overallSentiment": { "score": 0.72, "label": "Positive" },
    "mainTopics": ["Food Order", "Upsell"],
    "intentsDetected": [
      { "intent": "CreateOrder", "count": 2 },
      { "intent": "Farewell", "count": 1 }
    ],
    "actionsPerformed": [
      { "type": "OrderCreated", "referenceId": "ord-456", "turnIndex": 2 }
    ],
    "escalationRequired": false,
    "escalationReason": null,
    "keyMoments": [
      { "turnIndex": 2, "description": "Order placed — Big Burger 50 EGP" }
    ],
    "modelsUsed": ["whisper-large-v3", "gpt-4o", "tts-1-hd"],
    "languageSwitches": 0,
    "avgCustomerTurnLengthMs": 2100,
    "avgAgentResponseDelayMs": 800
  }
}
```

### Get Conversation Analytics (Charts Data)
```
GET /api/conversations/analytics?from=2026-05-01&to=2026-05-28
```
Response `200`:
```json
{
  "totalSessions": 42,
  "totalDurationSeconds": 31500,
  "avgDurationSeconds": 750,
  "avgSentimentScore": 0.65,
  "sentimentBreakdown": {
    "Positive": 30,
    "Neutral": 8,
    "Negative": 4
  },
  "escalationRate": 0.05,
  "escalatedSessions": 2,
  "topIntents": [
    { "intent": "CreateOrder", "count": 88 },
    { "intent": "AskAboutProducts", "count": 42 },
    { "intent": "Complaint", "count": 7 }
  ],
  "topActions": [
    { "action": "OrderCreated", "count": 75 },
    { "action": "TicketCreated", "count": 7 }
  ],
  "sentimentOverTime": [
    { "date": "2026-05-01", "avgScore": 0.60, "sessionCount": 3 },
    { "date": "2026-05-02", "avgScore": 0.71, "sessionCount": 5 }
  ],
  "sessionsPerDay": [
    { "date": "2026-05-01", "count": 3 },
    { "date": "2026-05-02", "count": 5 }
  ],
  "languageBreakdown": {
    "ar": 35,
    "en": 7
  },
  "avgAgentResponseDelayMs": 820
}
```

---

## 5. Orders

### List Orders
```
GET /api/Order?page=1&pageSize=20&status=Pending
```
Response `200`:
```json
{
  "orders": [
    {
      "id": "guid",
      "customerId": "guid",
      "customerName": "string",
      "status": "Pending | InProgress | Completed | Cancelled",
      "totalPrice": 75.0,
      "createdAt": "2026-05-28T14:00:00Z",
      "items": [
        { "menuItemId": "guid", "menuItemName": "Big Burger", "quantity": 1, "unitPrice": 50.0 }
      ]
    }
  ],
  "total": 120,
  "page": 1,
  "pageSize": 20
}
```

### Update Order Status
```
PATCH /api/Order/{id}/status
```
```json
{ "status": "InProgress" }
```
Response `200`: updated order.

---

## 6. Tickets

### List Tickets
```
GET /api/Ticket?page=1&pageSize=20&status=Open
```
Response `200`:
```json
{
  "tickets": [
    {
      "id": "guid",
      "subject": "string",
      "description": "string",
      "status": "Open | InProgress | Closed",
      "priority": "Low | Normal | High | Critical",
      "customerId": "guid",
      "customerName": "string",
      "linkedSessionId": "guid | null",
      "createdAt": "2026-05-28T14:00:00Z",
      "resolvedAt": "string | null"
    }
  ],
  "total": 22,
  "page": 1,
  "pageSize": 20
}
```

Note: `linkedSessionId` will be non-null for tickets auto-created by AI escalations. The frontend can use it to navigate directly to the conversation transcript.

### Update Ticket Status
```
PATCH /api/Ticket/{id}/status
```
```json
{ "status": "InProgress", "note": "Looking into it now" }
```
Response `200`: updated ticket.

---

## 7. Menu

### List Categories with Items
```
GET /api/MenuCategory?includeItems=true
```
Response `200`:
```json
{
  "categories": [
    {
      "id": "guid",
      "name": "Burgers",
      "displayOrder": 1,
      "items": [
        {
          "id": "guid",
          "name": "Big Burger",
          "description": "string",
          "price": 50.0,
          "isAvailable": true,
          "imageUrl": "string | null"
        }
      ]
    }
  ]
}
```

### Create Menu Item
```
POST /api/MenuItem
```
```json
{
  "name": "string",
  "description": "string",
  "price": 50.0,
  "categoryId": "guid",
  "isAvailable": true
}
```
Response `201`: created item.

### Toggle Availability
```
PATCH /api/MenuItem/{id}/availability
```
```json
{ "isAvailable": false }
```
Response `200`: updated item.

---

## 8. Knowledge Base

### List FAQs
```
GET /api/KnowledgeBase
```
Response `200`:
```json
{
  "items": [
    {
      "id": "guid",
      "question": "string",
      "answer": "string",
      "category": "string | null",
      "createdAt": "string"
    }
  ]
}
```

### Create FAQ
```
POST /api/KnowledgeBase
```
```json
{ "question": "string", "answer": "string", "category": "string" }
```
Response `201`: created item.

---

## 9. Feedback

### List Feedback
```
GET /api/Feedback?page=1&pageSize=20
```
Response `200`:
```json
{
  "feedbacks": [
    {
      "id": "guid",
      "customerId": "guid",
      "customerName": "string",
      "rating": 5,
      "comment": "string",
      "linkedSessionId": "guid | null",
      "createdAt": "string"
    }
  ],
  "total": 30,
  "averageRating": 4.3
}
```

---

## 10. Settings (Chatbot & Voice Config)

### Get Settings
```
GET /api/Setting
```
Response `200`:
```json
{
  "id": "guid",
  "chatbotEnabled": true,
  "welcomeMessage": "string",
  "agentName": "string",
  "agentVoice": "ar-EG-SalmaNeural",
  "agentVoiceProvider": "Azure",
  "agentVoiceSpeed": 1.0,
  "agentVoicePitch": 1.0,
  "agentVoiceLanguage": "ar",
  "defaultLanguage": "ar",
  "supportedLanguages": ["ar", "en"]
}
```

### Update Settings
```
PUT /api/Setting
```
Same shape as GET response (minus `id`). Response `200`: updated settings.

---

## 11. Audit Logs

### Recent Logs
```
GET /api/Dashboard/audit-logs/recent?count=20
```
Response `200`:
```json
{
  "logs": [
    {
      "id": "guid",
      "action": "OrderStatusUpdated",
      "entity": "Order",
      "entityId": "guid",
      "userId": "guid",
      "userName": "string",
      "details": "string | null",
      "createdAt": "string"
    }
  ]
}
```

---

## 12. AI Session Token (Frontend → AI Handoff)

The frontend needs to give the AI app permission to write to a specific session on behalf of this business. It gets a short-lived token from the backend and passes it to the AI app.

### Get Session Token
```
POST /api/conversations/sessions/token
```
```json
{ "businessId": "guid" }
```
Response `200`:
```json
{
  "sessionToken": "short_lived_opaque_token",
  "expiresAt": "2026-05-28T15:00:00Z"
}
```

The frontend passes `sessionToken` to the desktop AI app via its local settings or a deep link. The AI app then uses this token in its own calls to the backend's session endpoints. This token is scoped to this business only and expires in 1 hour.

---

## Frontend Page → Endpoint Mapping

| Page | Endpoints Used |
|------|---------------|
| Login | `POST /api/Auth/login` |
| Onboarding | `POST /api/Business`, `PUT /api/Setting` |
| Dashboard Home | `GET /api/Dashboard/full` |
| Conversation History | `GET /api/conversations/sessions` |
| Conversation Detail | `GET /api/conversations/sessions/{id}/transcript` |
| Voice Analytics | `GET /api/conversations/analytics` |
| Orders | `GET /api/Order`, `PATCH /api/Order/{id}/status` |
| Tickets | `GET /api/Ticket`, `PATCH /api/Ticket/{id}/status` |
| Menu Manager | `GET /api/MenuCategory?includeItems=true`, `POST/PUT/DELETE /api/MenuItem` |
| Knowledge Base | `GET/POST/PUT/DELETE /api/KnowledgeBase` |
| Feedback | `GET /api/Feedback` |
| Settings | `GET /api/Setting`, `PUT /api/Setting` |
| Audit Logs | `GET /api/Dashboard/audit-logs/recent` |
| AI App Setup | `POST /api/conversations/sessions/token` |
