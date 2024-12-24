CREATE TABLE [dbo].[SystemParameter] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Value1]      VARCHAR (50)  NOT NULL,
    [Value2]      VARCHAR (50)  NULL,
    [Value3]      VARCHAR (50)  NULL,
    [Description] VARCHAR (500) NULL,
    CONSTRAINT [PK_SystemParameter] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'參數1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter', @level2type = N'COLUMN', @level2name = N'Value1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'參數2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter', @level2type = N'COLUMN', @level2name = N'Value2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'參數3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter', @level2type = N'COLUMN', @level2name = N'Value3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統參數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemParameter';

