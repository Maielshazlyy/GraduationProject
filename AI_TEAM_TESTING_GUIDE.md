# 🧪 AI Team - دليل اختبار الـ Endpoints قبل التكامل

## 🎯 الهدف

هذا الدليل يوضح كيفية **اختبار جميع الـ endpoints قبل التكامل** مع خدمات AI.

**لماذا هذا مهم؟**
- ✅ فهم كيف يعمل Backend
- ✅ فهم البيانات التي يرسلها Backend
- ✅ فهم البيانات التي يتوقعها Backend
- ✅ اختبار الـ endpoints مع Placeholder implementations
- ✅ التأكد من أن كل شيء يعمل قبل التكامل

---

## 📋 المتطلبات

### 1. **تشغيل المشروع**
```bash
cd "D:\Users\Shazly\Desktop\assignments\grad-project"
dotnet run --project "digital employee"
```

### 2. **استيراد Postman**
1. افتح Postman
2. **Import** → `AI_Endpoints_Postman_Collection.json`
3. **Import** → `Postman_Environment.json`
4. اختر Environment: **"AI Endpoints - Test Environment"**

### 3. **Swagger (اختياري)**
افتح: `https://localhost:44361/swagger` (IIS Express) ✅
أو `https://localhost:7119/swagger` (dotnet run)

---

## 🔑 الخطوة 1: الحصول على Token

### الطريقة: استخدام Postman

1. **Authentication → Login - Owner**
2. اضغط **Send**
3. ✅ Token يُحفظ تلقائياً في `jwtToken`

**Request Body:**
```json
{
  "email": "owner@test.com",
  "password": "Owner123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-18T10:30:00Z"
}
```

**✅ Token يُحفظ تلقائياً في Environment Variable: `jwtToken`**

---

## 🧪 الخطوة 2: اختبار الـ Endpoints

### 1. **Customer Chat Endpoints** 💬

#### أ) Get Capabilities
**Endpoint:** `GET /api/CustomerChat/capabilities/{businessId}`

**الغرض:** معرفة ما إذا كان Chat/Voice مفعل

**Request:**
- لا يحتاج Body
- لا يحتاج Token (Public)

**Response:**
```json
{
  "businessId": "business-123",
  "businessName": "Test Restaurant",
  "chatEnabled": true,
  "voiceEnabled": true,
  "welcomeMessage": "مرحبا بك!",
  "voiceSettings": {
    "agentVoice": "ar-EG-SalmaNeural",
    "agentVoiceProvider": "Azure",
    "agentVoiceSpeed": 1.0,
    "agentVoicePitch": 1.0,
    "agentVoiceLanguage": "ar"
  }
}
```

**✅ اختبر هذا أولاً للتأكد من أن Backend يعمل**

---

#### ب) Send Chat Message (Arabic)
**Endpoint:** `POST /api/CustomerChat/message`

**الغرض:** إرسال رسالة نصية (Chat)

**Request Body:**
```json
{
  "businessId": "business-123",
  "customerId": "customer-456",
  "channel": "WebChat",
  "message": "عايز أطلب برجر"
}
```

**Response:**
```json
{
  "interactionId": "interaction-789",
  "replyText": "مرحبا! يمكنني مساعدتك في طلب برجر...",
  "orderId": null,
  "ticketId": null,
  "cart": {
    "totalPrice": 0,
    "items": []
  },
  "recommendations": [
    {
      "menuItemId": "item-123",
      "name": "بطاطس",
      "price": 15.00,
      "reason": "عادة ما يتم طلبها مع البرجر"
    }
  ],
  "isInterrupted": false
}
```

**ملاحظات مهمة:**
- ✅ `interactionId` يُستخدم في الرسائل التالية
- ✅ `replyText` هو رد AI (حالياً Placeholder)
- ✅ `recommendations` هي توصيات المنتجات

**✅ احفظ `interactionId` للاستخدام في الرسائل التالية**

---

#### ج) Send Chat Message (English)
**Endpoint:** `POST /api/CustomerChat/message`

**Request Body:**
```json
{
  "businessId": "business-123",
  "customerId": "customer-456",
  "interactionId": "interaction-789",
  "channel": "WebChat",
  "message": "I want to order a pizza"
}
```

**Response:**
```json
{
  "interactionId": "interaction-789",
  "replyText": "Hello! I can help you order a pizza...",
  "orderId": null,
  "ticketId": null,
  "cart": {
    "totalPrice": 0,
    "items": []
  },
  "recommendations": [],
  "isInterrupted": false
}
```

**✅ لاحظ أن `interactionId` نفسها (نفس المحادثة)**

---

#### د) Send Chat Message - Request Human Agent
**Endpoint:** `POST /api/CustomerChat/message`

**Request Body:**
```json
{
  "businessId": "business-123",
  "customerId": "customer-456",
  "interactionId": "interaction-789",
  "channel": "WebChat",
  "message": "عايز أتكلم مع موظف بشري"
}
```

**Response:**
```json
{
  "interactionId": "interaction-789",
  "replyText": "سأقوم بنقل محادثتك إلى موظف بشري...",
  "orderId": null,
  "ticketId": "ticket-123",
  "cart": null,
  "recommendations": [],
  "isInterrupted": false
}
```

**ملاحظات مهمة:**
- ✅ `ticketId` تم إنشاؤه (Escalation)
- ✅ المحادثة تم نقلها لموظف بشري

---

### 2. **Customer Voice Endpoints** 🎤

#### أ) Initialize Voice Session
**Endpoint:** `POST /api/CustomerVoice/initialize`

**الغرض:** بدء جلسة مكالمة صوتية

**Request Body:**
```json
{
  "businessId": "business-123",
  "customerId": "customer-456",
  "callSessionId": "call-123"
}
```

**Response:**
```json
{
  "interactionId": "interaction-456",
  "businessId": "business-123",
  "customerId": "customer-456",
  "channel": "Voice",
  "status": "Open",
  "startedAt": "2024-01-15T10:30:00Z",
  "callSessionId": "call-123"
}
```

**✅ احفظ `interactionId` و `callSessionId`**

---

#### ب) Send Voice Message
**Endpoint:** `POST /api/CustomerVoice/message`

**الغرض:** إرسال رسالة صوتية

**Request Body:**
```json
{
  "businessId": "business-123",
  "customerId": "customer-456",
  "interactionId": "interaction-456",
  "callSessionId": "call-123",
  "channel": "Voice",
  "audioData": "UklGRiQAAABXQVZFZm10...",
  "audioFormat": "audio/wav",
  "message": "عايز أطلب برجر"
}
```

**ملاحظات:**
- `audioData`: Base64-encoded audio (اختياري)
- `message`: نص الرسالة (إذا كان Audio غير متوفر)

**Response:**
```json
{
  "interactionId": "interaction-456",
  "replyText": "مرحبا! يمكنني مساعدتك...",
  "replyAudio": null,
  "replyAudioFormat": null,
  "orderId": null,
  "ticketId": null,
  "cart": null,
  "recommendations": [],
  "hasDeliveryDelay": false,
  "alternativeTimeSlots": null,
  "isInterrupted": false
}
```

**ملاحظات مهمة:**
- ✅ `replyAudio` حالياً `null` (لأن Text-to-Speech غير موجود)
- ✅ `replyText` موجود (يمكن استخدامه للتحقق)

---

### 3. **Sentiment Analysis Endpoints** 😊

#### أ) Get Sentiment by Message ID
**Endpoint:** `GET /api/Sentiment/message/{messageId}`

**الغرض:** الحصول على تحليل المشاعر لرسالة معينة

**Request:**
- لا يحتاج Body
- يحتاج Token (OwnerOrAdmin)

**Response:**
```json
[
  {
    "sentimentId": "sentiment-123",
    "messageId": "msg-789",
    "sourceText": "الخدمة كانت ممتازة شكرا ليكم",
    "score": 0.85,
    "label": "Positive",
    "analyzedAt": "2024-01-15T10:30:00Z"
  }
]
```

**ملاحظات مهمة:**
- ✅ `score`: من -1.0 (سلبي جداً) إلى 1.0 (إيجابي جداً)
- ✅ `label`: "Positive", "Negative", أو "Neutral"
- ✅ حالياً Placeholder (keyword-based)

**✅ هذا هو ما يجب أن يعيده AI Service**

---

#### ب) Get Sentiment by Business ID
**Endpoint:** `GET /api/Sentiment/business/{businessId}`

**الغرض:** الحصول على جميع تحليلات المشاعر لعميل معين

**Response:**
```json
[
  {
    "sentimentId": "sentiment-123",
    "messageId": "msg-789",
    "sourceText": "الخدمة كانت ممتازة",
    "score": 0.85,
    "label": "Positive",
    "analyzedAt": "2024-01-15T10:30:00Z"
  },
  {
    "sentimentId": "sentiment-124",
    "messageId": "msg-790",
    "sourceText": "مش عاجبني الطلب",
    "score": -0.75,
    "label": "Negative",
    "analyzedAt": "2024-01-15T10:35:00Z"
  }
]
```

---

## 🔍 فهم البيانات

### 1. **ما يرسله Backend للـ AI (Internally)**

عندما يرسل العميل رسالة، Backend يستدعي:

#### أ) Intent Detection
```csharp
// Backend يستدعي هذا داخلياً
DetectIntentAsync(
    businessId: "business-123",
    interactionId: "interaction-789",
    recentMessages: [
        "مرحبا",
        "عايز أطلب برجر",
        "عايز برجر كبير"
    ]
)
```

**ما يجب أن يعيده AI Service:**
```csharp
{
    Intent: "CreateOrder",
    Entities: {
        "product": "برجر",
        "size": "كبير"
    },
    Confidence: 0.92,
    DetectedLanguage: "ar",
    DetectedDialect: "Egyptian",
    ComplexityLevel: "Low",
    RequiresEscalation: false,
    PriorityLevel: "Normal"
}
```

---

#### ب) Sentiment Analysis
```csharp
// Backend يستدعي هذا داخلياً
AnalyzeSentimentAsync(
    messageId: "msg-789",
    messageText: "الخدمة كانت ممتازة شكرا ليكم",
    language: "ar"
)
```

**ما يجب أن يعيده AI Service:**
```csharp
{
    SentimentId: "sentiment-123",
    MessageId: "msg-789",
    Score: 0.85,
    Label: "Positive"
}
```

---

### 2. **ما يرسله Backend للعميل (API Response)**

```json
{
  "interactionId": "interaction-789",
  "replyText": "مرحبا! يمكنني مساعدتك...",
  "orderId": null,
  "ticketId": null,
  "cart": {...},
  "recommendations": [...]
}
```

**ملاحظات:**
- ✅ `replyText` هو رد AI (يتم إنشاؤه بناءً على Intent)
- ✅ `recommendations` هي توصيات المنتجات
- ✅ `orderId` و `ticketId` يتم إنشاؤهما عند الحاجة

---

## 📊 سيناريوهات الاختبار

### سيناريو 1: محادثة كاملة بالعربية

1. **Get Capabilities** → تحقق من Chat/Voice
2. **Send Chat Message (Arabic)** → `"عايز أطلب برجر"`
3. احفظ `interactionId` من Response
4. **Send Chat Message** → `"عايز برجر كبير مع بطاطس"` (استخدم `interactionId`)
5. **Get Sentiment by Message ID** → تحقق من تحليل المشاعر

**النتيجة المتوقعة:**
- ✅ Intent: "CreateOrder"
- ✅ Entities: { "product": "برجر", "size": "كبير", "side": "بطاطس" }
- ✅ Sentiment: Positive (إذا كانت الرسالة إيجابية)

---

### سيناريو 2: محادثة بالإنجليزية

1. **Send Chat Message (English)** → `"I want to order a pizza"`
2. تحقق من `DetectedLanguage: "en"` في Response
3. **Send Chat Message** → `"I want to talk to a human agent"`
4. تحقق من `ticketId` في Response (Escalation)

**النتيجة المتوقعة:**
- ✅ Intent: "RequestHumanAgent"
- ✅ `ticketId` موجود
- ✅ `RequiresEscalation: true`

---

### سيناريو 3: مكالمة صوتية

1. **Initialize Voice Session** → أنشئ جلسة صوتية
2. احفظ `interactionId` و `callSessionId`
3. **Send Voice Message** → أرسل رسالة صوتية
4. تحقق من `replyText` في Response
5. **Submit Voice Feedback** → أرسل تقييم

**النتيجة المتوقعة:**
- ✅ `interactionId` موجود
- ✅ `replyText` موجود (حالياً، `replyAudio` null)
- ✅ بعد التكامل: `replyAudio` سيكون موجود

---

## 🎯 ما يجب أن تفهمه قبل التكامل

### 1. **الـ Endpoints المتاحة**
- ✅ Customer Chat: `POST /api/CustomerChat/message`
- ✅ Customer Voice: `POST /api/CustomerVoice/message`
- ✅ Sentiment: `GET /api/Sentiment/message/{messageId}`

### 2. **البيانات التي يرسلها Backend**
- ✅ `businessId`, `interactionId`, `recentMessages` (للاكتشاف النية)
- ✅ `messageId`, `messageText`, `language` (لتحليل المشاعر)
- ✅ `audioDataBase64`, `audioFormat` (للـ Speech-to-Text)

### 3. **البيانات التي يتوقعها Backend**
- ✅ `DetectedIntentResultDTO` (للاكتشاف النية)
- ✅ `Sentiment` (لتحليل المشاعر)
- ✅ `string` (للـ Speech-to-Text)
- ✅ `(string audioBase64, string audioFormat)` (للـ Text-to-Speech)

### 4. **Placeholder Implementations**
- ✅ حالياً: Keyword-based
- ✅ بعد التكامل: AI-based

---

## ✅ Checklist قبل التكامل

- [ ] فهمت جميع الـ Endpoints
- [ ] اختبرت Customer Chat
- [ ] اختبرت Customer Voice
- [ ] اختبرت Sentiment Analysis
- [ ] فهمت البيانات التي يرسلها Backend
- [ ] فهمت البيانات التي يتوقعها Backend
- [ ] جاهز للبدء في التكامل

---

## 📚 الملفات المرجعية

- **Postman Collection:** `AI_Endpoints_Postman_Collection.json`
- **Postman Environment:** `Postman_Environment.json`
- **Test Data Examples:** `TEST_DATA_EXAMPLES.json`
- **Setup Guide:** `TEST_ENVIRONMENT_SETUP.md`
- **Quick Start:** `QUICK_START_GUIDE.md`

---

## 🎯 الخطوات التالية

1. ✅ اختبر جميع الـ Endpoints
2. ✅ فهم البيانات
3. ✅ ابدأ في التكامل مع AI Services
4. ✅ اختبر التكامل
5. ✅ أرسل النتائج للـ Backend Team

---

**جاهز للاختبار؟ ابدأ الآن! 🚀**

**آخر تحديث:** 2024-01-15

