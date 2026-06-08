# Voice API — Frontend Contract

Base URL: `https://<your-domain>/api`

---

## POST `/CustomerVoice/call/start`

يتبعت لما الـ Customer يدوس زرار "Call".

### Request
```json
{
  "businessId": "string",   // required
  "meetingUrl": "string",   // required — الـ URL اللي عند الـ Frontend
  "customerId": "string"    // optional — لو الـ customer معروف
}
```

### Response `200 OK`
```json
{
  "interactionId": "string",
  "status": "connecting"
}
```

> بعد استلام الـ response، افتح الـ `meetingUrl` للـ Customer عشان يدخل المكالمة.

### Errors
| Status | السبب |
|--------|-------|
| `400`  | `meetingUrl` مش موجود في الـ request |
| `404`  | `businessId` مش موجود |
| `500`  | خطأ داخلي |

---

## Flow كامل

```
Customer دوس "Call"
        ↓
POST /CustomerVoice/call/start  { businessId, meetingUrl, customerId? }
        ↓
Backend يرجع { interactionId, status: "connecting" }
        ↓
Frontend يفتح meetingUrl
        ↓
Customer + AI في المكالمة
        ↓
المكالمة تخلص (AI تتولى التخزين تلقائياً)
```

---

## ملاحظات

- الـ `interactionId` احتفظ بيه — ممكن يتحتاج لو في feature جديد بعدين.
- مفيش feedback screen.
- مفيش polling — المكالمة بتخلص بشكل تلقائي من جهة الـ AI.
