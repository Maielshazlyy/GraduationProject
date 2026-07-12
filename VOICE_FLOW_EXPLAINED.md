# 🎤 شرح تفصيلي: Voice Flow مع AI

## 📋 نظرة عامة

هذا المستند يشرح **بشكل تفصيلي** كيف يعمل Voice Flow في النظام، وكيف يتفاعل Backend مع AI في كل خطوة.

**الهدف:** فهم كامل للـ Voice Flow حتى يمكن شرحه للـ AI Team.

---

## 🤖 Agentic AI - المفهوم الأساسي

1. **AI** يديك **Intent** → Backend ينفذ **Business Logic**
2. **Backend** يخبر AI إنه نفذ (ActionOutcome + ActionData)
3. **AI** يولد الرد → Backend يبعته للـ Frontend

📄 **راجع:** `AGENTIC_AI_FLOW.md` للتفاصيل الكاملة.

---

## ⚠️ ملاحظة مهمة: WebSocket للـ Voice

**الـ Voice سيتم عبر WebSocket (SignalR) وليس REST API.**

- الصوت يُرسل مباشرة من الجهاز (streaming) عبر WebSocket
- لا يتم حفظ الملفات الصوتية - الصوت يُعالج في الوقت الفعلي (real-time)
- الـ REST API الحالي (`/api/CustomerVoice/message`) هو placeholder للاختبار فقط

---

## 🔄 تدفق المكالمة الصوتية (Voice Flow) - الخطوات الكاملة

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        العميل يتصل (Voice Call)                                  │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 0: قبل المكالمة - Initialize Voice Session                              │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Frontend/Telephony يرسل: BusinessId, CustomerId, CallSessionId                 │
│  • Backend ينشئ Interaction جديدة (Channel = "Voice")                           │
│  • لا يوجد تفاعل مع AI هنا                                                       │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  العميل يتكلم (يرسل رسالة صوتية عبر WebSocket - Streaming)                       │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 1: AI Speech-to-Text (تحويل الصوت إلى نص)                               │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يستقبل: Audio chunks مباشرة من WebSocket (streaming)                  │
│  • Backend يستدعي: ConvertAudioToTextAsync(audioChunk)                          │
│  • AI Team يجب أن ينفذ: تحويل الصوت إلى نص (streaming STT)                       │
│  • Output: نص منسوخ (مثل: "عايز أطلب برجر كبير")                                │
│  • ⚠️ الصوت لا يُحفظ - يُعالج مباشرة (real-time)                                │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 2: Backend يحفظ الرسالة                                                  │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يحفظ الرسالة في قاعدة البيانات (Message)                              │
│  • SenderType = "Customer"                                                       │
│  • Content = النص (من Speech-to-Text)                                             │
│  • لا يوجد تفاعل مع AI هنا                                                       │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 3: AI Intent Detection (اكتشاف النية)                                    │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يجمع آخر 10 رسائل من المحادثة                                          │
│  • Backend يستدعي: DetectIntentAsync(businessId, interactionId, recentMessages) │
│  • AI Team يجب أن ينفذ: تحليل النص واستخراج النية والكيانات                       │
│  • Output: DetectedIntentResultDTO (Intent, Entities, Confidence, Language, etc.) │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 4: AI Sentiment Analysis (تحليل المشاعر)                                  │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يستدعي: AnalyzeSentimentAsync(messageId, messageText, language)       │
│  • AI Team يجب أن ينفذ: تحليل مشاعر النص                                          │
│  • Output: Sentiment (Score, Label: Positive/Negative/Neutral)                   │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 5: Backend ينفذ Business Logic (بدون AI)                                 │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يقرر: بناءً على Intent (CreateOrder, Complaint, etc.)                │
│  • Backend ينفذ: إنشاء طلب، إنشاء تذكرة، إلخ                                     │
│  • Backend يجمع: ActionOutcome + ActionData (orderId, cart, etc.)                │
│  • لا يوجد تفاعل مع AI هنا - Backend فقط                                          │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 5.5: AI Response Generation (توليد الرد) ⭐ Agentic AI                     │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يرسل: السياق الكامل (intent, actionData, recentMessages)              │
│  • AI Agent يولد: replyText (الرد الطبيعي المحادثي)                              │
│  • ⭐ الـ AI هو اللي بيقول الرد - مش Backend                                     │
│  • Backend يستلم الرد ويبعته للـ Frontend مباشرة                                 │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 6: AI Text-to-Speech (تحويل النص إلى صوت)                                │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Backend يستدعي: ConvertTextToAudioAsync(replyText, settings, dialect)         │
│  • AI Team يجب أن ينفذ: تحويل النص إلى صوت                                       │
│  • Input: replyText, VoiceSettings (صوت، سرعة، نبرة، لغة)، dialect               │
│  • Output: Audio chunks (streaming) - يُرسل مباشرة عبر WebSocket                │
│  • ⚠️ الصوت لا يُحفظ - يُرسل مباشرة للعميل (streaming)                           │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  الخطوة 7: Backend يرجع الرد للعميل عبر WebSocket                                │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  • Response: Audio chunks (streaming) تُرسل مباشرة للعميل عبر WebSocket          │
│  • Frontend/Telephony يشتغل الصوت للعميل مباشرة (real-time)                      │
│  • ⚠️ لا يتم حفظ الملفات الصوتية - streaming only                                │
└─────────────────────────────────────────────────────────────────────────────────┘
```

---

## 📝 شرح تفصيلي لكل خطوة مع AI

### 🔴 الخطوة 1: Speech-to-Text (تحويل الصوت إلى نص)

**متى يحدث؟**
- عندما يرسل العميل رسالة صوتية عبر WebSocket (streaming)
- إذا أرسل العميل نصاً مباشرة (Message)، يتم تخطي هذه الخطوة

**⚠️ ملاحظة:** في التطبيق الفعلي، الصوت يُرسل مباشرة من الجهاز عبر WebSocket (streaming)، وليس Base64 في JSON.

**ما يرسله Backend للـ AI (في WebSocket):**
```csharp
ConvertAudioToTextAsync(
    audioChunk: byte[],  // Audio chunk مباشرة من WebSocket (streaming)
    audioFormat: "audio/wav"  // أو "audio/mp3", "audio/webm"
)
// أو في REST API placeholder:
ConvertAudioToTextAsync(
    audioDataBase64: "UklGRiQAAABXQVZFZm10...",  // Base64-encoded audio (placeholder only)
    audioFormat: "audio/wav"
)
```

**ما يتوقعه Backend من AI:**
- نص منسوخ من الصوت
- بالعربية أو الإنجليزية حسب الصوت
- Plain text بدون تنسيق خاص

**مثال:**
- Input: Base64 audio لصوت يقول "عايز أطلب برجر كبير"
- Output: `"عايز أطلب برجر كبير"`

**ملاحظات مهمة:**
- AI يجب أن يكتشف اللغة تلقائياً (عربي أو إنجليزي)
- AI يجب أن يدعم صيغ مختلفة (WAV, MP3, WebM)
- إذا فشل AI، Backend سيفشل في معالجة الرسالة
- حالياً: Placeholder موجود - يرجع نص ثابت

---

### 🔴 الخطوة 3: Intent Detection (اكتشاف النية)

**متى يحدث؟**
- بعد تحويل الصوت إلى نص (أو بعد استلام النص مباشرة)
- بعد حفظ الرسالة في قاعدة البيانات

**ما يرسله Backend للـ AI:**
```csharp
DetectIntentAsync(
    businessId: "business-123",
    interactionId: "interaction-456",
    recentMessages: [
        "Customer: مرحبا",
        "AI: مرحبا! كيف يمكنني مساعدتك؟",
        "Customer: عايز أطلب برجر",
        "AI: ممتاز! أي حجم برجر؟",
        "Customer: عايز برجر كبير مع بطاطس"   // ← آخر رسالة
    ]
)
```

**ما يتوقعه Backend من AI:**
```json
{
    "intent": "CreateOrder",
    "entities": {
        "product": "برجر",
        "size": "كبير",
        "side": "بطاطس"
    },
    "confidence": 0.92,
    "requiresAction": true,
    "detectedLanguage": "ar",
    "detectedDialect": "Egyptian",
    "complexityLevel": "Low",
    "requiresEscalation": false,
    "priorityLevel": "Normal",
    "escalationReason": null
}
```

**لماذا recentMessages؟**
- لأن المحادثة قد تكون متعددة الجولات
- العميل قد يقول: "عايز برجر" ثم "كبير" ثم "مع بطاطس"
- AI يحتاج السياق لفهم النية الكاملة

**الـ Intents المدعومة:**
- `CreateOrder` - إنشاء طلب
- `ModifyOrder` - تعديل طلب
- `CancelOrder` - إلغاء طلب
- `AskAboutOrderStatus` - السؤال عن حالة الطلب
- `Complaint` - شكوى
- `RequestHumanAgent` - طلب موظف بشري
- `AskAboutProducts` - السؤال عن المنتجات
- `GeneralQuestion` - سؤال عام

**ملاحظات مهمة:**
- `DetectedLanguage` و `DetectedDialect` يُستخدمان لاحقاً في Text-to-Speech
- `Entities` تُستخدم في Business Logic (مثل: إنشاء الطلب)
- `RequiresEscalation` و `ComplexityLevel` تُستخدمان لقرار التحويل لموظف بشري

---

### 🔴 الخطوة 4: Sentiment Analysis (تحليل المشاعر)

**متى يحدث؟**
- بعد Intent Detection
- بعد تحديث الرسالة بالـ Intent

**ما يرسله Backend للـ AI:**
```csharp
AnalyzeSentimentAsync(
    messageId: "msg-789",
    messageText: "الخدمة كانت ممتازة شكرا ليكم",
    language: "ar"  // من intentResult.DetectedLanguage
)
```

**ما يتوقعه Backend من AI:**
```json
{
    "sentimentId": "sentiment-123",
    "messageId": "msg-789",
    "sourceText": "الخدمة كانت ممتازة شكرا ليكم",
    "analyzedAt": "2024-01-15T10:30:00Z",
    "score": 0.85,
    "label": "Positive"
}
```

**لماذا Sentiment؟**
- لتتبع رضا العملاء
- للتقارير والتحليلات
- للتنبيه في حالة مشاعر سلبية متكررة

**ملاحظات مهمة:**
- يتم تنفيذها في الخلفية (لا تؤثر على الرد)
- إذا فشلت، Backend يستمر في المعالجة (لا يتوقف)

---

### 🔴 الخطوة 6: Text-to-Speech (تحويل النص إلى صوت)

**متى يحدث؟**
- بعد تنفيذ Business Logic و AI Response Generation
- ⚠️ في WebSocket: Audio chunks تُرسل مباشرة للعميل (streaming)، وليس Base64 في JSON
- بعد إنشاء replyText

**ما يرسله Backend للـ AI:**
```csharp
ConvertTextToAudioAsync(
    text: "تم إنشاء طلبك بنجاح! هل تريد إضافة أي شيء؟",
    settings: new VoiceSettingsDTO {
        AgentVoice = "ar-EG-SalmaNeural",
        AgentVoiceProvider = "Azure",
        AgentVoiceSpeed = 1.0,
        AgentVoicePitch = 1.0,
        AgentVoiceLanguage = "ar"
    },
    dialect: "Egyptian"  // من intentResult.DetectedDialect
)
```

**ملاحظة:** الدالة `ConvertTextToAudioAsync` **غير موجودة حالياً** - الكود معلق في `CustomerVoiceService.cs` السطر 248.

**ما يتوقعه Backend من AI:**
- **في WebSocket (التطبيق الفعلي):** Audio chunks (byte[]) تُرسل مباشرة عبر WebSocket (streaming)
- **في REST API placeholder:** Base64-encoded audio (للاستخدام في الاختبار فقط)

```csharp
// WebSocket (streaming):
audioChunk: byte[]  // Audio chunk مباشرة من TTS service

// REST API placeholder:
audioBase64: "UklGRiQAAABXQVZFZm10...",
audioFormat: "audio/wav"
```

**من أين تأتي VoiceSettings؟**
- من قاعدة البيانات (جدول Settings)
- كل Business له إعدادات خاصة (صوت، سرعة، نبرة، لغة)
- Business Owner يضبطها من لوحة التحكم

**لماذا Dialect？**
- لاختيار الصوت المناسب (مصري، فصحى، إنجليزي)
- صوت مصري مختلف عن صوت فصحى

**ملاحظات مهمة:**
- ⚠️ **في WebSocket:** Audio chunks تُرسل مباشرة للعميل (streaming) - لا يتم حفظ الملفات
- ⚠️ **في REST API placeholder:** Base64 في JSON (للاستخدام في الاختبار فقط)
- حالياً: ReplyAudio = null (لأن TTS غير منفذ)
- بعد التكامل: العميل سيسمع الصوت مباشرة عبر WebSocket

---

## 📊 جدول ملخص: AI Interactions في Voice Flow

| الخطوة | AI Service | Input | Output | الحالة |
|--------|------------|-------|--------|--------|
| 1 | **Speech-to-Text** | `ConvertAudioToTextAsync(audioBase64, format)` | `string` (نص) | ⚠️ Placeholder |
| 3 | **Intent Detection** | `DetectIntentAsync(businessId, interactionId, recentMessages)` | `DetectedIntentResultDTO` | ⚠️ Placeholder |
| 4 | **Sentiment Analysis** | `AnalyzeSentimentAsync(messageId, messageText, language)` | `Sentiment` | ⚠️ Placeholder |
| **5.5** | **Response Generation** ⭐ | `GenerateResponseAsync(context)` | `string` (الرد) | ❌ غير موجود |
| 6 | **Text-to-Speech** | `ConvertTextToAudioAsync(text, settings, dialect)` | `(audioBase64, format)` | ❌ غير موجود |

**ملاحظة:** Response Generation هو العقد الأهم في Agentic AI - **الـ AI يولد الرد**، Backend يمرره للـ Frontend.

---

## 🔄 تدفق البيانات بالتفصيل

### Request من Frontend/Telephony:

```json
POST /api/CustomerVoice/message

{
    "businessId": "business-123",
    "customerId": "customer-456",
    "interactionId": "interaction-789",
    "callSessionId": "call-123",
    "channel": "Voice",
    "audioData": "UklGRiQAAABXQVZFZm10...",
    "audioFormat": "audio/wav",
    "message": null
}
```

**ملاحظة:** يمكن إرسال `message` (نص) بدلاً من `audioData` للاختبار - في هذه الحالة يتم تخطي Speech-to-Text.

### Response من Backend:

```json
{
    "interactionId": "interaction-789",
    "replyText": "تم إنشاء طلبك بنجاح! هل تريد إضافة بطاطس أو مشروب؟",
    "replyAudio": null,
    "replyAudioFormat": null,
    "orderId": "order-123",
    "ticketId": null,
    "cart": {
        "totalPrice": 50.00,
        "items": [
            {
                "menuItemId": "item-1",
                "name": "برجر كبير",
                "quantity": 1,
                "unitPrice": 45.00
            }
        ]
    },
    "recommendations": [
        {
            "menuItemId": "item-2",
            "name": "بطاطس",
            "price": 15.00,
            "reason": "عادة ما يتم طلبها مع البرجر"
        }
    ],
    "hasDeliveryDelay": false,
    "alternativeTimeSlots": null,
    "isInterrupted": false
}
```

**بعد التكامل:** `replyAudio` و `replyAudioFormat` سيكونان لهما قيمة.

---

## 🎯 نقاط مهمة للـ AI Team

### 1. **ترتيب الاستدعاءات:**
```
Speech-to-Text → Intent Detection → Sentiment Analysis → [Business Logic] → Text-to-Speech
```

### 2. **الاعتماديات:**
- **Intent Detection** يعتمد على النص (من Speech-to-Text أو Message)
- **Sentiment Analysis** يعتمد على `DetectedLanguage` من Intent Detection
- **Text-to-Speech** يعتمد على `replyText` و `DetectedDialect` و `VoiceSettings`

### 3. **البيانات المشتركة:**
- `IntentResult.DetectedLanguage` → يُستخدم في Sentiment و TTS
- `IntentResult.DetectedDialect` → يُستخدم في TTS
- `VoiceSettings` → من قاعدة البيانات (كل Business له إعداداته)

### 4. **حالات خاصة:**
- **إذا كان AudioData فارغاً و Message موجود:** يتم تخطي Speech-to-Text
- **إذا فشل Sentiment:** Backend يستمر (لا يتوقف)
- **إذا فشل Speech-to-Text:** Backend سيفشل (لا يمكن المتابعة بدون نص)
- **إذا فشل Intent Detection:** Backend يستخدم Default (GeneralQuestion)
- **إذا فشل Text-to-Speech:** Backend يرجع ReplyText فقط (ReplyAudio = null)

---

## 📋 ملخص للشرح

عند شرح Voice Flow للـ AI Team:
1. **4 خدمات AI** مطلوبة في Voice Flow
2. **الترتيب:** Speech-to-Text → Intent → Sentiment → Text-to-Speech
3. **Speech-to-Text** و **Text-to-Speech** خاصان بالـ Voice (لا يوجدان في Chat)
4. **Intent** و **Sentiment** مشتركان بين Chat و Voice
5. **VoiceSettings** تأتي من إعدادات كل Business
6. **Dialect** و **Language** من Intent Detection يُستخدمان في TTS

---

**آخر تحديث:** 2024-01-15

