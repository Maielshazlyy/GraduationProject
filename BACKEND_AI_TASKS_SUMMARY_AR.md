# ملخص مهام Backend و AI Team

## 🎯 نظرة سريعة

هذا ملف ملخص سريع يوضح **ماذا يجب على Backend عمله** و **ماذا يجب على AI Team عمله**.

---

## ✅ Backend - ما تم إنجازه (جاهز تماماً)

### 1. الواجهات (Interfaces) جاهزة:
- ✅ `IIntentDetectionService` - واجهة اكتشاف النية
- ✅ `ISentimentService` - واجهة تحليل المشاعر
- ✅ كل الـ DTOs جاهزة
- ✅ كل الـ Models في قاعدة البيانات جاهزة

### 2. نقاط التكامل جاهزة:
- ✅ Backend يستدعي `DetectIntentAsync` عند استقبال رسالة
- ✅ Backend يستدعي `AnalyzeSentimentAsync` بعد معالجة الرسالة
- ✅ Backend جاهز لاستقبال النتائج من AI

### 3. الملفات الجاهزة:
- ✅ `Service layer/Services/IntentDetectionService.cs` - فيه placeholder
- ✅ `Service layer/Services/SentimentService.cs` - فيه placeholder
- ✅ `Service layer/Services/CustomerVoiceService.cs` - فيه placeholder للـ Speech-to-Text

---

## ⚠️ Backend - ما يحتاج من AI Team

### 1. **اكتشاف النية (Intent Detection)**

**الملف:** `Service layer/Services/IntentDetectionService.cs`

**الوضع الحالي:**
- يوجد placeholder بسيط (keyword-based)
- يجب استبداله بـ AI حقيقي

**ما يرسله Backend للـ AI:**
```csharp
DetectIntentAsync(
    businessId: "business-123",
    interactionId: "interaction-456",
    recentMessages: ["مرحبا", "عايز اطلب برجر", "عايز برجر كبير"]
)
```

**ما يتوقعه Backend من AI:**
```csharp
{
    Intent: "CreateOrder",
    Entities: { "product": "برجر", "size": "كبير" },
    Confidence: 0.92,
    DetectedLanguage: "ar",
    DetectedDialect: "Egyptian",
    ComplexityLevel: "Low",
    RequiresEscalation: false,
    PriorityLevel: "Normal"
}
```

**ما يجب على AI Team عمله:**
- استبدال الـ placeholder بـ AI service حقيقي
- استخدام: Azure LUIS، Google Dialogflow، AWS Lex، أو OpenAI
- دعم العربية والإنجليزية
- اكتشاف اللهجة (مصري، فصحى)
- استخراج الكيانات (منتجات، كميات، أحجام)

---

### 2. **تحليل المشاعر (Sentiment Analysis)**

**الملف:** `Service layer/Services/SentimentService.cs`

**الوضع الحالي:**
- يوجد placeholder بسيط (keyword-based)
- يجب استبداله بـ AI حقيقي

**ما يرسله Backend للـ AI:**
```csharp
AnalyzeSentimentAsync(
    messageId: "msg-789",
    messageText: "الخدمة كانت ممتازة شكرا ليكم",
    language: "ar"
)
```

**ما يتوقعه Backend من AI:**
```csharp
{
    SentimentId: "sentiment-123",
    MessageId: "msg-789",
    Score: 0.85,  // من -1.0 إلى 1.0
    Label: "Positive"  // أو "Negative" أو "Neutral"
}
```

**ما يجب على AI Team عمله:**
- استبدال الـ placeholder بـ AI service حقيقي
- استخدام: Azure Text Analytics، Google Cloud Natural Language، AWS Comprehend
- دعم العربية والإنجليزية
- إرجاع score دقيق من -1.0 إلى 1.0

---

### 3. **تحويل الصوت إلى نص (Speech-to-Text)**

**الملف:** `Service layer/Services/CustomerVoiceService.cs`
**الدالة:** `ConvertAudioToTextAsync` (السطر 404)

**الوضع الحالي:**
- يوجد placeholder فقط
- يرجع نص ثابت: `"[Audio converted to text - integrate with speech-to-text service]"`

**ما يرسله Backend للـ AI:**
```csharp
ConvertAudioToTextAsync(
    audioDataBase64: "UklGRiQAAABXQVZFZm10...",  // Base64
    audioFormat: "audio/wav"
)
```

**ما يتوقعه Backend من AI:**
- نص منسوخ من الصوت
- بالعربية أو الإنجليزية حسب الصوت

**ما يجب على AI Team عمله:**
- استبدال الـ placeholder بـ AI service حقيقي
- استخدام: Azure Speech Service، Google Cloud Speech-to-Text، AWS Transcribe
- دعم صيغ متعددة: WAV، MP3، WebM
- اكتشاف اللغة تلقائياً

---

### 4. **تحويل النص إلى صوت (Text-to-Speech)**

**الملف:** `Service layer/Services/CustomerVoiceService.cs`
**الدالة:** `ConvertTextToAudioAsync` (غير موجودة - معلقة في السطر 248)

**الوضع الحالي:**
- الدالة غير موجودة
- الكود معلق في `CustomerVoiceService.cs`

**ما يرسله Backend للـ AI:**
```csharp
ConvertTextToAudioAsync(
    text: "مرحبا بك، كيف يمكنني مساعدتك؟",
    settings: {
        AgentVoice: "ar-EG-SalmaNeural",
        AgentVoiceSpeed: 1.0,
        AgentVoicePitch: 1.0,
        AgentVoiceLanguage: "ar"
    },
    dialect: "Egyptian"
)
```

**ما يتوقعه Backend من AI:**
```csharp
{
    audioBase64: "UklGRiQAAABXQVZFZm10...",  // Base64
    audioFormat: "audio/wav"
}
```

**ما يجب على AI Team عمله:**
- **إنشاء الدالة** `ConvertTextToAudioAsync`
- استخدام: Azure Text-to-Speech، Google Cloud TTS، AWS Polly
- تطبيق إعدادات الصوت (سرعة، نبرة، صوت)
- دعم اللهجات (مصري، فصحى، إنجليزي)

---

## 📋 ملخص المهام

| المهمة | الملف | الحالة | ما يجب عمله |
|--------|------|--------|-------------|
| **اكتشاف النية** | `IntentDetectionService.cs` | ⚠️ Placeholder | استبدال بـ AI service |
| **تحليل المشاعر** | `SentimentService.cs` | ⚠️ Placeholder | استبدال بـ AI service |
| **Speech-to-Text** | `CustomerVoiceService.cs` | ⚠️ Placeholder | استبدال بـ AI service |
| **Text-to-Speech** | `CustomerVoiceService.cs` | ❌ غير موجود | إنشاء الدالة |

---

## 🔄 تدفق البيانات

### للمحادثة النصية (Chat):
```
العميل → رسالة نصية
    ↓
Backend → AI (اكتشاف النية) → DetectedIntentResultDTO
    ↓
Backend → معالجة النية (إنشاء طلب، إلخ)
    ↓
Backend → AI (تحليل المشاعر) → Sentiment
    ↓
Backend → حفظ في قاعدة البيانات
    ↓
Backend → إرجاع رد نصي للعميل
```

### للمكالمات الصوتية (Voice):
```
العميل → رسالة صوتية (Base64)
    ↓
Backend → AI (Speech-to-Text) → نص
    ↓
Backend → AI (اكتشاف النية) → DetectedIntentResultDTO
    ↓
Backend → معالجة النية
    ↓
Backend → AI (تحليل المشاعر) → Sentiment
    ↓
Backend → AI (Text-to-Speech) → صوت (Base64)
    ↓
Backend → إرجاع رد صوتي للعميل
```

---

## 📝 ملاحظات مهمة

### للـ Backend:
1. ✅ كل شيء جاهز - فقط انتظار AI Team
2. ✅ الواجهات محددة بوضوح
3. ✅ الـ DTOs جاهزة
4. ✅ قاعدة البيانات جاهزة

### للـ AI Team:
1. ⚠️ يجب استبدال 3 placeholders
2. ⚠️ يجب إنشاء دالة Text-to-Speech
3. ⚠️ يجب دعم العربية والإنجليزية
4. ⚠️ يجب اختبار التكامل مع Backend

---

## 📚 ملفات مرجعية

- **الوثيقة الكاملة:** `BACKEND_AI_TEAM_COLLABORATION.md`
- **Postman Collection:** `AI_Endpoints_Postman_Collection.json`
- **Test Environment:** `TEST_ENVIRONMENT_SETUP.md`

---

**آخر تحديث:** 2024-01-15

