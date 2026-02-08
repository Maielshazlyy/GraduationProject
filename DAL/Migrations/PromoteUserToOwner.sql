-- ترقية المستخدم إلى Owner مباشرة في قاعدة البيانات
-- استبدل 'mai@gmail.com' بالإيميل الخاص بك

USE DigitalEmployeeDB;
GO

UPDATE AspNetUsers
SET Role = 'Owner'
WHERE Email = 'mai@gmail.com';
GO

-- التحقق من التحديث
SELECT Id, Email, Role, BusinessId
FROM AspNetUsers
WHERE Email = 'mai@gmail.com';
GO


