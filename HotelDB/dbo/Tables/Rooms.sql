CREATE TABLE [dbo].[Rooms] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Number] NVARCHAR (MAX)   NOT NULL,
    [Active] BIT              NOT NULL,
    [TypeId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Rooms_RoomTypes_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[RoomTypes] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Rooms_TypeId]
    ON [dbo].[Rooms]([TypeId] ASC);

