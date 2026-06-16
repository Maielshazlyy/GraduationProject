# Voice API — Frontend Contract

Base URL: `https://graduationproject.fly.dev/api`

---

## POST `/CustomerVoice/call/start`

يتبعت لما الـ Customer يدوس زرار "Call".

### Request
```json
{
  "businessId": "string",   // required
  "customerId": "string"    // required — الـ customer لازم يكون عنده account في النظام
}
```

### Response `200 OK`
```json
{
  "interactionId": "string",
  "meetingUrl": "string",
  "status": "connecting"
}
```

> بعد استلام الـ response، افتح الـ `meetingUrl` للـ Customer عشان يدخل المكالمة.

### Errors
| Status | السبب |
|--------|-------|
| `400`  | `businessId` أو `customerId` مش موجودين في الـ request، أو `MeetingUrl` مش متضبط في الـ Settings |
| `404`  | `businessId` أو `customerId` مش موجودين في الـ database |
| `500`  | خطأ داخلي |

---

## Flow كامل

```
Customer دوس "Call"
        ↓
POST /CustomerVoice/call/start  { businessId, customerId? }
        ↓
Backend يجيب MeetingUrl من Settings
        ↓
Backend يرجع { interactionId, meetingUrl, status: "connecting" }
        ↓
Frontend يفتح meetingUrl
        ↓
Customer + AI في المكالمة
        ↓
المكالمة تخلص (AI تتولى التخزين تلقائياً)
```

---

## ملاحظات

- الـ `MeetingUrl` بيتحدد من الـ Settings بتاع الـ business — الـ frontend مش محتاج يبعته.
- الـ `interactionId` احتفظ بيه — ممكن يتحتاج لو في feature جديد بعدين.
- مفيش feedback screen.
- مفيش polling — المكالمة بتخلص بشكل تلقائي من جهة الـ AI.
