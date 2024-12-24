CREATE TABLE [dbo].[SystemCatalogItems] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [MenuName]     NVARCHAR (30)  NOT NULL,
    [Description]  NVARCHAR (100) DEFAULT (N'') NULL,
    [DisplayOrder] INT            NOT NULL,
    [Url]          NVARCHAR (255) DEFAULT (N'') NULL,
    [Icon]         NVARCHAR (50)  DEFAULT (N'') NULL,
    [ParentId]     INT            NULL,
    [IsGroup]      BIT            DEFAULT (CONVERT([bit],(0))) NULL,
    [ImportAt]     NVARCHAR (255) NULL,
    CONSTRAINT [PK_SystemCatalogItems] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SystemCatalogItems_SystemCatalogItems_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[SystemCatalogItems] ([ID])
);




GO
CREATE NONCLUSTERED INDEX [IX_SystemCatalogItems_ParentId]
    ON [dbo].[SystemCatalogItems]([ParentId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目錄識別碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目錄名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'MenuName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示順序 (透過Id抓取，判斷在第幾層位置)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'DisplayOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'網址..', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'Url';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Icon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'Icon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父層ID (目前設為 Id)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統目錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemCatalogItems';

