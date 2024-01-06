IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'FlowCiao')
EXEC sys.sp_executesql N'CREATE SCHEMA [FlowCiao]'