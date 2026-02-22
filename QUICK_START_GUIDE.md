# ⚡ دليل البدء السريع - AI Endpoints Testing

## 🚀 البدء في 5 دقائق

### الخطوة 1: تشغيل المشروع
```bash
cd "D:\Users\Shazly\Desktop\assignments\grad-project"
dotnet run --project "digital employee"
```

### الخطوة 2: فتح Swagger
افتح المتصفح على: **https://localhost:44361/swagger** (IIS Express) ✅
أو **https://localhost:7119/swagger** (dotnet run)

### الخطوة 3: استيراد Postman
1. افتح Postman
2. Import → `AI_Endpoints_Postman_Collection.json`
3. Import → `Postman_Environment.json`
4. اختر Environment: **"AI Endpoints - Test Environment"**

### الخطوة 4: الحصول على Token
1. في Postman: **Authentication → Login - Owner**
2. اضغط **Send**
3. ✅ Token محفوظ تلقائياً!

### الخطوة 5: اختبار Chat
1. **Customer Chat → Send Chat Message (Arabic)**
2. عدّل `businessId` في Environment
3. اضغط **Send**
4. ✅ جاهز!

---

## 📍 Swagger URLs

| Environment | URL |
|-------------|-----|
| IIS Express HTTPS | `https://localhost:44361/swagger` | ✅ (الافتراضي) |
| IIS Express HTTP | `http://localhost:9875/swagger` |
| HTTPS (Development) | `https://localhost:7119/swagger` |
| HTTP (Development) | `http://localhost:5157/swagger` |

---

## 🔑 الحصول على Token

### في Postman (موصى به):
1. **Authentication → Login - Owner**
2. Token يُحفظ تلقائياً في `jwtToken`

### في Swagger:
1. **POST /api/Auth/login**
2. Body:
```json
{
    "email": "owner@test.com",
    "password": "Owner123!"
}
```
3. انسخ `token` من Response

---

## 📝 Environment Variables المهمة

| Variable | القيمة | ملاحظات |
|----------|--------|---------|
| `baseUrl` | `https://localhost:44361` | عنوان الـ API (IIS Express) |
| `swaggerUrl` | `https://localhost:44361/swagger` | رابط Swagger (IIS Express) |
| `businessId` | `your-business-id` | من قاعدة البيانات |
| `jwtToken` | `auto-saved` | من Login |

---

## ✅ Test Checklist

- [ ] المشروع يعمل
- [ ] Swagger مفتوح
- [ ] Postman Collection مستورد
- [ ] Environment مستورد ومفعل
- [ ] Token موجود
- [ ] `businessId` معرّف
- [ ] جاهز للاختبار!

---

**للمزيد من التفاصيل، راجع `TEST_ENVIRONMENT_SETUP.md`**

