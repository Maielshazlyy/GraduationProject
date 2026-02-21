# 📬 Postman Collection - AI Endpoints

## 📋 نظرة عامة
هذا الـ Postman Collection يحتوي على جميع الـ endpoints المتعلقة بالـ AI في المشروع:
- **Customer Chat** - الدردشة النصية
- **Customer Voice** - المكالمات الصوتية
- **Sentiment Analysis** - تحليل المشاعر

---

## 🚀 كيفية الاستخدام

### 1. استيراد الـ Collection في Postman

1. افتح Postman
2. اضغط على **Import** (أعلى يسار)
3. اختر ملف `AI_Endpoints_Postman_Collection.json`
4. سيتم استيراد الـ Collection بنجاح

### 2. إعداد المتغيرات (Variables)

بعد استيراد الـ Collection، يجب تعيين المتغيرات التالية:

#### متغيرات مطلوبة:
- **`baseUrl`**: عنوان الـ API (افتراضي: `https://localhost:7000`)
  - يمكن تغييره إلى: `http://localhost:5000` أو أي عنوان آخر

#### متغيرات اختيارية (لكن مهمة للاختبار):
- **`businessId`**: معرف العمل (Business ID)
- **`customerId`**: معرف العميل (Customer ID) - اختياري
- **`interactionId`**: معرف التفاعل (Interaction ID) - سيتم إنشاؤه تلقائياً
- **`messageId`**: معرف الرسالة (Message ID) - للاستعلام عن Sentiment
- **`sentimentId`**: معرف تحليل المشاعر (Sentiment ID)
- **`menuItemId`**: معرف عنصر المنيو (MenuItem ID) - للتوصيات
- **`callSessionId`**: معرف جلسة المكالمة (Call Session ID)
- **`jwtToken`**: JWT Token - مطلوب للـ endpoints التي تحتاج مصادقة

#### كيفية تعيين المتغيرات:
1. في Postman، اضغط على الـ Collection
2. اختر تبويب **Variables**
3. قم بتعبئة القيم المطلوبة

---

## 📝 تفاصيل الـ Endpoints

### 1. Customer Chat Endpoints

#### ✅ Get Capabilities
- **Method**: `GET`
- **URL**: `/api/CustomerChat/capabilities/{businessId}`
- **Description**: معرفة القدرات المتاحة (Chat, Voice)
- **Authentication**: ❌ غير مطلوب

#### ✅ Send Chat Message (Arabic)
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Body Example**:
```json
{
    "businessId": "your-business-id",
    "customerId": "customer-id-optional",
    "interactionId": null,
    "channel": "WebChat",
    "message": "عايز أطلب برجر"
}
```
- **Description**: إرسال رسالة نصية بالعربية (Create Order intent)
- **Authentication**: ❌ غير مطلوب

#### ✅ Send Chat Message (English)
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Body Example**:
```json
{
    "businessId": "your-business-id",
    "customerId": "customer-id-optional",
    "interactionId": null,
    "channel": "WebChat",
    "message": "I want to order a burger"
}
```
- **Description**: إرسال رسالة نصية بالإنجليزية
- **Authentication**: ❌ غير مطلوب

#### ✅ Send Chat Message - Ask Order Status
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Description**: الاستفسار عن حالة الطلب
- **Note**: يجب تعيين `interactionId` من رسالة سابقة

#### ✅ Send Chat Message - Complaint
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Description**: إرسال شكوى (سيتم إنشاء Ticket)

#### ✅ Send Chat Message - Request Human Agent
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Description**: طلب موظف بشري (سيتم Escalation)

#### ✅ Send Chat Message - Ask About Products
- **Method**: `POST`
- **URL**: `/api/CustomerChat/message`
- **Description**: الاستفسار عن المنتجات/المنيو

#### ✅ Get Order Recommendations
- **Method**: `POST`
- **URL**: `/api/CustomerChat/recommendations`
- **Body Example**:
```json
{
    "businessId": "your-business-id",
    "mainMenuItemId": "menu-item-id"
}
```
- **Description**: الحصول على توصيات منتجات بناءً على منتج رئيسي

---

### 2. Customer Voice Endpoints

#### ✅ Initialize Voice Session
- **Method**: `POST`
- **URL**: `/api/CustomerVoice/session/initialize`
- **Body Example**:
```json
{
    "businessId": "your-business-id",
    "customerId": "customer-id-optional",
    "callSessionId": "call-session-123"
}
```
- **Description**: بدء جلسة مكالمة صوتية جديدة
- **Response**: سيعيد `Interaction` object مع `InteractionId`

#### ✅ Send Voice Message (Text)
- **Method**: `POST`
- **URL**: `/api/CustomerVoice/message`
- **Body Example**:
```json
{
    "businessId": "your-business-id",
    "customerId": "customer-id",
    "interactionId": "interaction-id-from-initialize",
    "channel": "Voice",
    "callSessionId": "call-session-id",
    "message": "عايز أطلب برجر",
    "audioData": null,
    "audioFormat": null
}
```
- **Description**: إرسال رسالة صوتية (يمكن استخدام نص للاختبار)
- **Note**: `audioData` و `audioFormat` حالياً placeholder

#### ✅ Mark Interaction Interrupted
- **Method**: `POST`
- **URL**: `/api/CustomerVoice/interaction/{interactionId}/interrupt`
- **Description**: تحديد التفاعل كمقطوع (انقطاع المكالمة)

#### ✅ Submit Voice Feedback
- **Method**: `POST`
- **URL**: `/api/CustomerVoice/feedback`
- **Body Example**:
```json
{
    "interactionId": "interaction-id",
    "rating": 5,
    "comment": "ممتاز"
}
```
- **Description**: إرسال تقييم بعد انتهاء المكالمة

#### ✅ Get Voice Settings
- **Method**: `GET`
- **URL**: `/api/CustomerVoice/settings/{businessId}`
- **Description**: الحصول على إعدادات الصوت للعمل

---

### 3. Sentiment Analysis Endpoints

**⚠️ ملاحظة**: جميع endpoints تحليل المشاعر تحتاج **Authentication** (JWT Token)

#### ✅ Get All Sentiments
- **Method**: `GET`
- **URL**: `/api/Sentiment`
- **Authentication**: ✅ مطلوب (Agent/Owner/Admin)
- **Header**: `Authorization: Bearer {jwtToken}`

#### ✅ Get Sentiment by Message ID
- **Method**: `GET`
- **URL**: `/api/Sentiment/message/{messageId}`
- **Authentication**: ✅ مطلوب (Agent/Owner/Admin)
- **Description**: الحصول على تحليل المشاعر لرسالة معينة

#### ✅ Get Sentiments by Business ID
- **Method**: `GET`
- **URL**: `/api/Sentiment/business/{businessId}`
- **Authentication**: ✅ مطلوب (Owner/Admin)
- **Description**: الحصول على جميع تحليلات المشاعر لعمل معين

#### ✅ Get Sentiment by ID
- **Method**: `GET`
- **URL**: `/api/Sentiment/{sentimentId}`
- **Authentication**: ✅ مطلوب (Agent/Owner/Admin)
- **Description**: الحصول على تحليل مشاعر معين

---

## 🔄 سيناريو اختبار كامل

### سيناريو 1: محادثة كاملة بالعربية

1. **Get Capabilities**
   - تحقق من أن Chat و Voice متاحين

2. **Send Chat Message (Arabic) - Create Order**
   - أرسل: `"عايز أطلب برجر"`
   - احفظ `interactionId` من الـ Response

3. **Send Chat Message - Ask Order Status**
   - استخدم `interactionId` من الخطوة السابقة
   - أرسل: `"عايز أعرف حالة الطلب"`

4. **Get Sentiment by Message ID** (بعد تسجيل الدخول)
   - استخدم `messageId` من الرسائل السابقة
   - تحقق من تحليل المشاعر

### سيناريو 2: محادثة بالإنجليزية

1. **Send Chat Message (English)**
   - أرسل: `"I want to order a pizza"`
   - تحقق من أن النظام اكتشف اللغة الإنجليزية

2. **Send Chat Message - Request Human Agent**
   - أرسل: `"I want to talk to a human agent"`
   - تحقق من أن النظام أنشأ Ticket و Escalation

### سيناريو 3: مكالمة صوتية

1. **Initialize Voice Session**
   - أنشئ جلسة صوتية جديدة
   - احفظ `interactionId` و `callSessionId`

2. **Send Voice Message**
   - استخدم `interactionId` من الخطوة السابقة
   - أرسل رسالة صوتية (نص للاختبار)

3. **Submit Voice Feedback**
   - أرسل تقييم بعد انتهاء المكالمة

---

## 🔑 الحصول على JWT Token

للحصول على JWT Token للـ endpoints التي تحتاج مصادقة:

1. استخدم endpoint: `POST /api/Auth/login`
2. أرسل:
```json
{
    "email": "your-email@example.com",
    "password": "your-password"
}
```
3. احفظ `token` من الـ Response
4. ضعه في متغير `jwtToken` في Postman

---

## 📌 ملاحظات مهمة

1. **Sentiment Analysis**: يتم إنشاؤه تلقائياً عند إرسال رسالة في Chat أو Voice
2. **InteractionId**: يتم إنشاؤه تلقائياً في أول رسالة، استخدمه في الرسائل التالية
3. **Language Detection**: النظام يكتشف اللغة تلقائياً (Arabic/English)
4. **Dialect Detection**: النظام يكتشف اللهجة (Egyptian/Standard Arabic)
5. **Escalation**: عند طلب موظف بشري، سيتم إنشاء Ticket من نوع `HumanEscalation`

---

## 🐛 استكشاف الأخطاء

### خطأ 400 Bad Request
- تحقق من أن `businessId` موجود وصحيح
- تحقق من أن الـ Body يحتوي على جميع الحقول المطلوبة

### خطأ 401 Unauthorized
- تحقق من أن JWT Token صحيح وغير منتهي الصلاحية
- تأكد من أن المستخدم لديه الصلاحيات المطلوبة (Agent/Owner/Admin)

### خطأ 404 Not Found
- تحقق من أن `businessId` موجود في قاعدة البيانات
- تحقق من أن `interactionId` أو `messageId` صحيح

### خطأ 500 Internal Server Error
- تحقق من سجلات الخادم (Server Logs)
- تأكد من أن قاعدة البيانات متصلة

---

## 📚 أمثلة Response

### Response من Send Chat Message:
```json
{
    "interactionId": "interaction-guid",
    "replyText": "تمام، سجلت لك طلب جديد...",
    "orderId": "order-guid",
    "ticketId": null,
    "cart": {
        "totalPrice": 50.00,
        "items": [...]
    },
    "recommendations": [
        {
            "menuItemId": "item-guid",
            "name": "بطاطس",
            "price": 10.00,
            "reason": "Popular side with burgers"
        }
    ],
    "isInterrupted": false
}
```

### Response من Get Sentiment:
```json
{
    "sentimentId": "sentiment-guid",
    "messageId": "message-guid",
    "sourceText": "عايز أطلب برجر",
    "label": "Positive",
    "score": 0.8,
    "analyzedAt": "2024-01-15T10:30:00Z"
}
```

---

**تم إنشاء هذا الـ Collection لاختبار جميع الـ AI endpoints في المشروع.**

