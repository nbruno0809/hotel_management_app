CREATE TABLE [dbo].[RoomTypes] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (MAX)   NOT NULL,
    [Description]   NVARCHAR (MAX)   NOT NULL,
    [PricePerNight] FLOAT (53)       NOT NULL,
    [NumberOfBeds]  INT              NOT NULL,
    [ImgPath]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_RoomTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

