# 🤖 AI Team - دليل البدء السريع

## 📋 نظرة عامة

هذا المشروع هو نظام Digital Employee يدعم:
- محادثات نصية (Chat) مع AI
- مكالمات صوتية (Voice) مع AI
- تحليل المشاعر (Sentiment Analysis)
- اكتشاف النية (Intent Detection)

**Backend جاهز بالكامل** - نحتاج منك تنفيذ خدمات AI فقط.

---

## 🎯 المهام المطلوبة

### 1. **اكتشاف النية (Intent Detection)** ⚠️
- **الملف:** `Service layer/Services/IntentDetectionService.cs`
- **الوضع:** Placeholder موجود
- **المطلوب:** استبدال بـ AI Service

### 2. **تحليل المشاعر (Sentiment Analysis)** ⚠️
- **الملف:** `Service layer/Services/SentimentService.cs`
- **الوضع:** Placeholder موجود
- **المطلوب:** استبدال بـ AI Service

### 3. **Speech-to-Text** ⚠️
- **الملف:** `Service layer/Services/CustomerVoiceService.cs`
- **الدالة:** `ConvertAudioToTextAsync` (السطر 404)
- **الوضع:** Placeholder موجود
- **المطلوب:** استبدال بـ AI Service

### 4. **Text-to-Speech** ❌
- **الملف:** `Service layer/Services/CustomerVoiceService.cs`
- **الدالة:** `ConvertTextToAudioAsync` (غير موجودة)
- **الوضع:** غير موجودة
- **المطلوب:** إنشاء الدالة + AI Service

---

## 📚 ابدأ بقراءة هذه الملفات

### 1. **Agentic AI Flow** ⭐⭐⭐
**الملف:** `AGENTIC_AI_FLOW.md`
- **الـ AI يولد الرد** - Backend يمرره للـ Frontend
- المفهوم الأساسي للتعاون

### 2. **العقد الرسمي** ⭐⭐⭐
**الملف:** `AI_BACKEND_CONTRACT.md`
- **العقد الرسمي** بين Backend و AI Team
- يحدد بالضبط ما يرسله Backend وما يتوقعه
- جميع الـ Contracts محددة بوضوح (بما فيها Response Generation)
- أمثلة مفصلة

### 3. **الوثيقة الرئيسية** ⭐
**الملف:** `BACKEND_AI_TEAM_COLLABORATION.md`
- وثيقة شاملة بالعربية والإنجليزية
- توضح كل شيء بالتفصيل
- أمثلة على التكامل
- عقود API

### 4. **ملخص سريع** ⚡
**الملف:** `BACKEND_AI_TASKS_SUMMARY_AR.md`
- ملخص بالعربية فقط
- سريع وسهل القراءة

### 5. **دليل الاختبار** 🧪
**الملف:** `TEST_ENVIRONMENT_SETUP.md`
- كيفية إعداد بيئة الاختبار
- كيفية استخدام Postman
- كيفية الحصول على Tokens

---

## 🚀 خطوات البدء

### الخطوة 1: فهم المشروع
1. اقرأ `BACKEND_AI_TEAM_COLLABORATION.md`
2. اقرأ `BACKEND_AI_TASKS_SUMMARY_AR.md`
3. راجع الملفات البرمجية المذكورة

### الخطوة 2: إعداد بيئة الاختبار
1. استورد `AI_Endpoints_Postman_Collection.json` في Postman
2. استورد `Postman_Environment.json` في Postman
3. شغل المشروع: `dotnet run --project "digital employee"`
4. افتح Swagger: `https://localhost:44361/swagger` (IIS Express) ✅
   أو `https://localhost:7119/swagger` (dotnet run)
5. Login من Postman للحصول على Token

### الخطوة 3: فهم الواجهات (Interfaces)

#### أ) IIntentDetectionService
```csharp
Task<DetectedIntentResultDTO> DetectIntentAsync(
    string businessId,
    string interactionId,
    IEnumerable<string> recentMessages
);
```

**ما يرسله Backend:**
- `businessId`: معرف العمل
- `interactionId`: معرف المحادثة
- `recentMessages`: آخر 5-10 رسائل

**ما يتوقعه Backend:**
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

#### ب) ISentimentService
```csharp
Task<Sentiment> AnalyzeSentimentAsync(
    string messageId,
    string messageText,
    string language = "ar"
);
```

**ما يرسله Backend:**
- `messageId`: معرف الرسالة
- `messageText`: نص الرسالة
- `language`: اللغة ("ar" أو "en")

**ما يتوقعه Backend:**
```csharp
{
    SentimentId: "sentiment-123",
    MessageId: "msg-789",
    Score: 0.85,  // من -1.0 إلى 1.0
    Label: "Positive"  // أو "Negative" أو "Neutral"
}
```

#### ج) Speech-to-Text
```csharp
private async Task<string> ConvertAudioToTextAsync(
    string audioDataBase64,
    string? audioFormat
)
```

**ما يرسله Backend:**
- `audioDataBase64`: Base64-encoded audio
- `audioFormat`: "audio/wav", "audio/mp3", إلخ

**ما يتوقعه Backend:**
- نص منسوخ من الصوت

#### د) Text-to-Speech
```csharp
private async Task<(string audioBase64, string audioFormat)> ConvertTextToAudioAsync(
    string text,
    VoiceSettingsDTO settings,
    string? dialect
)
```

**ما يرسله Backend:**
- `text`: النص للتحويل
- `settings`: إعدادات الصوت (سرعة، نبرة، صوت)
- `dialect`: اللهجة ("Egyptian", "Standard Arabic", "English")

**ما يتوقعه Backend:**
- `audioBase64`: Base64-encoded audio
- `audioFormat`: "audio/wav", "audio/mp3", إلخ

---

## 🛠️ الخدمات المقترحة

### 1. **Azure Cognitive Services** (موصى به)
- **Intent Detection:** Azure Language Understanding (LUIS) / Azure Language Service
- **Sentiment Analysis:** Azure Text Analytics
- **Speech-to-Text:** Azure Speech Service
- **Text-to-Speech:** Azure Text-to-Speech

### 2. **Google Cloud AI**
- **Intent Detection:** Google Dialogflow
- **Sentiment Analysis:** Google Cloud Natural Language API
- **Speech-to-Text:** Google Cloud Speech-to-Text
- **Text-to-Speech:** Google Cloud Text-to-Speech

### 3. **AWS AI Services**
- **Intent Detection:** AWS Lex
- **Sentiment Analysis:** AWS Comprehend
- **Speech-to-Text:** AWS Transcribe
- **Text-to-Speech:** AWS Polly

### 4. **OpenAI**
- **Intent Detection:** OpenAI API (with prompt)
- **Sentiment Analysis:** OpenAI API (with prompt)
- **Speech-to-Text:** OpenAI Whisper API
- **Text-to-Speech:** OpenAI TTS API

---

## 📝 مثال على التكامل

### مثال: Intent Detection مع Azure

```csharp
public async Task<DetectedIntentResultDTO> DetectIntentAsync(
    string businessId,
    string interactionId,
    IEnumerable<string> recentMessages)
{
    // 1. إعداد Azure Language Service Client
    var client = new TextAnalyticsClient(
        new Uri("https://your-endpoint.cognitiveservices.azure.com/"),
        new AzureKeyCredential("your-api-key")
    );
    
    // 2. تجهيز البيانات
    var conversationContext = string.Join(" ", recentMessages);
    
    // 3. استدعاء AI Service
    var response = await client.AnalyzeConversationAsync(
        conversationContext,
        businessId,
        interactionId
    );
    
    // 4. تحويل النتيجة إلى DTO
    return new DetectedIntentResultDTO
    {
        Intent = response.Intent,
        Entities = response.Entities,
        Confidence = response.Confidence,
        RequiresAction = response.RequiresAction,
        DetectedLanguage = response.DetectedLanguage,
        DetectedDialect = response.DetectedDialect,
        ComplexityLevel = response.ComplexityLevel,
        RequiresEscalation = response.RequiresEscalation,
        PriorityLevel = response.PriorityLevel,
        EscalationReason = response.EscalationReason
    };
}
```

---

## 🧪 الاختبار قبل التكامل ⭐

### ⚠️ مهم جداً: اختبر الـ Endpoints قبل التكامل!

**لماذا؟**
- ✅ فهم كيف يعمل Backend
- ✅ فهم البيانات التي يرسلها Backend
- ✅ فهم البيانات التي يتوقعها Backend
- ✅ اختبار الـ endpoints مع Placeholder implementations
- ✅ التأكد من أن كل شيء يعمل قبل التكامل

### 📚 دليل الاختبار الكامل
**الملف:** `AI_TEAM_TESTING_GUIDE.md`
- شرح مفصل لجميع الـ Endpoints
- أمثلة على Requests والـ Responses
- سيناريوهات اختبار
- فهم البيانات قبل التكامل

### خطوات الاختبار السريع:

#### 1. **إعداد بيئة الاختبار**
```bash
# 1. شغل المشروع
dotnet run --project "digital employee"

# 2. استورد Postman Collection
# Import → AI_Endpoints_Postman_Collection.json
# Import → Postman_Environment.json

# 3. Login للحصول على Token
# Authentication → Login - Owner → Send
```

#### 2. **اختبار Customer Chat**
- **Get Capabilities** → تحقق من Chat/Voice
- **Send Chat Message (Arabic)** → `"عايز أطلب برجر"`
- احفظ `interactionId` من Response
- **Send Chat Message** → استخدم `interactionId` في الرسالة التالية

#### 3. **اختبار Sentiment Analysis**
- بعد إرسال رسالة، استخدم `messageId` من Response
- **Get Sentiment by Message ID** → تحقق من تحليل المشاعر

#### 4. **اختبار Customer Voice**
- **Initialize Voice Session** → أنشئ جلسة صوتية
- **Send Voice Message** → أرسل رسالة صوتية

### 📖 اقرأ دليل الاختبار الكامل
**الملف:** `AI_TEAM_TESTING_GUIDE.md` - يحتوي على:
- شرح مفصل لكل Endpoint
- أمثلة على Requests والـ Responses
- سيناريوهات اختبار كاملة
- فهم البيانات قبل التكامل

---

## 🧪 الاختبار (بعد التكامل)

### 1. **استخدام Postman**
- استورد `AI_Endpoints_Postman_Collection.json`
- استورد `Postman_Environment.json`
- Login للحصول على Token
- اختبر كل endpoint

### 2. **استخدام Swagger**
- افتح `https://localhost:44361/swagger` (IIS Express) ✅
- أو `https://localhost:7119/swagger` (dotnet run)
- جرب الـ endpoints مباشرة

### 3. **أمثلة Test Data**
- راجع `TEST_DATA_EXAMPLES.json`
- يحتوي على أمثلة لجميع الـ Requests

---

## 📦 الملفات المطلوبة

### وثائق:
- ✅ `BACKEND_AI_TEAM_COLLABORATION.md`
- ✅ `BACKEND_AI_TASKS_SUMMARY_AR.md`
- ✅ `TEST_ENVIRONMENT_SETUP.md`
- ✅ `QUICK_START_GUIDE.md`

### Testing:
- ✅ `AI_Endpoints_Postman_Collection.json`
- ✅ `Postman_Environment.json`
- ✅ `TEST_DATA_EXAMPLES.json`

### Code:
- ⚠️ `Service layer/Services/IntentDetectionService.cs` (يحتاج تعديل)
- ⚠️ `Service layer/Services/SentimentService.cs` (يحتاج تعديل)
- ⚠️ `Service layer/Services/CustomerVoiceService.cs` (يحتاج تعديل)

---

## ❓ أسئلة شائعة

### 1. **ما هي الخدمات المفضلة؟**
- يمكنك اختيار أي خدمة AI (Azure، Google، AWS، OpenAI)
- المهم هو الالتزام بالواجهات (Interfaces)

### 2. **كيف أحصل على API Keys؟**
- تواصل مع Backend Team
- سنوفر API Keys للخدمات المختارة

### 3. **ما هو Timeline؟**
- ناقش مع Backend Team
- نحدد موعد نهائي معاً

### 4. **هل يجب دعم لغات أخرى؟**
- حالياً: العربية والإنجليزية فقط
- إذا أردت إضافة لغات، ناقش مع Backend Team

---

## 📞 التواصل

### للأسئلة:
- راجع الوثائق أولاً
- إذا لم تجد الإجابة، تواصل مع Backend Team

### للإبلاغ عن المشاكل:
- وثق المشكلة
- أرسل Screenshots
- أرسل Error Messages

---

## ✅ Checklist

- [ ] قرأت `BACKEND_AI_TEAM_COLLABORATION.md`
- [ ] قرأت `BACKEND_AI_TASKS_SUMMARY_AR.md`
- [ ] أعددت بيئة الاختبار
- [ ] فهمت الواجهات (Interfaces)
- [ ] اخترت خدمات AI
- [ ] بدأت التنفيذ
- [ ] اختبرت التكامل
- [ ] أرسلت النتائج للـ Backend Team

---

## 🎯 الخطوات التالية

1. **اقرأ الوثائق** (30 دقيقة)
2. **أعد بيئة الاختبار** (15 دقيقة)
3. **اختر خدمات AI** (ناقش مع Backend Team)
4. **ابدأ التنفيذ** (حسب Timeline)
5. **اختبر التكامل** (مع Backend Team)
6. **أرسل النتائج** ✅

---

**جاهز للبدء؟ ابدأ بقراءة `BACKEND_AI_TEAM_COLLABORATION.md`! 🚀**

**آخر تحديث:** 2024-01-15

