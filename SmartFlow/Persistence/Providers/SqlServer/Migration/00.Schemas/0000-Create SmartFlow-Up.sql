IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'SmartFlow')
EXEC sys.sp_executesql N'CREATE SCHEMA [SmartFlow]'