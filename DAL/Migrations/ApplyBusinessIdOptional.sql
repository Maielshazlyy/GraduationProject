-- Migration: Make BusinessId Optional in AspNetUsers table
-- Run this SQL script directly on your database: DigitalEmployeeDB

USE DigitalEmployeeDB;
GO

-- Make BusinessId nullable in AspNetUsers table
ALTER TABLE AspNetUsers
ALTER COLUMN BusinessId NVARCHAR(450) NULL;
GO

-- Verify the change
SELECT 
    COLUMN_NAME,
    IS_NULLABLE,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AspNetUsers' 
  AND COLUMN_NAME = 'BusinessId';
GO

