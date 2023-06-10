CREATE TABLE [dbo].[Reservations] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [DateOfReservation] DATETIME2 (7)    NULL,
    [From]              DATETIME2 (7)    NOT NULL,
    [To]                DATETIME2 (7)    NOT NULL,
    [Price]             FLOAT (53)       NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reservations_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Reservations_UserId]
    ON [dbo].[Reservations]([UserId] ASC);

