IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'FlowCiao')
BEGIN
    EXEC ('CREATE SCHEMA [FlowCiao]')
END
