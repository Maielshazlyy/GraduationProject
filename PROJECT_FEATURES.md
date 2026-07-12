# 📋 دليل شامل لجميع الميزات المطبقة في المشروع

## 🎯 نظرة عامة
هذا المستند يحتوي على قائمة شاملة بجميع الميزات (Features) المطبقة في المشروع، مقسمة حسب نوع المستخدم:
- **Customer** (العميل)
- **Human Employee / Agent** (الموظف البشري / الوكيل)
- **Business Owner** (صاحب العمل)

---

## 👤 1. ميزات العميل (Customer Features)

### 1.1 التواصل مع الذكاء الاصطناعي

#### 1.1.1 الدردشة النصية (Text Chat)
- **API Endpoint**: `POST /api/CustomerChat/message`
- **الوصف**: إرسال رسائل نصية للذكاء الاصطناعي عبر قناة WebChat
- **الميزات**:
  - إرسال رسائل نصية
  - استقبال ردود فورية من الذكاء الاصطناعي
  - دعم اللهجة المصرية والعربية الفصحى
  - حفظ تاريخ المحادثة
  - تحليل النية (Intent Detection) تلقائياً

#### 1.1.2 المكالمات الصوتية (Voice Calls)
- **API Endpoints**:
  - `POST /api/CustomerVoice/session/initialize` - بدء جلسة مكالمة صوتية
  - `POST /api/CustomerVoice/message` - إرسال رسالة صوتية
  - `POST /api/CustomerVoice/interaction/{interactionId}/interrupt` - إيقاف المكالمة
  - `POST /api/CustomerVoice/feedback` - إرسال تقييم بعد المكالمة
  - `GET /api/CustomerVoice/settings/{businessId}` - الحصول على إعدادات الصوت
- **الميزات**:
  - بدء جلسة مكالمة صوتية مع CallSessionId
  - إرسال رسائل صوتية (Audio Data)
  - تحويل الكلام إلى نص (Speech-to-Text) - Placeholder
  - تحويل النص إلى كلام (Text-to-Speech) - Placeholder
  - معالجة انقطاع المكالمة (Call Interruption)
  - إرسال تقييم بعد انتهاء المكالمة
  - إعدادات الصوت (Voice Settings) لكل عمل

#### 1.1.3 معرفة القدرات المتاحة
- **API Endpoint**: `GET /api/CustomerChat/capabilities/{businessId}`
- **الوصف**: معرفة ما إذا كان العمل يدعم Chat أو Voice أو كليهما

### 1.2 إدارة الطلبات (Order Management)

#### 1.2.1 إنشاء طلب من خلال الدردشة/الصوت
- **الوصف**: إنشاء طلب من خلال المحادثة مع الذكاء الاصطناعي
- **الميزات**:
  - استخراج المنتجات من الرسالة تلقائياً
  - حساب السعر الإجمالي
  - إنشاء سلة تسوق (Cart)
  - حفظ الطلب في قاعدة البيانات
  - إرجاع ملخص الطلب مع التوصيات
  - تسجيل في Audit Log

#### 1.2.2 الحصول على توصيات المنتجات
- **API Endpoint**: `POST /api/CustomerChat/recommendations`
- **الوصف**: الحصول على توصيات منتجات بناءً على المنتج الرئيسي المطلوب
- **الميزات**:
  - توصيات تلقائية (مثلاً: برجر → بطاطس + مشروب)
  - عرض الأسباب للتوصية
  - عرض الأسعار
  - متاح للصفحة العامة (Direct Ordering Page)

#### 1.2.3 الاستفسار عن حالة الطلب
- **الوصف**: السؤال عن حالة طلب معين
- **الميزات**:
  - البحث عن الطلب برقم الطلب
  - عرض حالة الطلب (Pending, InProgress, Delivered, etc.)
  - عرض السعر الإجمالي

#### 1.2.4 تعديل الطلب
- **الوصف**: طلب تعديل طلب موجود (Placeholder - TODO)

#### 1.2.5 إلغاء الطلب
- **الوصف**: إلغاء طلب موجود
- **الميزات**:
  - البحث عن الطلب برقم الطلب
  - تحديث حالة الطلب إلى Cancelled
  - تسجيل في Audit Log

### 1.3 إدارة التذاكر والشكاوى (Ticket Management)

#### 1.3.1 إنشاء تذكرة شكوى
- **الوصف**: إنشاء تذكرة شكوى من خلال الدردشة/الصوت
- **الميزات**:
  - إنشاء تذكرة تلقائياً عند اكتشاف شكوى
  - ربط التذكرة بالتفاعل (Interaction)
  - ربط التذكرة بالطلب (Order) إن وجد
  - تحديد نوع التذكرة (LateDelivery, WrongOrder, MissingItem, PaymentIssue, QualityIssue)
  - تحديد الأولوية (Priority Level)

#### 1.3.2 طلب موظف بشري (Human Escalation)
- **الوصف**: طلب التحدث مع موظف بشري بدلاً من الذكاء الاصطناعي
- **الميزات**:
  - إنشاء تذكرة نوعها HumanEscalation
  - تحديث حالة التفاعل إلى "Escalated"
  - إضافة سبب التحويل (Escalation Reason)
  - إضافة مستوى الثقة (Confidence Score)
  - تسجيل في Audit Log

### 1.4 الاستفسارات العامة

#### 1.4.1 الاستفسار عن المنتجات
- **الوصف**: السؤال عن المنتجات المتاحة في المنيو
- **الميزات**:
  - عرض قائمة بالمنتجات المتاحة
  - عرض الأسعار والأوصاف
  - عرض أول 10 منتجات

#### 1.4.2 الأسئلة العامة
- **الوصف**: الإجابة على الأسئلة العامة
- **الميزات**:
  - استخدام Knowledge Base
  - إجابة عامة عن المنيو، حالة الطلب، والتذاكر

### 1.5 التقييمات والملاحظات (Feedback)

#### 1.5.1 إرسال تقييم بعد المكالمة الصوتية
- **API Endpoint**: `POST /api/CustomerVoice/feedback`
- **الميزات**:
  - إرسال تقييم من 1 إلى 5
  - ربط التقييم بالتفاعل (Interaction)
  - حفظ التقييم في قاعدة البيانات
  - تسجيل في Audit Log

---

## 👨‍💼 2. ميزات الموظف البشري / الوكيل (Human Employee / Agent Features)

### 2.1 إدارة التذاكر (Ticket Management)

#### 2.1.1 عرض قائمة التذاكر المتاحة
- **API Endpoint**: `GET /api/Ticket/queue`
- **الوصف**: عرض جميع التذاكر المرفوعة للبشر (HumanEscalation) غير المخصصة بعد
- **الميزات**:
  - عرض التذاكر المرفوعة فقط
  - عرض التذاكر غير المخصصة (Unassigned)
  - تصفية حسب BusinessId من Token
  - عرض الأولوية (Priority)
  - عرض سبب التحويل (Escalation Reason)

#### 2.1.2 عرض جميع التذاكر
- **API Endpoints**:
  - `GET /api/Ticket` - جميع التذاكر
  - `GET /api/Ticket/business/{businessId}` - تذاكر عمل معين
  - `GET /api/Ticket/{id}` - تذكرة معينة
- **الميزات**:
  - عرض جميع التذاكر
  - تصفية حسب Business
  - عرض تفاصيل التذكرة

#### 2.1.3 الانضمام إلى تذكرة (Join Ticket)
- **API Endpoint**: `POST /api/Ticket/{id}/assign`
- **الوصف**: عندما ينضم Agent إلى تذكرة، يتم تخصيصها له وتختفي من قوائم الـ Agents الآخرين
- **الميزات**:
  - تخصيص التذكرة للـ Agent
  - تحديث حالة التذكرة إلى "InProgress"
  - إخفاء التذكرة من قوائم الـ Agents الآخرين
  - تسجيل في Audit Log

#### 2.1.4 إغلاق التذكرة
- **API Endpoint**: `POST /api/Ticket/{id}/close`
- **الميزات**:
  - إغلاق التذكرة
  - تحديث حالة التذكرة إلى "Closed"
  - تحديث حالة التفاعل (Interaction) المرتبط إلى "Closed"
  - إضافة ملاحظات الإغلاق
  - تسجيل في Audit Log

#### 2.1.5 إنشاء تذكرة جديدة
- **API Endpoint**: `POST /api/Ticket`
- **الميزات**:
  - إنشاء تذكرة يدوياً
  - تحديد نوع التذكرة
  - تحديد الأولوية
  - ربط التذكرة بعملاء وطلبات

#### 2.1.6 تحديث التذكرة
- **API Endpoint**: `PUT /api/Ticket/{id}`
- **الميزات**:
  - تحديث معلومات التذكرة
  - تحديث الحالة
  - تحديث الأولوية

### 2.2 إدارة الرسائل (Message Management)

#### 2.2.1 عرض الرسائل
- **API Endpoints**:
  - `GET /api/Message` - جميع الرسائل
  - `GET /api/Message/interaction/{interactionId}` - رسائل تفاعل معين
  - `GET /api/Message/{id}` - رسالة معينة
- **الميزات**:
  - عرض جميع الرسائل في التفاعل
  - عرض تاريخ المحادثة الكامل
  - عرض نوع المرسل (Customer, AI, Agent)
  - عرض تحليل المشاعر (Sentiment Analysis)

#### 2.2.2 إرسال رسالة للعميل
- **API Endpoint**: `POST /api/Message`
- **الميزات**:
  - إرسال رسالة للعميل في التفاعل
  - حفظ الرسالة في قاعدة البيانات
  - تسجيل في Audit Log
  - إمكانية إضافة تحليل المشاعر

### 2.3 إدارة التفاعلات (Interaction Management)

#### 2.3.1 عرض التفاعلات
- **API Endpoints**:
  - `GET /api/Interaction` - جميع التفاعلات
  - `GET /api/Interaction/business/{businessId}` - تفاعلات عمل معين
  - `GET /api/Interaction/customer/{customerId}` - تفاعلات عميل معين
  - `GET /api/Interaction/user/{userId}` - تفاعلات مستخدم معين
  - `GET /api/Interaction/{id}` - تفاعل معين
- **الميزات**:
  - عرض جميع التفاعلات
  - تصفية حسب Business, Customer, User
  - عرض حالة التفاعل (Open, InProgress, Escalated, Closed)
  - عرض نوع التفاعل (Informational, Order, Ticket, Mixed)
  - عرض القناة (WebChat, Voice)

#### 2.3.2 بدء تفاعل جديد
- **API Endpoint**: `POST /api/Interaction/start`
- **الميزات**:
  - بدء تفاعل جديد يدوياً
  - تحديد القناة
  - ربط بالعميل والعمل

#### 2.3.3 إنهاء تفاعل
- **API Endpoint**: `POST /api/Interaction/{id}/end`
- **الميزات**:
  - إنهاء تفاعل
  - تحديث حالة التفاعل
  - حفظ وقت الانتهاء

### 2.4 إدارة العملاء (Customer Management)

#### 2.4.1 عرض العملاء
- **API Endpoints**:
  - `GET /api/Customer` - جميع العملاء
  - `GET /api/Customer/business/{businessId}` - عملاء عمل معين
  - `GET /api/Customer/{id}` - عميل معين
  - `GET /api/Customer/email/{email}` - عميل بالبريد الإلكتروني
- **الميزات**:
  - عرض جميع العملاء
  - تصفية حسب Business
  - عرض تفاصيل العميل
  - عرض تاريخ العميل

### 2.5 إدارة الطلبات (Order Management)

#### 2.5.1 عرض الطلبات
- **API Endpoints**:
  - `GET /api/Order` - جميع الطلبات
  - `GET /api/Order/business/{businessId}` - طلبات عمل معين
  - `GET /api/Order/customer/{customerId}` - طلبات عميل معين
  - `GET /api/Order/{id}` - طلب معين
- **الميزات**:
  - عرض جميع الطلبات
  - تصفية حسب Business, Customer
  - عرض تفاصيل الطلب
  - عرض حالة الطلب

#### 2.5.2 تحديث حالة الطلب
- **API Endpoint**: `PUT /api/Order/{id}/status`
- **الميزات**:
  - تحديث حالة الطلب
  - تسجيل في Audit Log

### 2.6 إدارة التقييمات (Feedback Management)

#### 2.6.1 عرض التقييمات
- **API Endpoints**:
  - `GET /api/Feedback` - جميع التقييمات
  - `GET /api/Feedback/business/{businessId}` - تقييمات عمل معين
  - `GET /api/Feedback/customer/{customerId}` - تقييمات عميل معين
  - `GET /api/Feedback/ticket/{ticketId}` - تقييمات تذكرة معينة
  - `GET /api/Feedback/{id}` - تقييم معين
- **الميزات**:
  - عرض جميع التقييمات
  - تصفية حسب Business, Customer, Ticket
  - عرض التقييم (Rating) من 1 إلى 5
  - عرض التعليقات

### 2.7 إدارة الإشعارات (Notification Management)

#### 2.7.1 عرض الإشعارات
- **API Endpoints**:
  - `GET /api/Notification` - جميع الإشعارات
  - `GET /api/Notification/business/{businessId}` - إشعارات عمل معين
  - `GET /api/Notification/user/{userId}` - إشعارات مستخدم معين
  - `GET /api/Notification/{id}` - إشعار معين
- **الميزات**:
  - عرض جميع الإشعارات
  - تصفية حسب Business, User
  - عرض حالة القراءة (Read/Unread)

#### 2.7.2 تحديد الإشعار كمقروء
- **API Endpoint**: `PUT /api/Notification/{id}/read`
- **الميزات**:
  - تحديث حالة الإشعار إلى "Read"
  - حفظ وقت القراءة

### 2.8 إدارة التقارير (Report Management)

#### 2.8.1 عرض التقارير
- **API Endpoints**:
  - `GET /api/Report` - جميع التقارير
  - `GET /api/Report/business/{businessId}` - تقارير عمل معين
  - `GET /api/Report/{id}` - تقرير معين
- **الميزات**:
  - عرض جميع التقارير
  - تصفية حسب Business
  - عرض تفاصيل التقرير
  - تحميل ملف التقرير

### 2.9 إدارة تحليل المشاعر (Sentiment Analysis)

#### 2.9.1 عرض تحليل المشاعر
- **API Endpoints**:
  - `GET /api/Sentiment` - جميع تحليلات المشاعر
  - `GET /api/Sentiment/business/{businessId}` - تحليلات عمل معين
  - `GET /api/Sentiment/message/{messageId}` - تحليل رسالة معينة
  - `GET /api/Sentiment/{id}` - تحليل معين
- **الميزات**:
  - عرض جميع تحليلات المشاعر
  - تصفية حسب Business, Message
  - عرض التصنيف (Positive, Negative, Neutral)
  - عرض النتيجة (Score)

#### 2.9.2 تحليل مشاعر رسالة
- **API Endpoint**: `POST /api/Sentiment/analyze`
- **الميزات**:
  - تحليل مشاعر رسالة معينة
  - حفظ التحليل في قاعدة البيانات

### 2.10 إدارة المنيو (Menu Management) - للقراءة فقط

#### 2.10.1 عرض عناصر المنيو
- **API Endpoints**:
  - `GET /api/MenuItem` - جميع عناصر المنيو
  - `GET /api/MenuItem/business/{businessId}` - عناصر عمل معين
  - `GET /api/MenuItem/{id}` - عنصر معين
- **الميزات**:
  - عرض جميع عناصر المنيو
  - تصفية حسب Business
  - عرض الأسعار والأوصاف

#### 2.10.2 عرض فئات المنيو
- **API Endpoints**:
  - `GET /api/MenuCategory` - جميع فئات المنيو
  - `GET /api/MenuCategory/business/{businessId}` - فئات عمل معين
  - `GET /api/MenuCategory/{id}` - فئة معينة
- **الميزات**:
  - عرض جميع فئات المنيو
  - تصفية حسب Business

### 2.11 إدارة قاعدة المعرفة (Knowledge Base) - للقراءة فقط

#### 2.11.1 عرض قاعدة المعرفة
- **API Endpoints**:
  - `GET /api/KnowledgeBase` - جميع عناصر قاعدة المعرفة
  - `GET /api/KnowledgeBase/business/{businessId}` - عناصر عمل معين
  - `GET /api/KnowledgeBase/{id}` - عنصر معين
- **الميزات**:
  - عرض جميع عناصر قاعدة المعرفة
  - تصفية حسب Business
  - عرض الأسئلة والأجوبة

---

## 🏢 3. ميزات صاحب العمل (Business Owner Features)

### 3.1 لوحة التحكم (Dashboard)

#### 3.1.1 ملخص لوحة التحكم
- **API Endpoint**: `GET /api/Dashboard/summary`
- **الميزات**:
  - معلومات العمل الأساسية
  - إحصائيات المنيو (عدد العناصر، الفئات)
  - إحصائيات قاعدة المعرفة
  - حالة الإعداد (Setup Status)
  - إحصائيات Audit Log (إجمالي، آخر 24 ساعة، آخر تاريخ)
  - خطوات الإعداد المكتملة والمعلقة

#### 3.1.2 التحليلات (Analytics)
- **API Endpoint**: `GET /api/Dashboard/analytics`
- **الميزات**:
  - إجمالي الطلبات والإيرادات
  - متوسط قيمة الطلب
  - عدد العملاء الجدد
  - إحصائيات التذاكر (مفتوحة، مغلقة، قيد المعالجة)
  - متوسط وقت حل التذكرة
  - إحصائيات التقييمات (متوسط، إيجابية، سلبية)
  - تحليل المشاعر (إيجابي، سلبي، محايد)
  - إحصائيات التفاعلات

#### 3.1.3 لوحة التحكم الكاملة
- **API Endpoint**: `GET /api/Dashboard/full`
- **الميزات**:
  - دمج الملخص والتحليلات في استجابة واحدة

### 3.2 إدارة Audit Log

#### 3.2.1 عرض Audit Logs الأخيرة
- **API Endpoint**: `GET /api/Dashboard/audit-logs/recent?count=20`
- **الميزات**:
  - عرض آخر 20 Audit Log (قابل للتعديل)
  - ترتيب حسب التاريخ (الأحدث أولاً)
  - عرض الإجراء، الكيان، المستخدم، التاريخ

#### 3.2.2 إحصائيات Audit Log
- **API Endpoint**: `GET /api/Dashboard/audit-logs/statistics`
- **الميزات**:
  - إجمالي الإجراءات
  - الإجراءات في آخر 24 ساعة، 7 أيام، 30 يوم
  - الإجراءات حسب نوع الكيان (Order, Ticket, Interaction, etc.)
  - الإجراءات حسب النوع (Create, Update, Delete, etc.)
  - أكثر المستخدمين نشاطاً
  - الإجراءات الحرجة الأخيرة (Delete, Escalate, etc.)

#### 3.2.3 Audit Logs لعميل معين
- **API Endpoint**: `GET /api/Dashboard/audit-logs/customer/{customerId}`
- **الميزات**:
  - عرض جميع Audit Logs المتعلقة بعميل معين
  - يشمل: Orders, Tickets, Interactions, Feedbacks
  - ترتيب حسب التاريخ (الأحدث أولاً)

### 3.3 إدارة العمل (Business Management)

#### 3.3.1 إنشاء عمل جديد
- **API Endpoint**: `POST /api/Business`
- **الميزات**:
  - إنشاء عمل جديد
  - ربط المستخدم بالعمل
  - ترقية المستخدم إلى دور Owner
  - إضافة معلومات العمل (الاسم، النوع، العنوان، الهاتف)
  - إضافة معلومات الاتصال (البريد، الموقع، Facebook, Instagram)
  - إضافة الموقع (المدينة، الدولة، الإحداثيات)
  - إضافة معلومات المطعم (الوصف، نوع المطبخ، نطاق السعر)
  - إضافة الصور (الشعار، صورة الغلاف)
  - إضافة الميزات (التوصيل، الطلبات الخارجية، موقف السيارات، WiFi، الجلوس الخارجي، الحجوزات)
  - إضافة طرق الدفع
  - إضافة ساعات العمل
  - تسجيل في Audit Log

#### 3.3.2 Onboarding للعمل
- **API Endpoint**: `POST /api/Business/onboard`
- **الميزات**:
  - عملية Onboarding كاملة للعمل
  - إنشاء العمل مع جميع المعلومات
  - ربط المستخدم بالعمل (إن كان مسجل دخول)

#### 3.3.3 عرض العمل
- **API Endpoints**:
  - `GET /api/Business` - جميع الأعمال
  - `GET /api/Business/{id}` - عمل معين
- **الميزات**:
  - عرض جميع الأعمال
  - عرض تفاصيل العمل الكاملة

#### 3.3.4 تحديث العمل
- **API Endpoint**: `PUT /api/Business/{id}`
- **الميزات**:
  - تحديث معلومات العمل
  - تحديث جميع الحقول
  - تسجيل في Audit Log

#### 3.3.5 حذف العمل
- **API Endpoint**: `DELETE /api/Business/{id}`
- **الميزات**:
  - حذف العمل (Admin Only)
  - تسجيل في Audit Log

### 3.4 إدارة الموظفين البشريين (Human Employee Management)

#### 3.4.1 إضافة موظف بشري جديد
- **API Endpoint**: `POST /api/User/agents`
- **الميزات**:
  - إنشاء حساب جديد للموظف البشري
  - تعيين دور Agent تلقائياً
  - ربط الموظف بالعمل (BusinessId من Token)
  - تسجيل في Audit Log

#### 3.4.2 عرض الموظفين
- **API Endpoints**:
  - `GET /api/User/business/{businessId}` - موظفو عمل معين
  - `GET /api/User/{id}` - موظف معين
- **الميزات**:
  - عرض جميع الموظفين في العمل
  - عرض تفاصيل الموظف

### 3.5 إدارة المنيو (Menu Management)

#### 3.5.1 إدارة عناصر المنيو
- **API Endpoints**:
  - `GET /api/MenuItem` - عرض جميع العناصر
  - `GET /api/MenuItem/business/{businessId}` - عناصر عمل معين
  - `GET /api/MenuItem/{id}` - عنصر معين
  - `POST /api/MenuItem` - إنشاء عنصر جديد
  - `PUT /api/MenuItem/{id}` - تحديث عنصر
  - `DELETE /api/MenuItem/{id}` - حذف عنصر
- **الميزات**:
  - إنشاء عناصر المنيو (الاسم، الوصف، السعر، الصورة)
  - تحديث عناصر المنيو
  - حذف عناصر المنيو
  - تحديد توفر العنصر (IsAvailable)
  - ربط العنصر بفئة
  - تسجيل في Audit Log

#### 3.5.2 إدارة فئات المنيو
- **API Endpoints**:
  - `GET /api/MenuCategory` - عرض جميع الفئات
  - `GET /api/MenuCategory/business/{businessId}` - فئات عمل معين
  - `GET /api/MenuCategory/{id}` - فئة معينة
  - `POST /api/MenuCategory` - إنشاء فئة جديدة
  - `PUT /api/MenuCategory/{id}` - تحديث فئة
  - `DELETE /api/MenuCategory/{id}` - حذف فئة
- **الميزات**:
  - إنشاء فئات المنيو (الاسم، الوصف، الصورة)
  - تحديث فئات المنيو
  - حذف فئات المنيو
  - ترتيب الفئات
  - تسجيل في Audit Log

### 3.6 إدارة قاعدة المعرفة (Knowledge Base Management)

#### 3.6.1 إدارة قاعدة المعرفة
- **API Endpoints**:
  - `GET /api/KnowledgeBase` - عرض جميع العناصر
  - `GET /api/KnowledgeBase/business/{businessId}` - عناصر عمل معين
  - `GET /api/KnowledgeBase/{id}` - عنصر معين
  - `POST /api/KnowledgeBase` - إنشاء عنصر جديد
  - `PUT /api/KnowledgeBase/{id}` - تحديث عنصر
  - `DELETE /api/KnowledgeBase/{id}` - حذف عنصر
- **الميزات**:
  - إنشاء أسئلة وأجوبة
  - تحديث الأسئلة والأجوبة
  - حذف الأسئلة والأجوبة
  - استخدامها في إجابات الذكاء الاصطناعي
  - تسجيل في Audit Log

### 3.7 إدارة الإعدادات (Settings Management)

#### 3.7.1 إدارة إعدادات العمل
- **API Endpoints**:
  - `GET /api/Setting/business/{businessId}` - إعدادات عمل معين
  - `PUT /api/Setting/business/{businessId}` - تحديث الإعدادات
- **الميزات**:
  - تفعيل/تعطيل الذكاء الاصطناعي (ChatbotEnabled)
  - رسالة الترحيب (ChatbotWelcomeMessage)
  - صوت الوكيل (AgentVoice)
  - إعدادات أخرى
  - تسجيل في Audit Log

### 3.8 إدارة التذاكر (Ticket Management)

#### 3.8.1 عرض جميع التذاكر
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Ticket` - جميع التذاكر
  - `GET /api/Ticket/business/{businessId}` - تذاكر عمل معين
  - `GET /api/Ticket/{id}` - تذكرة معينة
- **الميزات**:
  - عرض جميع التذاكر
  - تصفية حسب Business
  - عرض تفاصيل التذكرة

#### 3.8.2 تخصيص تذكرة لموظف
- **API Endpoint**: `POST /api/Ticket/{id}/assign`
- **الميزات**:
  - تخصيص تذكرة لموظف معين
  - تحديث حالة التذكرة
  - تسجيل في Audit Log

#### 3.8.3 إغلاق التذكرة
- **API Endpoint**: `POST /api/Ticket/{id}/close`
- **الميزات**: (نفس ميزات Agent)

### 3.9 إدارة الطلبات (Order Management)

#### 3.9.1 عرض جميع الطلبات
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Order` - جميع الطلبات
  - `GET /api/Order/business/{businessId}` - طلبات عمل معين
  - `GET /api/Order/customer/{customerId}` - طلبات عميل معين
  - `GET /api/Order/{id}` - طلب معين
- **الميزات**:
  - عرض جميع الطلبات
  - تصفية حسب Business, Customer
  - عرض تفاصيل الطلب

#### 3.9.2 تحديث حالة الطلب
- **API Endpoint**: `PUT /api/Order/{id}/status`
- **الميزات**:
  - تحديث حالة الطلب
  - تسجيل في Audit Log

#### 3.9.3 حذف الطلب
- **API Endpoint**: `DELETE /api/Order/{id}`
- **الميزات**:
  - حذف الطلب (Admin Only)
  - تسجيل في Audit Log

### 3.10 إدارة العملاء (Customer Management)

#### 3.10.1 عرض العملاء
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Customer` - جميع العملاء
  - `GET /api/Customer/business/{businessId}` - عملاء عمل معين
  - `GET /api/Customer/{id}` - عميل معين
- **الميزات**:
  - عرض جميع العملاء
  - تصفية حسب Business
  - عرض تفاصيل العميل

#### 3.10.2 إنشاء عميل جديد
- **API Endpoint**: `POST /api/Customer`
- **الميزات**:
  - إنشاء عميل جديد
  - ربط العميل بالعمل

#### 3.10.3 تحديث العميل
- **API Endpoint**: `PUT /api/Customer/{id}`
- **الميزات**:
  - تحديث معلومات العميل

#### 3.10.4 حذف العميل
- **API Endpoint**: `DELETE /api/Customer/{id}`
- **الميزات**:
  - حذف العميل (Admin Only)

### 3.11 إدارة التقييمات (Feedback Management)

#### 3.11.1 عرض التقييمات
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Feedback` - جميع التقييمات
  - `GET /api/Feedback/business/{businessId}` - تقييمات عمل معين
  - `GET /api/Feedback/customer/{customerId}` - تقييمات عميل معين
  - `GET /api/Feedback/ticket/{ticketId}` - تقييمات تذكرة معينة
  - `GET /api/Feedback/{id}` - تقييم معين
- **الميزات**:
  - عرض جميع التقييمات
  - تصفية حسب Business, Customer, Ticket
  - عرض التقييم والتعليقات

#### 3.11.2 حذف التقييم
- **API Endpoint**: `DELETE /api/Feedback/{id}`
- **الميزات**:
  - حذف التقييم (Admin Only)

### 3.12 إدارة الإشعارات (Notification Management)

#### 3.12.1 عرض الإشعارات
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Notification` - جميع الإشعارات
  - `GET /api/Notification/business/{businessId}` - إشعارات عمل معين
  - `GET /api/Notification/user/{userId}` - إشعارات مستخدم معين
  - `GET /api/Notification/{id}` - إشعار معين
- **الميزات**:
  - عرض جميع الإشعارات
  - تصفية حسب Business, User

#### 3.12.2 إنشاء إشعار
- **API Endpoint**: `POST /api/Notification`
- **الميزات**:
  - إنشاء إشعار جديد
  - إرسال إشعار لمستخدم معين
  - ربط الإشعار بالعمل

#### 3.12.3 حذف الإشعار
- **API Endpoint**: `DELETE /api/Notification/{id}`
- **الميزات**:
  - حذف الإشعار

### 3.13 إدارة التقارير (Report Management)

#### 3.13.1 عرض التقارير
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Report` - جميع التقارير
  - `GET /api/Report/business/{businessId}` - تقارير عمل معين
  - `GET /api/Report/{id}` - تقرير معين
- **الميزات**:
  - عرض جميع التقارير
  - تصفية حسب Business
  - تحميل ملف التقرير

#### 3.13.2 إنشاء تقرير
- **API Endpoint**: `POST /api/Report`
- **الميزات**:
  - إنشاء تقرير جديد
  - تحديد نوع التقرير
  - ربط التقرير بالعمل
  - حفظ ملف التقرير

### 3.14 إدارة تحليل المشاعر (Sentiment Analysis)

#### 3.14.1 عرض تحليل المشاعر
- **API Endpoints**: (نفس ميزات Agent)
  - `GET /api/Sentiment` - جميع تحليلات المشاعر
  - `GET /api/Sentiment/business/{businessId}` - تحليلات عمل معين
  - `GET /api/Sentiment/message/{messageId}` - تحليل رسالة معينة
  - `GET /api/Sentiment/{id}` - تحليل معين
- **الميزات**:
  - عرض جميع تحليلات المشاعر
  - تصفية حسب Business, Message

#### 3.14.2 تحليل مشاعر رسالة
- **API Endpoint**: `POST /api/Sentiment/analyze`
- **الميزات**:
  - تحليل مشاعر رسالة معينة
  - حفظ التحليل

### 3.15 إدارة FAQ

#### 3.15.1 إدارة FAQ
- **API Endpoints**:
  - `GET /api/FAQ/business/{businessId}` - FAQs لعمل معين
  - `GET /api/FAQ/{id}` - FAQ معين
  - `POST /api/FAQ` - إنشاء FAQ جديد
  - `PUT /api/FAQ/{id}` - تحديث FAQ
  - `DELETE /api/FAQ/{id}` - حذف FAQ
- **الميزات**:
  - إنشاء أسئلة وأجوبة شائعة
  - تحديث FAQs
  - حذف FAQs
  - استخدامها في إجابات الذكاء الاصطناعي

### 3.16 Chatbot للأسئلة

#### 3.16.1 طرح سؤال على Chatbot
- **API Endpoint**: `POST /api/Chatbot/ask`
- **الميزات**:
  - طرح سؤال على Chatbot
  - الحصول على إجابة بناءً على بيانات العمل
  - استخدام Analytics في الإجابات
  - اقتراحات أسئلة

#### 3.16.2 الحصول على اقتراحات أسئلة
- **API Endpoint**: `GET /api/Chatbot/suggestions`
- **الميزات**:
  - عرض قائمة بأسئلة مقترحة
  - أسئلة عن الأداء، الإيرادات، رضا العملاء، إلخ

### 3.17 إدارة التكاملات (Integration Management)

#### 3.17.1 إدارة التكاملات
- **API Endpoints**:
  - `GET /api/Integration` - جميع التكاملات (Admin Only)
  - `GET /api/Integration/business/{businessId}` - تكاملات عمل معين
  - `GET /api/Integration/{id}` - تكامل معين
  - `POST /api/Integration` - إنشاء تكامل جديد
  - `PUT /api/Integration/{id}` - تحديث تكامل
  - `DELETE /api/Integration/{id}` - حذف تكامل
- **الميزات**:
  - إدارة التكاملات الخارجية
  - ربط التكاملات بالعمل
  - إعدادات التكامل

### 3.18 إدارة الاشتراكات (Subscription Management)

#### 3.18.1 عرض الاشتراكات
- **API Endpoints**:
  - `GET /api/Subscription` - جميع الاشتراكات (Admin Only)
  - `GET /api/Subscription/business/{businessId}` - اشتراك عمل معين
  - `GET /api/Subscription/{id}` - اشتراك معين
- **الميزات**:
  - عرض جميع الاشتراكات
  - تصفية حسب Business
  - عرض تفاصيل الاشتراك

#### 3.18.2 إنشاء اشتراك
- **API Endpoint**: `POST /api/Subscription`
- **الميزات**:
  - إنشاء اشتراك جديد
  - ربط الاشتراك بالعمل
  - تحديد نوع الخطة

### 3.19 إدارة معاملات الدفع (Payment Transaction Management)

#### 3.19.1 عرض معاملات الدفع
- **API Endpoints**:
  - `GET /api/PaymentTransaction` - جميع المعاملات (Admin Only)
  - `GET /api/PaymentTransaction/business/{businessId}` - معاملات عمل معين
  - `GET /api/PaymentTransaction/subscription/{subscriptionId}` - معاملات اشتراك معين
  - `GET /api/PaymentTransaction/{id}` - معاملة معينة
- **الميزات**:
  - عرض جميع معاملات الدفع
  - تصفية حسب Business, Subscription
  - عرض تفاصيل المعاملة

#### 3.19.2 إنشاء معاملة دفع
- **API Endpoint**: `POST /api/PaymentTransaction`
- **الميزات**:
  - إنشاء معاملة دفع جديدة (Admin Only)
  - ربط المعاملة بالاشتراك
  - تسجيل في Audit Log

---

## 🔐 4. ميزات المصادقة والتفويض (Authentication & Authorization)

### 4.1 التسجيل (Registration)
- **API Endpoint**: `POST /api/Auth/register`
- **الميزات**:
  - تسجيل مستخدم جديد
  - إنشاء حساب مع Email و Password
  - الحصول على JWT Token
  - ربط المستخدم بعمل (اختياري)

### 4.2 تسجيل الدخول (Login)
- **API Endpoint**: `POST /api/Auth/login`
- **الميزات**:
  - تسجيل الدخول بالبريد الإلكتروني وكلمة المرور
  - الحصول على JWT Token
  - Token يحتوي على: UserId, Email, Role, BusinessId

### 4.3 تسجيل الدخول بـ Google
- **API Endpoint**: `POST /api/Auth/google-login`
- **الميزات**:
  - تسجيل الدخول باستخدام Google
  - التحقق من IdToken
  - إنشاء حساب جديد إن لم يكن موجوداً
  - الحصول على JWT Token

### 4.4 ترقية المستخدمين (Admin Only)
- **API Endpoints**:
  - `POST /api/Auth/promote-to-owner` - ترقية إلى Owner
  - `POST /api/Auth/promote-to-admin` - ترقية إلى Admin
- **الميزات**:
  - ترقية المستخدمين إلى أدوار أعلى
  - فقط Admin يمكنه الترقية

---

## 📊 5. نظام Audit Log (تسجيل العمليات)

### 5.1 الميزات العامة
- **الوصف**: تسجيل جميع الإجراءات المهمة في النظام
- **الميزات**:
  - تسجيل جميع الإجراءات (Create, Update, Delete)
  - ربط الإجراءات بالكيان (Entity) ورقم الكيان (EntityId)
  - ربط الإجراءات بالمستخدم (User) أو AI
  - حفظ التاريخ والوقت
  - ربط الإجراءات بالعمل (Business)

### 5.2 أنواع الإجراءات المسجلة

#### 5.2.1 إجراءات الطلبات (Order Actions)
- CreateOrderFromChat
- CreateOrderFromVoice
- CancelOrderFromChat
- UpdateOrderStatus
- DeleteOrder

#### 5.2.2 إجراءات التذاكر (Ticket Actions)
- CreateTicket
- AssignTicket
- CloseTicket
- UpdateTicket
- DeleteTicket

#### 5.2.3 إجراءات التفاعلات (Interaction Actions)
- EscalateToHuman_{Intent}
- AgentSendMessage
- StartInteraction
- EndInteraction

#### 5.2.4 إجراءات العمل (Business Actions)
- CreateBusiness
- UpdateBusiness
- DeleteBusiness

#### 5.2.5 إجراءات المنيو (Menu Actions)
- CreateMenuItem
- UpdateMenuItem
- DeleteMenuItem
- CreateMenuCategory
- UpdateMenuCategory
- DeleteMenuCategory

#### 5.2.6 إجراءات قاعدة المعرفة (Knowledge Base Actions)
- CreateKnowledgeBase
- UpdateKnowledgeBase
- DeleteKnowledgeBase

#### 5.2.7 إجراءات الإعدادات (Settings Actions)
- UpdateSettings

#### 5.2.8 إجراءات الدفع (Payment Actions)
- CreatePaymentTransaction

#### 5.2.9 إجراءات التقييمات (Feedback Actions)
- CreateFeedback

#### 5.2.10 إجراءات المستخدمين (User Actions)
- CreateHumanEmployee
- AssignRole_{Role}
- DeleteUser

### 5.3 عرض Audit Logs
- **API Endpoints**:
  - `GET /api/AuditLog` - جميع Audit Logs (Admin Only)
  - `GET /api/AuditLog/business/{businessId}` - Audit Logs لعمل معين
  - `GET /api/Dashboard/audit-logs/recent` - آخر Audit Logs
  - `GET /api/Dashboard/audit-logs/statistics` - إحصائيات Audit Log
  - `GET /api/Dashboard/audit-logs/customer/{customerId}` - Audit Logs لعميل معين

---

## 🤖 6. ميزات الذكاء الاصطناعي (AI Features)

### 6.1 تحليل النية (Intent Detection)
- **الوصف**: تحليل رسالة العميل لتحديد النية
- **الميزات**:
  - اكتشاف النية (CreateOrder, AskAboutOrderStatus, Complaint, RequestHumanAgent, etc.)
  - حساب مستوى الثقة (Confidence Score)
  - تحديد مستوى التعقيد (Complexity Level)
  - تحديد الحاجة للتحويل (RequiresEscalation)
  - تحديد الأولوية (Priority Level)
  - استخراج الكيانات (Entities) من الرسالة
  - اكتشاف اللهجة (Egyptian, Standard Arabic)

### 6.2 منطق التحويل للبشر (Human Escalation Logic)
- **الوصف**: تحديد متى يجب تحويل المحادثة لموظف بشري
- **شروط التحويل**:
  1. طلب صريح من العميل (RequestHumanAgent)
  2. مستوى ثقة منخفض (< 0.5)
  3. مستوى تعقيد عالي (High Complexity)
  4. نية معقدة تتطلب تدخل بشري
- **الميزات**:
  - إنشاء تذكرة HumanEscalation تلقائياً
  - تحديث حالة التفاعل إلى "Escalated"
  - إضافة سبب التحويل
  - تسجيل في Audit Log

### 6.3 توصيات المنتجات (Product Recommendations)
- **الوصف**: اقتراح منتجات إضافية بناءً على المنتج الرئيسي
- **الميزات**:
  - اقتراح منتجات مكملة (مثلاً: برجر → بطاطس + مشروب)
  - عرض الأسباب للتوصية
  - عرض الأسعار
  - متاح في الدردشة والصوت والصفحة العامة

### 6.4 تحليل المشاعر (Sentiment Analysis)
- **الوصف**: تحليل مشاعر رسائل العملاء
- **الميزات**:
  - تصنيف المشاعر (Positive, Negative, Neutral)
  - حساب النتيجة (Score)
  - حفظ التحليل في قاعدة البيانات
  - استخدامه في التحليلات

### 6.5 Chatbot للأسئلة
- **الوصف**: Chatbot للإجابة على أسئلة أصحاب العمل
- **الميزات**:
  - الإجابة على أسئلة عن الأداء
  - استخدام Analytics في الإجابات
  - اقتراحات أسئلة

---

## 📝 7. ملاحظات تقنية مهمة

### 7.1 سياسات التفويض (Authorization Policies)
- **AdminOnly**: فقط Admin
- **OwnerOrAdmin**: Owner أو Admin
- **AgentOrOwnerOrAdmin**: Agent أو Owner أو Admin

### 7.2 الأدوار (Roles)
- **Admin**: مدير النظام
- **Owner**: صاحب العمل
- **Agent**: موظف بشري
- **User**: مستخدم عادي (للعملاء)

### 7.3 القنوات (Channels)
- **WebChat**: الدردشة النصية
- **Voice**: المكالمات الصوتية

### 7.4 حالات الطلبات (Order Status)
- Pending
- InProgress
- Delivered
- Cancelled
- Paid

### 7.5 حالات التذاكر (Ticket Status)
- Open
- InProgress
- Escalated
- Closed

### 7.6 حالات التفاعلات (Interaction Status)
- Open
- InProgress
- Escalated
- Closed
- Interrupted

### 7.7 أنواع التذاكر (Ticket Types)
- LateDelivery
- WrongOrder
- MissingItem
- PaymentIssue
- QualityIssue
- HumanEscalation

---

## 📅 8. تاريخ التحديثات

- **آخر تحديث**: تم إنشاء هذا المستند بناءً على تحليل شامل للمشروع
- **الإصدار**: 1.0
- **التاريخ**: 2024

---

## 📌 9. ملاحظات إضافية

### 9.1 Placeholders (قيد التطوير)
- Speech-to-Text: Placeholder
- Text-to-Speech: Placeholder
- Intent Detection: Implementation بسيطة (Keyword-based)، يحتاج تحسين
- Order Modification: TODO

### 9.2 الميزات المستقبلية المقترحة
- تحسين Intent Detection باستخدام AI Models
- تحسين Speech-to-Text و Text-to-Speech
- إضافة المزيد من التحليلات
- إضافة إشعارات Push
- إضافة تكاملات مع منصات خارجية

---

**تم إنشاء هذا المستند بناءً على تحليل شامل لجميع ملفات المشروع.**

