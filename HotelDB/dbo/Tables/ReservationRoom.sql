CREATE TABLE [dbo].[ReservationRoom] (
    [ReservationsId] UNIQUEIDENTIFIER NOT NULL,
    [RoomsId]        UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ReservationRoom] PRIMARY KEY CLUSTERED ([ReservationsId] ASC, [RoomsId] ASC),
    CONSTRAINT [FK_ReservationRoom_Reservations_ReservationsId] FOREIGN KEY ([ReservationsId]) REFERENCES [dbo].[Reservations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReservationRoom_Rooms_RoomsId] FOREIGN KEY ([RoomsId]) REFERENCES [dbo].[Rooms] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ReservationRoom_RoomsId]
    ON [dbo].[ReservationRoom]([RoomsId] ASC);

