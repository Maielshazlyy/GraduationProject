# 📋 AI-Backend Contract / عقد AI-Backend

## 🎯 نظرة عامة

هذا العقد يحدد **التزامات** و **متطلبات** التكامل بين Backend و AI Team.

**الهدف:** ضمان أن AI Team يفهم بالضبط ما يرسله Backend وما يتوقعه Backend.

---

## 🤖 Agentic AI - المفهوم الأساسي

**التقسيم:**
1. **AI** يديك **Intent** → Backend ينفذ **Business Logic** بناءً عليه
2. **Backend** يخبر AI إنه نفذ (ActionOutcome + ActionData)
3. **AI** يولد الرد بناءً على النتيجة
4. **Backend** يبعته للـ Frontend (بدون تعديل)

📄 **راجع:** `AGENTIC_AI_FLOW.md` للتفاصيل الكاملة.

---

## ⚠️ ملاحظة مهمة: WebSocket للـ Voice

**الـ Voice سيتم عبر WebSocket (SignalR) وليس REST API.**

- الصوت يُرسل مباشرة من الجهاز (streaming) عبر WebSocket
- لا يتم حفظ الملفات الصوتية - الصوت يُعالج في الوقت الفعلي (real-time)
- الـ REST API الحالي (`/api/CustomerVoice/message`) هو placeholder للاختبار فقط
- Speech-to-Text و Text-to-Speech يعملان مع audio chunks مباشرة (streaming)

---

## 📝 Contract 0: Response Generation / توليد الرد ⭐ (الأهم)

**الترتيب:** Backend ينفذ Business Logic أولاً → ثم يرسل النتيجة للـ AI → AI يولد الرد.

### Interface
```csharp
Task<string> GenerateResponseAsync(ResponseGenerationContextDTO context);
```

### ✅ Input (ما يرسله Backend)

Backend يرسل **النتيجة** بعد تنفيذ Business Logic:
- `ActionOutcome`: ماذا نفذ (OrderCreated, TicketCreated, etc.)
- `ActionData`: البيانات (orderId, cart, recommendations, etc.)

```csharp
public class ResponseGenerationContextDTO
{
    public string BusinessId { get; set; }
    public string InteractionId { get; set; }
    public string Intent { get; set; }
    public string? DetectedLanguage { get; set; }
    public string? DetectedDialect { get; set; }
    public List<string> RecentMessages { get; set; }
    public string ActionOutcome { get; set; }      // "OrderCreated", "TicketCreated", etc.
    public Dictionary<string, object> ActionData { get; set; }  // orderId, cart, etc.
    public string Channel { get; set; }           // "WebChat" or "Voice"
}
```

### ✅ Output (ما يتوقعه Backend)

**Return Type:** `string` - **الرد النصي الكامل**

- هذا هو الرد اللي Backend **يبعته للـ Frontend مباشرة**
- Backend **لا يعدله**
- الـ AI يولد رد طبيعي، محادثي، مناسب للسياق واللغة

**Example Output:**
```
"تمام! سجلت لك برجر كبير. عايز تضيف بطاطس أو مشروب؟"
```

### ⚠️ Requirements

1. الرد يجب أن يكون **باللغة المناسبة** (عربي أو إنجليزي حسب DetectedLanguage)
2. الرد يجب أن **يتضمن المعلومات المهمة** من ActionData (رقم الطلب، التذكرة، إلخ)
3. الرد يجب أن يكون **طبيعي ومحادثي** (Generative AI)
4. الرد يجب أن يكون **مناسب للقناة** (Chat أو Voice)

### 📦 Recommendations / التوصيات

**Backend** يحسب التوصيات (مثلاً: طلب برجر → يقترح بطاطس، مشروب) ويبعتها في `ActionData["recommendations"]`.

**AI** يستقبل التوصيات ويضيفها في الرد بشكل طبيعي للعميل (Chat أو Voice).

```csharp
// ActionData عند OrderCreated:
{
    "orderId": "ord-123",
    "totalPrice": 50,
    "recommendations": [
        { "menuItemId": "...", "name": "بطاطس", "price": 15, "reason": "مناسبة مع البرجر" },
        { "menuItemId": "...", "name": "بيبسي", "price": 10, "reason": "مشروب مناسب" }
    ]
}
```

**مثال على رد AI:** "تمام! سجلت لك برجر كبير. عايز تضيف بطاطس أو بيبسي؟"

---

## 📝 Contract 1: Intent Detection / اكتشاف النية

### Interface
```csharp
Task<DetectedIntentResultDTO> DetectIntentAsync(
    string businessId,
    string interactionId,
    IEnumerable<string> recentMessages
);
```

### ✅ Input (ما يرسله Backend)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `businessId` | `string` | ✅ Yes | معرف العمل (Business ID) |
| `interactionId` | `string` | ✅ Yes | معرف المحادثة (Conversation Session ID) |
| `recentMessages` | `IEnumerable<string>` | ✅ Yes | آخر 5-10 رسائل في المحادثة (من الأقدم للأحدث) |

**Example Input:**
```csharp
DetectIntentAsync(
    businessId: "business-123",
    interactionId: "interaction-456",
    recentMessages: new List<string> {
        "مرحبا",
        "عايز أطلب برجر",
        "عايز برجر كبير مع بطاطس"
    }
)
```

### ✅ Output (ما يتوقعه Backend)

**Return Type:** `DetectedIntentResultDTO`

```csharp
public class DetectedIntentResultDTO
{
    // Required Fields
    public string Intent { get; set; }                    // Required
    public Dictionary<string, string> Entities { get; set; }  // Required
    public double Confidence { get; set; }                // Required (0.0 to 1.0)
    public bool RequiresAction { get; set; }              // Required
    
    // Optional Fields
    public string? DetectedLanguage { get; set; }         // "ar" or "en"
    public string? DetectedDialect { get; set; }          // "Egyptian", "Standard Arabic", "English"
    public string? ComplexityLevel { get; set; }          // "Low", "Medium", "High"
    public bool RequiresEscalation { get; set; }          // Default: false
    public string? PriorityLevel { get; set; }            // "Low", "Normal", "High", "Critical"
    public string? EscalationReason { get; set; }         // Human-readable reason
}
```

**Example Output:**
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

### 📋 Supported Intents

| Intent | Description | RequiresAction | Example |
|--------|-------------|----------------|---------|
| `CreateOrder` | العميل يريد إنشاء طلب | ✅ true | "عايز أطلب برجر" |
| `AskAboutOrderStatus` | العميل يسأل عن حالة الطلب | ❌ false | "وين الطلب؟" |
| `Complaint` | العميل لديه شكوى | ✅ true | "مش عاجبني الطلب" |
| `RequestHumanAgent` | العميل يطلب موظف بشري | ✅ true | "عايز أتكلم مع موظف" |
| `AskAboutProducts` | العميل يسأل عن المنتجات | ❌ false | "عايز أشوف المنيو" |
| `GeneralQuestion` | سؤال عام | ❌ false | "إيه ساعات العمل؟" |

### ⚠️ Requirements

1. **Intent** يجب أن يكون واحد من القيم المدعومة أعلاه
2. **Confidence** يجب أن يكون بين `0.0` و `1.0`
3. **Entities** Dictionary يمكن أن يكون فارغاً `{}` إذا لم يتم استخراج كيانات
4. **DetectedLanguage** يجب أن يكون `"ar"` أو `"en"`
5. **ComplexityLevel** يجب أن يكون `"Low"`, `"Medium"`, أو `"High"`
6. **PriorityLevel** يجب أن يكون `"Low"`, `"Normal"`, `"High"`, أو `"Critical"`

### ❌ Error Handling

- إذا فشل AI Service، يجب إرجاع `GeneralQuestion` مع `Confidence: 0.0`
- لا يجب إلقاء Exception إلا في حالات خطأ فادحة (مثل: API Key غير صحيح)

---

## 📝 Contract 2: Sentiment Analysis / تحليل المشاعر

### Interface
```csharp
Task<Sentiment> AnalyzeSentimentAsync(
    string messageId,
    string messageText,
    string language = "ar"
);
```

### ✅ Input (ما يرسله Backend)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `messageId` | `string` | ✅ Yes | معرف الرسالة (Message ID) |
| `messageText` | `string` | ✅ Yes | نص الرسالة للتحليل |
| `language` | `string` | ❌ No | اللغة ("ar" أو "en") - Default: "ar" |

**Example Input:**
```csharp
AnalyzeSentimentAsync(
    messageId: "msg-789",
    messageText: "الخدمة كانت ممتازة شكرا ليكم",
    language: "ar"
)
```

### ✅ Output (ما يتوقعه Backend)

**Return Type:** `Sentiment`

```csharp
public class Sentiment
{
    public string SentimentId { get; set; }      // Auto-generated by Backend
    public string MessageId { get; set; }        // Provided by Backend
    public string SourceText { get; set; }        // Provided by Backend
    public DateTime AnalyzedAt { get; set; }     // Auto-set by Backend
    
    // AI Team must provide these:
    public double Score { get; set; }            // Required: -1.0 to 1.0
    public string Label { get; set; }            // Required: "Positive", "Negative", or "Neutral"
}
```

**Example Output:**
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

### ⚠️ Requirements

1. **Score** يجب أن يكون بين `-1.0` (سلبي جداً) و `1.0` (إيجابي جداً)
2. **Label** يجب أن يكون واحد من:
   - `"Positive"` (إيجابي)
   - `"Negative"` (سلبي)
   - `"Neutral"` (محايد)
3. **Score Mapping:**
   - `Score > 0.3` → `Label: "Positive"`
   - `Score < -0.3` → `Label: "Negative"`
   - `-0.3 <= Score <= 0.3` → `Label: "Neutral"`

### ❌ Error Handling

- إذا فشل AI Service، يجب إرجاع `Label: "Neutral"` مع `Score: 0.0`
- لا يجب إلقاء Exception إلا في حالات خطأ فادحة

---

## 📝 Contract 3: Speech-to-Text / تحويل الصوت إلى نص

### Interface

**⚠️ ملاحظة:** في التطبيق الفعلي (WebSocket)، Audio chunks تُرسل مباشرة (streaming)، وليس Base64.

```csharp
// WebSocket (streaming) - التطبيق الفعلي:
private async Task<string> ConvertAudioToTextAsync(
    byte[] audioChunk,  // Audio chunk مباشرة من WebSocket
    string? audioFormat
)

// REST API placeholder - للاختبار فقط:
private async Task<string> ConvertAudioToTextAsync(
    string audioDataBase64,  // Base64-encoded audio (placeholder only)
    string? audioFormat
)
```

### ✅ Input (ما يرسله Backend)

**WebSocket (streaming) - التطبيق الفعلي:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `audioChunk` | `byte[]` | ✅ Yes | Audio chunk مباشرة من WebSocket (streaming) |
| `audioFormat` | `string?` | ❌ No | Audio format ("audio/wav", "audio/mp3", "audio/webm", etc.) |

**REST API placeholder - للاختبار فقط:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `audioDataBase64` | `string` | ✅ Yes | Base64-encoded audio data (placeholder only) |
| `audioFormat` | `string?` | ❌ No | Audio format ("audio/wav", "audio/mp3", "audio/webm", etc.) |

**Example Input (WebSocket - streaming):**
```csharp
ConvertAudioToTextAsync(
    audioChunk: byte[],  // Audio chunk مباشرة من WebSocket
    audioFormat: "audio/wav"
)
```

**Example Input (REST API placeholder):**
```csharp
ConvertAudioToTextAsync(
    audioDataBase64: "UklGRiQAAABXQVZFZm10...",  // Base64 string (placeholder only)
    audioFormat: "audio/wav"
)
```

### ✅ Output (ما يتوقعه Backend)

**Return Type:** `string`

- نص منسوخ من الصوت
- بالعربية أو الإنجليزية حسب الصوت
- Plain text (بدون تنسيق خاص)

**Example Output:**
```
"عايز أطلب برجر كبير مع بطاطس"
```

أو:
```
"I want to order a large burger with fries"
```

### ⚠️ Requirements

1. **Supported Audio Formats:**
   - `audio/wav`
   - `audio/mp3`
   - `audio/webm`
   - `audio/m4a`

2. **Language Detection:**
   - يجب اكتشاف اللغة تلقائياً (عربي أو إنجليزي)
   - أو استخدام language hint من context إذا كان متوفراً

3. **Output Format:**
   - Plain text string
   - بدون علامات ترقيم إضافية
   - بدون تنسيق خاص

### ❌ Error Handling

- إذا فشل AI Service، يجب إرجاع `string.Empty` أو throw Exception
- Exception يجب أن تحتوي على رسالة خطأ واضحة

---

## 📝 Contract 4: Text-to-Speech / تحويل النص إلى صوت

### Interface
```csharp
private async Task<(string audioBase64, string audioFormat)> ConvertTextToAudioAsync(
    string text,
    VoiceSettingsDTO settings,
    string? dialect
)
```

### ✅ Input (ما يرسله Backend)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `text` | `string` | ✅ Yes | النص للتحويل إلى صوت |
| `settings` | `VoiceSettingsDTO` | ✅ Yes | إعدادات الصوت |
| `dialect` | `string?` | ❌ No | اللهجة ("Egyptian", "Standard Arabic", "English") |

**VoiceSettingsDTO Structure:**
```csharp
public class VoiceSettingsDTO
{
    public string AgentVoice { get; set; }           // Voice name/ID
    public string AgentVoiceProvider { get; set; }    // "Azure", "Google", etc.
    public double AgentVoiceSpeed { get; set; }       // 0.5 to 2.0
    public double AgentVoicePitch { get; set; }      // 0.5 to 2.0
    public string AgentVoiceLanguage { get; set; }    // "ar" or "en"
}
```

**Example Input:**
```csharp
ConvertTextToAudioAsync(
    text: "مرحبا بك، كيف يمكنني مساعدتك اليوم؟",
    settings: new VoiceSettingsDTO {
        AgentVoice = "ar-EG-SalmaNeural",
        AgentVoiceProvider = "Azure",
        AgentVoiceSpeed = 1.0,
        AgentVoicePitch = 1.0,
        AgentVoiceLanguage = "ar"
    },
    dialect: "Egyptian"
)
```

### ✅ Output (ما يتوقعه Backend)

**⚠️ ملاحظة:** في التطبيق الفعلي (WebSocket)، Audio chunks تُرسل مباشرة (streaming)، وليس Base64.

**Return Type (WebSocket - التطبيق الفعلي):**
- `byte[] audioChunk` - Audio chunk مباشرة من TTS service (streaming)
- `string audioFormat` - Audio format string (e.g., "audio/wav", "audio/mp3")

**Return Type (REST API placeholder - للاختبار فقط):**
- `(string audioBase64, string audioFormat)`
- `audioBase64`: Base64-encoded audio file
- `audioFormat`: Audio format string (e.g., "audio/wav", "audio/mp3")

**Example Output (REST API placeholder):**
```csharp
(
    audioBase64: "UklGRiQAAABXQVZFZm10...",  // Base64 string (placeholder only)
    audioFormat: "audio/wav"
)
```

**Example Output (WebSocket - streaming):**
```csharp
audioChunk: byte[]  // Audio chunk مباشرة (streaming)
audioFormat: "audio/wav"
```

### ⚠️ Requirements

1. **Voice Selection:**
   - يجب استخدام `settings.AgentVoice` لاختيار الصوت
   - إذا كان `dialect` متوفراً، يجب استخدامه لاختيار الصوت المناسب

2. **Voice Settings:**
   - `AgentVoiceSpeed`: يجب تطبيق السرعة (0.5 إلى 2.0)
   - `AgentVoicePitch`: يجب تطبيق النبرة (0.5 إلى 2.0)
   - `AgentVoiceLanguage`: يجب استخدام اللغة المحددة

3. **Audio Format:**
   - يجب إرجاع format متوافق (WAV, MP3, etc.)
   - Format يجب أن يكون مناسب للاستخدام في web browsers

### ❌ Error Handling

- إذا فشل AI Service، يجب throw Exception
- Exception يجب أن تحتوي على رسالة خطأ واضحة

---

## 🔄 Data Flow / تدفق البيانات

### 1. Chat Flow (Text)
```
Customer → Text Message
    ↓
Backend → DetectIntentAsync(recentMessages)
    ↓
AI Team → Returns DetectedIntentResultDTO
    ↓
Backend → Processes Intent & Generates Response
    ↓
Backend → AnalyzeSentimentAsync(messageText)
    ↓
AI Team → Returns Sentiment
    ↓
Backend → Saves to Database
    ↓
Backend → Returns Response to Customer
```

### 2. Voice Flow (Audio) - WebSocket Streaming
```
Customer → Audio Chunks (WebSocket Streaming) ⚠️ NOT Base64
    ↓
Backend → ConvertAudioToTextAsync(audioChunk)  // Streaming STT
    ↓
AI Team → Returns Transcribed Text (real-time)
    ↓
Backend → DetectIntentAsync(recentMessages)
    ↓
AI Team → Returns DetectedIntentResultDTO
    ↓
Backend → Processes Intent & Generates Response Text
    ↓
Backend → AnalyzeSentimentAsync(messageText)
    ↓
AI Team → Returns Sentiment
    ↓
Backend → ConvertTextToAudioAsync(responseText, settings)
    ↓
AI Team → Returns Audio Chunks (streaming) ⚠️ NOT Base64
    ↓
Backend → Sends Audio Chunks to Customer via WebSocket (streaming)
```

**⚠️ ملاحظة:** 
- الـ Voice سيتم عبر **WebSocket (SignalR)** وليس REST API
- الصوت يُرسل مباشرة من الجهاز (streaming) - **لا يتم حفظ الملفات**
- الـ REST API الحالي (`/api/CustomerVoice/message`) هو placeholder للاختبار فقط

---

## ✅ Acceptance Criteria / معايير القبول

### Contract 0: Response Generation ⭐
- [ ] يولد رد طبيعي ومحادثي
- [ ] يتضمن المعلومات المهمة من ActionData
- [ ] يدعم العربية والإنجليزية
- [ ] الرد جاهز للعرض مباشرة (Backend يمرره للـ Frontend بدون تعديل)

### Contract 1: Intent Detection
- [ ] يدعم جميع الـ Intents المذكورة
- [ ] يكتشف اللغة (عربي/إنجليزي)
- [ ] يستخرج الكيانات (Entities)
- [ ] يحدد Complexity Level
- [ ] يحدد Escalation Requirements
- [ ] Confidence بين 0.0 و 1.0

### Contract 2: Sentiment Analysis
- [ ] Score بين -1.0 و 1.0
- [ ] Label: Positive, Negative, أو Neutral
- [ ] يدعم العربية والإنجليزية
- [ ] Score mapping صحيح

### Contract 3: Speech-to-Text
- [ ] يدعم صيغ Audio متعددة (WAV, MP3, WebM)
- [ ] يكتشف اللغة تلقائياً
- [ ] يرجع نص واضح ومفهوم

### Contract 4: Text-to-Speech
- [ ] يدعم إعدادات الصوت (سرعة، نبرة)
- [ ] يدعم اللهجات (مصري، فصحى)
- [ ] يرجع Base64 audio صحيح
- [ ] Format متوافق مع web browsers

---

## 📊 Performance Requirements / متطلبات الأداء

### Response Time
- **Response Generation:** < 3 seconds ⭐
- **Intent Detection:** < 2 seconds
- **Sentiment Analysis:** < 1 second
- **Speech-to-Text:** < 5 seconds (حسب طول الصوت)
- **Text-to-Speech:** < 3 seconds (حسب طول النص)

### Availability
- **Uptime:** 99%+
- **Error Rate:** < 1%

---

## 🔒 Security Requirements / متطلبات الأمان

1. **API Keys:**
   - يجب حفظ API Keys في Environment Variables
   - لا يجب hardcode في الكود

2. **Data Privacy:**
   - لا يجب حفظ أو تسجيل بيانات العملاء خارج النظام
   - يجب حذف البيانات المؤقتة بعد المعالجة

3. **Error Messages:**
   - لا يجب كشف معلومات حساسة في Error Messages

---

## 📝 Testing Requirements / متطلبات الاختبار

### Unit Tests
- [ ] Test جميع الـ Contracts
- [ ] Test Error Handling
- [ ] Test Edge Cases

### Integration Tests
- [ ] Test التكامل مع Backend
- [ ] Test Data Flow الكامل
- [ ] Test Performance

---

## 📚 Examples / أمثلة

### Example 1: Intent Detection (Arabic)
**Input:**
```csharp
DetectIntentAsync(
    "business-123",
    "interaction-456",
    new[] { "مرحبا", "عايز أطلب برجر كبير" }
)
```

**Expected Output:**
```json
{
    "intent": "CreateOrder",
    "entities": { "product": "برجر", "size": "كبير" },
    "confidence": 0.95,
    "requiresAction": true,
    "detectedLanguage": "ar",
    "detectedDialect": "Egyptian",
    "complexityLevel": "Low",
    "requiresEscalation": false,
    "priorityLevel": "Normal"
}
```

### Example 2: Sentiment Analysis (English)
**Input:**
```csharp
AnalyzeSentimentAsync(
    "msg-789",
    "The service was excellent, thank you!",
    "en"
)
```

**Expected Output:**
```json
{
    "sentimentId": "sentiment-123",
    "messageId": "msg-789",
    "sourceText": "The service was excellent, thank you!",
    "analyzedAt": "2024-01-15T10:30:00Z",
    "score": 0.88,
    "label": "Positive"
}
```

---

## ✅ Sign-off / التوقيع

**Backend Team:**
- ✅ جميع الـ Contracts محددة بوضوح
- ✅ جميع الـ Interfaces جاهزة
- ✅ جميع الـ DTOs جاهزة

**AI Team:**
- ⏳ يجب تنفيذ جميع الـ Contracts
- ⏳ يجب الالتزام بالمتطلبات
- ⏳ يجب اختبار التكامل

---

**آخر تحديث:** 2024-01-15
**الإصدار:** 1.0

