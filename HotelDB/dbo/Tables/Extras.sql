CREATE TABLE [dbo].[Extras] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (MAX)   NOT NULL,
    [Description] NVARCHAR (MAX)   NOT NULL,
    [Price]       FLOAT (53)       NOT NULL,
    CONSTRAINT [PK_Extras] PRIMARY KEY CLUSTERED ([Id] ASC)
);

