# دليل استخدام API - حل مشكلة 401 Unauthorized

## المشكلة
عند اختبار الـ endpoints في Swagger، تحصل على error 401 Unauthorized.

## الحل

### 1. الحصول على JWT Token

#### أ) التسجيل (Register)
```
POST /api/Auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!",
  "fullName": "Test User",
  "role": "Owner",
  "businessId": "your-business-id"
}
```

**الرد:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-15T10:30:00Z",
  "userId": "...",
  "email": "test@example.com",
  "fullName": "Test User",
  "role": "Owner",
  "businessId": "..."
}
```

#### ب) تسجيل الدخول (Login)
```
POST /api/Auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!"
}
```

**الرد:** نفس الرد أعلاه مع الـ token.

### 2. استخدام الـ Token في Swagger

1. **انسخ الـ Token** من الرد (القيمة في `token` field)
2. **افتح Swagger UI** (عادة `http://localhost:5157/swagger`)
3. **اضغط على زر "Authorize"** (القفل 🔒 في أعلى الصفحة)
4. **في حقل "Value"**، أدخل:
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
   **مهم:** يجب أن تبدأ بـ `Bearer ` (مع مسافة بعدها) ثم الـ token
5. **اضغط "Authorize"** ثم **"Close"**
6. الآن يمكنك اختبار أي endpoint

### 3. استخدام الـ Token في Postman/Insomnia

في **Headers**، أضف:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 4. استخدام الـ Token في curl

```bash
curl -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
     http://localhost:5157/api/Business
```

## نصائح مهمة

1. **الـ Token صالح لمدة 3 أيام** - إذا انتهت صلاحيته، ستحصل على 401. قم بتسجيل الدخول مرة أخرى.

2. **تأكد من وجود "Bearer " قبل الـ Token** - بدونها ستحصل على 401.

3. **الـ Token حساس لحالة الأحرف** - لا تغير أي حرف فيه.

4. **Endpoints التي لا تحتاج Token:**
   - `POST /api/Auth/register` - [AllowAnonymous]
   - `POST /api/Auth/login` - [AllowAnonymous]
   - `POST /api/Auth/google-login` - [AllowAnonymous]

5. **جميع الـ Endpoints الأخرى تحتاج Token:**
   - Business, Customer, Order, Ticket, etc.

## استكشاف الأخطاء

### إذا حصلت على 401:
1. ✅ تأكد من نسخ الـ Token كاملاً
2. ✅ تأكد من إضافة `Bearer ` قبل الـ Token
3. ✅ تأكد من أن الـ Token لم ينتهي (صالح 3 أيام)
4. ✅ تأكد من أن الـ Token من endpoint صحيح (register/login)

### إذا حصلت على 403 Forbidden:
- هذا يعني أن الـ Token صحيح لكن ليس لديك الصلاحيات المطلوبة
- بعض الـ endpoints تحتاج roles معينة (Admin, Owner, etc.)

## مثال كامل

```bash
# 1. تسجيل الدخول
curl -X POST http://localhost:5157/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}'

# 2. استخدم الـ Token في request آخر
curl -X GET http://localhost:5157/api/Business \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```





