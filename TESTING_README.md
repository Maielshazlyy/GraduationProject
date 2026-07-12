# 🧪 دليل الاختبار الكامل - AI Endpoints

## 📚 الملفات المتاحة

### 1. **Postman Collection**
- **الملف**: `AI_Endpoints_Postman_Collection.json`
- **المحتوى**: جميع الـ endpoints المتعلقة بالـ AI
- **يشمل**: Authentication, Customer Chat, Customer Voice, Sentiment Analysis

### 2. **Postman Environment**
- **الملف**: `Postman_Environment.json`
- **المحتوى**: جميع المتغيرات المطلوبة للاختبار
- **يشمل**: URLs, Tokens, Test Data

### 3. **دليل الإعداد الكامل**
- **الملف**: `TEST_ENVIRONMENT_SETUP.md`
- **المحتوى**: شرح مفصل لجميع الإعدادات

### 4. **دليل البدء السريع**
- **الملف**: `QUICK_START_GUIDE.md`
- **المحتوى**: خطوات سريعة للبدء في 5 دقائق

### 5. **أمثلة Test Data**
- **الملف**: `TEST_DATA_EXAMPLES.json`
- **المحتوى**: أمثلة لجميع الـ Requests والـ Responses

### 6. **دليل Postman Collection**
- **الملف**: `POSTMAN_COLLECTION_README.md`
- **المحتوى**: شرح تفصيلي لكل endpoint

---

## 🚀 البدء السريع (5 دقائق)

### 1. تشغيل المشروع
```bash
cd "D:\Users\Shazly\Desktop\assignments\grad-project"
dotnet run --project "digital employee"
```

### 2. فتح Swagger
افتح: **https://localhost:44361/swagger** (IIS Express) ✅
أو **https://localhost:7119/swagger** (dotnet run)

### 3. استيراد Postman
1. Import → `AI_Endpoints_Postman_Collection.json`
2. Import → `Postman_Environment.json`
3. اختر Environment: **"AI Endpoints - Test Environment"**

### 4. الحصول على Token
**Authentication → Login - Owner** → Send ✅

### 5. اختبار Chat
**Customer Chat → Send Chat Message (Arabic)** → Send ✅

---

## 🌐 Swagger URLs

| Environment | URL |
|-------------|-----|
| **IIS Express HTTPS** | `https://localhost:44361/swagger` | ✅ (الافتراضي) |
| **IIS Express HTTP** | `http://localhost:9875/swagger` |
| **HTTPS (dotnet run)** | `https://localhost:7119/swagger` |
| **HTTP (dotnet run)** | `http://localhost:5157/swagger` |

---

## 🔑 الحصول على JWT Token

### الطريقة 1: Postman (موصى به) ⭐
1. **Authentication → Login - Owner**
2. اضغط **Send**
3. ✅ Token يُحفظ تلقائياً في `jwtToken`

### الطريقة 2: Swagger
1. افتح Swagger
2. **POST /api/Auth/login**
3. Body:
```json
{
    "email": "owner@test.com",
    "password": "Owner123!"
}
```
4. انسخ `token` من Response

### الطريقة 3: curl
```bash
curl -X POST "https://localhost:44361/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"owner@test.com","password":"Owner123!"}'
```

---

## 📝 Environment Variables

### متغيرات أساسية:
- `baseUrl`: `https://localhost:44361` (IIS Express)
- `swaggerUrl`: `https://localhost:44361/swagger` (IIS Express)
- `businessId`: (من قاعدة البيانات)
- `jwtToken`: (يُحفظ تلقائياً من Login)

### متغيرات Test Users:
- `testAdminEmail`: `admin@test.com`
- `testAdminPassword`: `Admin123!`
- `testOwnerEmail`: `owner@test.com`
- `testOwnerPassword`: `Owner123!`
- `testAgentEmail`: `agent@test.com`
- `testAgentPassword`: `Agent123!`

---

## 🎯 سيناريوهات الاختبار

### سيناريو 1: محادثة كاملة بالعربية
1. **Get Capabilities** → تحقق من Chat/Voice
2. **Send Chat Message (Arabic)** → `"عايز أطلب برجر"`
3. احفظ `interactionId` من Response
4. **Send Chat Message - Ask Order Status** → استخدم `interactionId`
5. **Get Sentiment by Message ID** → تحقق من تحليل المشاعر

### سيناريو 2: محادثة بالإنجليزية
1. **Send Chat Message (English)** → `"I want to order a pizza"`
2. تحقق من اكتشاف اللغة الإنجليزية
3. **Send Chat Message - Request Human Agent** → `"I want to talk to a human agent"`
4. تحقق من Escalation و Ticket Creation

### سيناريو 3: مكالمة صوتية
1. **Initialize Voice Session** → أنشئ جلسة صوتية
2. احفظ `interactionId` و `callSessionId`
3. **Send Voice Message** → أرسل رسالة صوتية
4. **Submit Voice Feedback** → أرسل تقييم

---

## 📊 Sample Messages

### العربية:
- `"عايز أطلب برجر"` - Create Order
- `"عايز أعرف حالة الطلب"` - Ask Order Status
- `"مش عاجبني الطلب"` - Complaint
- `"عايز أتكلم مع موظف بشري"` - Request Human Agent
- `"عايز أشوف المنيو"` - Ask Products

### English:
- `"I want to order a burger"` - Create Order
- `"What is my order status"` - Ask Order Status
- `"I'm not satisfied"` - Complaint
- `"I want to talk to a human agent"` - Request Human Agent
- `"Show me the menu"` - Ask Products

---

## 🔍 JWT Token Structure

### Claims في الـ Token:
- `sub` (NameIdentifier): User ID
- `email`: Email
- `name`: Full Name
- `role`: Role (Admin/Owner/Agent/User)
- `BusinessId`: Business ID (إن وجد)

### Token Expiration:
- **3 أيام** من وقت الإنشاء

### فك التشفير:
استخدم [jwt.io](https://jwt.io) لفك تشفير الـ Token

---

## 🐛 استكشاف الأخطاء

| الخطأ | الحل |
|------|------|
| **401 Unauthorized** | تحقق من Token أو سجل دخول مرة أخرى |
| **400 Bad Request** | تحقق من الـ Body والحقول المطلوبة |
| **404 Not Found** | تحقق من `businessId` أو `interactionId` |
| **500 Internal Server Error** | تحقق من سجلات الخادم |

---

## 📁 هيكل الملفات

```
grad-project/
├── AI_Endpoints_Postman_Collection.json    # Postman Collection
├── Postman_Environment.json                # Postman Environment
├── TEST_ENVIRONMENT_SETUP.md               # دليل الإعداد الكامل
├── QUICK_START_GUIDE.md                    # دليل البدء السريع
├── POSTMAN_COLLECTION_README.md            # شرح Postman Collection
├── TEST_DATA_EXAMPLES.json                 # أمثلة Test Data
└── TESTING_README.md                       # هذا الملف
```

---

## ✅ Checklist

- [ ] المشروع يعمل
- [ ] Swagger مفتوح
- [ ] Postman Collection مستورد
- [ ] Postman Environment مستورد ومفعل
- [ ] JWT Token موجود
- [ ] `businessId` معرّف
- [ ] جاهز للاختبار! 🚀

---

## 📚 المزيد من المعلومات

- **دليل الإعداد الكامل**: `TEST_ENVIRONMENT_SETUP.md`
- **دليل البدء السريع**: `QUICK_START_GUIDE.md`
- **شرح Postman Collection**: `POSTMAN_COLLECTION_README.md`
- **أمثلة Test Data**: `TEST_DATA_EXAMPLES.json`

---

**تم إعداد بيئة الاختبار بنجاح! ابدأ الاختبار الآن! 🎉**

