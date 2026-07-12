# 🧪 دليل إعداد بيئة الاختبار الكاملة

## 📋 نظرة عامة
هذا الدليل يحتوي على كل ما تحتاجه لإعداد بيئة اختبار كاملة للـ AI Endpoints:
- ✅ Swagger URLs
- ✅ Test Environment Variables
- ✅ Sample Tokens (كيفية الحصول عليها)
- ✅ Postman Collection & Environment
- ✅ خطوات الإعداد السريع

---

## 🌐 Swagger URLs

### IIS Express (الافتراضي):
- **HTTPS**: `https://localhost:44361/swagger` ✅ (الموصى به)
- **HTTP**: `http://localhost:9875/swagger`

### Development (dotnet run):
- **HTTPS**: `https://localhost:7119/swagger`
- **HTTP**: `http://localhost:5157/swagger`

### كيفية الوصول:
1. شغل المشروع (F5 في Visual Studio أو `dotnet run`)
2. افتح المتصفح على: **`https://localhost:44361/swagger`** (إذا كان IIS Express)
3. ستجد Swagger UI مع جميع الـ endpoints

---

## 🔧 إعداد Postman

### الخطوة 1: استيراد الـ Collection
1. افتح Postman
2. اضغط على **Import** (أعلى يسار)
3. اختر ملف `AI_Endpoints_Postman_Collection.json`
4. ✅ سيتم استيراد الـ Collection

### الخطوة 2: استيراد الـ Environment
1. في Postman، اضغط على **Import** مرة أخرى
2. اختر ملف `Postman_Environment.json`
3. ✅ سيتم استيراد الـ Environment

### الخطوة 3: تفعيل الـ Environment
1. في أعلى يمين Postman، اضغط على القائمة المنسدلة (Environment)
2. اختر **"AI Endpoints - Test Environment"**
3. ✅ الآن جميع المتغيرات جاهزة

---

## 🔑 الحصول على JWT Tokens

### الطريقة 1: استخدام Postman Collection (موصى به)

#### أ) تسجيل مستخدم جديد:
1. في Postman Collection، اذهب إلى **Authentication → Register New User**
2. عدّل الـ Body إذا لزم الأمر:
```json
{
    "fullName": "Test Owner",
    "email": "owner@test.com",
    "password": "Owner123!",
    "businessId": null
}
```
3. اضغط **Send**
4. ✅ سيتم حفظ الـ Token تلقائياً في `jwtToken` variable

#### ب) تسجيل الدخول:
1. اذهب إلى **Authentication → Login - Owner** (أو Admin/Agent)
2. اضغط **Send**
3. ✅ سيتم حفظ الـ Token تلقائياً

### الطريقة 2: استخدام Swagger

1. افتح Swagger: `https://localhost:44361/swagger` (IIS Express) أو `https://localhost:7119/swagger` (dotnet run)
2. اذهب إلى **Auth → POST /api/Auth/login**
3. اضغط **Try it out**
4. أدخل البيانات:
```json
{
    "email": "your-email@example.com",
    "password": "your-password"
}
```
5. اضغط **Execute**
6. انسخ الـ `token` من الـ Response
7. ضعه في Postman Environment variable `jwtToken`

### الطريقة 3: استخدام curl

```bash
curl -X POST "https://localhost:44361/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "owner@test.com",
    "password": "Owner123!"
  }'
```

---

## 📝 Environment Variables

### المتغيرات الأساسية (مطلوبة):

| Variable | القيمة الافتراضية | الوصف |
|----------|-------------------|-------|
| `baseUrl` | `https://localhost:44361` | عنوان الـ API (HTTPS - IIS Express) |
| `baseUrlHttp` | `http://localhost:9875` | عنوان الـ API (HTTP - IIS Express) |
| `swaggerUrl` | `https://localhost:44361/swagger` | رابط Swagger (HTTPS - IIS Express) |
| `swaggerUrlHttp` | `http://localhost:9875/swagger` | رابط Swagger (HTTP - IIS Express) |

### المتغيرات للاختبار:

| Variable | الوصف | كيفية الحصول عليها |
|----------|-------|-------------------|
| `businessId` | معرف العمل | من قاعدة البيانات أو بعد إنشاء Business |
| `customerId` | معرف العميل | اختياري - من قاعدة البيانات |
| `interactionId` | معرف التفاعل | يتم إنشاؤه تلقائياً في أول رسالة |
| `messageId` | معرف الرسالة | من Response رسالة سابقة |
| `sentimentId` | معرف تحليل المشاعر | من Response Sentiment Analysis |
| `menuItemId` | معرف عنصر المنيو | من قاعدة البيانات |
| `callSessionId` | معرف جلسة المكالمة | يتم إنشاؤه عند Initialize Voice Session |

### متغيرات المصادقة:

| Variable | الوصف | كيفية الحصول عليها |
|----------|-------|-------------------|
| `jwtToken` | JWT Token الحالي | من Login/Register |
| `adminToken` | Token للـ Admin | من Login - Admin |
| `ownerToken` | Token للـ Owner | من Login - Owner |
| `agentToken` | Token للـ Agent | من Login - Agent |

### متغيرات بيانات الاختبار:

| Variable | القيمة الافتراضية | الوصف |
|----------|-------------------|-------|
| `testAdminEmail` | `admin@test.com` | إيميل Admin للاختبار |
| `testAdminPassword` | `Admin123!` | كلمة مرور Admin |
| `testOwnerEmail` | `owner@test.com` | إيميل Owner للاختبار |
| `testOwnerPassword` | `Owner123!` | كلمة مرور Owner |
| `testAgentEmail` | `agent@test.com` | إيميل Agent للاختبار |
| `testAgentPassword` | `Agent123!` | كلمة مرور Agent |

---

## 🚀 سيناريو اختبار سريع

### 1. إعداد أولي (مرة واحدة):

```bash
# 1. شغل المشروع
cd "D:\Users\Shazly\Desktop\assignments\grad-project"
dotnet run --project "digital employee"

# 2. افتح Swagger
# https://localhost:44361/swagger (IIS Express)
# أو https://localhost:7119/swagger (dotnet run)
```

### 2. في Postman:

#### أ) الحصول على Token:
1. **Authentication → Login - Owner**
2. اضغط **Send**
3. ✅ Token محفوظ تلقائياً

#### ب) تعيين BusinessId:
1. بعد Login، تحقق من Response
2. انسخ `businessId` من Response
3. ضعه في Environment variable `businessId`

#### ج) اختبار Chat:
1. **Customer Chat → Get Capabilities**
   - استخدم `{{businessId}}` في URL
   - اضغط **Send**
2. **Customer Chat → Send Chat Message (Arabic)**
   - اضغط **Send**
   - ✅ احفظ `interactionId` من Response
3. **Customer Chat → Send Chat Message - Ask Order Status**
   - استخدم `{{interactionId}}` من الخطوة السابقة
   - اضغط **Send**

#### د) اختبار Sentiment Analysis:
1. بعد إرسال رسالة، احفظ `messageId` من Response
2. **Sentiment Analysis → Get Sentiment by Message ID**
   - استخدم `{{messageId}}`
   - ✅ Token موجود تلقائياً في Header

---

## 📊 Sample Data للاختبار

### Sample BusinessId:
```
استخدم BusinessId من قاعدة البيانات الخاصة بك
أو أنشئ Business جديد من Swagger:
POST /api/Business
```

### Sample Messages للاختبار:

#### العربية:
- **Create Order**: `"عايز أطلب برجر"`
- **Ask Order Status**: `"عايز أعرف حالة الطلب"`
- **Complaint**: `"مش عاجبني الطلب"`
- **Request Human**: `"عايز أتكلم مع موظف بشري"`
- **Ask Products**: `"عايز أشوف المنيو"`

#### English:
- **Create Order**: `"I want to order a burger"`
- **Ask Order Status**: `"What is my order status"`
- **Complaint**: `"I'm not satisfied with my order"`
- **Request Human**: `"I want to talk to a human agent"`
- **Ask Products**: `"Show me the menu"`

---

## 🔍 التحقق من الـ Token

### فك تشفير JWT Token:
يمكنك استخدام [jwt.io](https://jwt.io) لفك تشفير الـ Token ومعرفة محتواه:

**Claims في الـ Token:**
- `sub` (NameIdentifier): User ID
- `email`: Email
- `name`: Full Name
- `role`: Role (Admin/Owner/Agent/User)
- `BusinessId`: Business ID (إن وجد)

### مثال على Token Structure:
```json
{
  "sub": "user-guid",
  "email": "owner@test.com",
  "name": "Test Owner",
  "role": "Owner",
  "BusinessId": "business-guid",
  "exp": 1234567890,
  "iss": "http://localhost:5157",
  "aud": "http://localhost:5157"
}
```

---

## 🛠️ إعداد قاعدة البيانات

### Connection String:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=DigitalEmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

### إنشاء قاعدة البيانات:
```bash
# في Package Manager Console أو Terminal
cd DAL
dotnet ef database update
```

---

## 📌 ملاحظات مهمة

### 1. SSL Certificate:
إذا واجهت مشكلة مع HTTPS:
- استخدم HTTP: `http://localhost:5157`
- أو اضغط "Advanced" في المتصفح و "Proceed to localhost"

### 2. CORS:
الـ API يدعم CORS من أي Origin في Development:
```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```

### 3. Token Expiration:
- الـ Token صالح لمدة **3 أيام**
- بعد انتهاء الصلاحية، قم بتسجيل الدخول مرة أخرى

### 4. Ports:
- **HTTP**: `5157`
- **HTTPS**: `7119`
- **IIS Express HTTP**: `9875`
- **IIS Express HTTPS**: `44361`

---

## 🐛 استكشاف الأخطاء الشائعة

### خطأ: "Swagger not found"
**الحل:**
- تأكد من أن المشروع يعمل
- تحقق من الـ Port في `launchSettings.json`
- جرب HTTP بدلاً من HTTPS

### خطأ: "401 Unauthorized"
**الحل:**
- تحقق من أن الـ Token موجود في Environment
- تأكد من أن الـ Token لم ينتهِ (3 أيام)
- تحقق من صيغة Header: `Bearer {token}` (مع مسافة بعد Bearer)

### خطأ: "BusinessId not found"
**الحل:**
- أنشئ Business جديد من Swagger
- أو استخدم BusinessId موجود في قاعدة البيانات
- تأكد من أن المستخدم مرتبط بـ Business

### خطأ: "Connection refused"
**الحل:**
- تأكد من أن المشروع يعمل
- تحقق من الـ Port
- جرب إعادة تشغيل المشروع

---

## 📚 روابط مفيدة

- **Swagger UI**: `https://localhost:44361/swagger` (IIS Express) أو `https://localhost:7119/swagger` (dotnet run)
- **Postman Collection**: `AI_Endpoints_Postman_Collection.json`
- **Postman Environment**: `Postman_Environment.json`
- **JWT Decoder**: https://jwt.io

---

## ✅ Checklist قبل البدء

- [ ] المشروع يعمل بدون أخطاء
- [ ] قاعدة البيانات متصلة ومحدثة
- [ ] Swagger يعمل على `https://localhost:44361/swagger` (IIS Express)
- [ ] Postman Collection مستورد
- [ ] Postman Environment مستورد ومفعل
- [ ] تم الحصول على JWT Token
- [ ] `businessId` معرّف في Environment
- [ ] جاهز للاختبار! 🚀

---

**تم إعداد بيئة الاختبار بنجاح! يمكنك الآن البدء في اختبار جميع الـ AI Endpoints.**

