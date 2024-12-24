CREATE TABLE [dbo].[UserAccounts] (
    [ID]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Account]    NVARCHAR (50)    NOT NULL,
    [Password]   NVARCHAR (50)    NOT NULL,
    [Name]       NVARCHAR (50)    COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Email]      NVARCHAR (50)    NULL,
    [UserRoleID] UNIQUEIDENTIFIER NULL,
    [IsEnabled]  BIT              DEFAULT (CONVERT([bit],(1))) NOT NULL,
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserRoleId_UserRole] FOREIGN KEY ([UserRoleID]) REFERENCES [dbo].[UserRole] ([ID])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'識別碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts', @level2type = N'COLUMN', @level2name = N'Account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts', @level2type = N'COLUMN', @level2name = N'Password';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者角色ID (UserRole)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts', @level2type = N'COLUMN', @level2name = N'UserRoleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否啟用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts', @level2type = N'COLUMN', @level2name = N'IsEnabled';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UserAccounts';

