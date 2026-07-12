# Voice API — Frontend Contract

Base URL: `https://graduationproject.fly.dev/api`

---

## Flow كامل

```
1. العميل يضغط "اتصل"
        ↓
2. POST /CustomerVoice/call/start
        ↓
3. الفرونت يفتح meetingUrl
        ↓
4. العميل يتكلم مع الذكاء الاصطناعي
        ↓
5. المكالمة تخلص — الذكاء الاصطناعي يرفع التسجيل تلقائياً
        ↓
6. الذكاء الاصطناعي يبعت التقرير تلقائياً
        ↓
7. الفرونت يجيب التقرير والتسجيل
```

---

## 1. بدء المكالمة

**`POST /CustomerVoice/call/start`**

### Request
```json
{
  "businessId": "string",
  "customerId": "string"
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

### Errors
| Status | السبب |
|--------|-------|
| `400` | `businessId` أو `customerId` ناقصين، أو `MeetingUrl` مش متضبط في الـ Settings |
| `404` | `businessId` أو `customerId` مش موجودين في قاعدة البيانات |
| `500` | خطأ داخلي |

> بعد استلام الـ response، افتح الـ `meetingUrl` للعميل عشان يدخل المكالمة.

---

## 2. رفع التسجيل (الذكاء الاصطناعي فقط)

**`POST /CallRecording/upload`**

multipart/form-data

| الحقل | النوع | مطلوب | الوصف |
|-------|-------|--------|-------|
| `file` | ملف `.wav` | نعم | ملف الصوت |
| `callId` | نص | نعم | نفس الـ `interactionId` |
| `speaker` | نص | لا | `customer` أو `agent` أو `stereo` |

### Response `200 OK`
```json
{
  "message": "Recording uploaded.",
  "callId": "string",
  "speaker": "string",
  "fileName": "string",
  "sizeBytes": 0,
  "downloadUrl": "https://graduationproject.fly.dev/api/CallRecording/{callId}/{fileName}"
}
```

---

## 3. تقرير المكالمة (الذكاء الاصطناعي فقط)

**`POST /voice/call-completed`**

```json
{
  "business_id": "string",
  "call_data": {
    "call_id": "string",
    "start_time": "2026-01-01T00:00:00Z",
    "end_time": "2026-01-01T00:00:00Z",
    "duration_seconds": 0,
    "messages_count": 0,
    "full_transcript": "string",
    "messages": [],
    "audio_info": {
      "sample_rate": 24000,
      "bit_depth": 16,
      "customer_duration_sec": 0,
      "agent_duration_sec": 0
    }
  },
  "analysis": {
    "callId": "string",
    "durationSeconds": 0,
    "analyzedAt": "2026-01-01T00:00:00Z",
    "summary": "string",
    "summaryAr": "string",
    "overallSentiment": { "score": 0.8, "label": "Positive" },
    "mainTopics": [],
    "intentsDetected": [],
    "actionsPerformed": [],
    "escalationRequired": false,
    "escalationReason": null,
    "keyMoments": []
  },
  "queued_at": "2026-01-01T00:00:00Z",
  "audio_urls": {
    "customer": "string or null",
    "agent": "string or null",
    "stereo": "string or null"
  }
}
```

---

## 4. جلب التقرير (الفرونت)

**`GET /CallSummary/{interactionId}`**

### Response `200 OK`
```json
{
  "id": "string",
  "interactionId": "string",
  "businessId": "string",
  "callId": "string",
  "startTime": "2026-01-01T00:00:00Z",
  "endTime": "2026-01-01T00:00:00Z",
  "durationSeconds": 0,
  "messagesCount": 0,
  "fullTranscript": "string",
  "summary": "string",
  "summaryAr": "string",
  "sentimentScore": 0.8,
  "sentimentLabel": "Positive",
  "escalationRequired": false,
  "escalationReason": null,
  "analyzedAt": "2026-01-01T00:00:00Z",
  "createdAt": "2026-01-01T00:00:00Z",
  "audioUrls": {
    "customer": "https://... or null",
    "agent": "https://... or null",
    "stereo": "https://... or null"
  }
}
```

**`GET /CallSummary/business/{businessId}`**

نفس الشكل بس بيرجع array بكل المكالمات.

---

## 5. تحميل التسجيل

**`GET /CallRecording/{callId}/{fileName}`**

يرجع الملف الصوتي مباشرة.

---

## ملاحظات

- الـ `MeetingUrl` بيتحدد من الـ Settings بتاع الـ business — الفرونت مش محتاج يبعته.
- الـ `interactionId` احتفظ بيه — هو نفسه الـ `callId` في التقرير.
- مفيش polling — المكالمة بتخلص بشكل تلقائي من جهة الذكاء الاصطناعي.
- الـ `audio_urls` بتيجي من الذكاء الاصطناعي بعد ما يرفع الملفات على `/CallRecording/upload`.
