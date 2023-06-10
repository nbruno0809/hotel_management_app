CREATE TABLE [dbo].[ExtraReservation] (
    [ExtrasId]       UNIQUEIDENTIFIER NOT NULL,
    [ReservationsId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ExtraReservation] PRIMARY KEY CLUSTERED ([ExtrasId] ASC, [ReservationsId] ASC),
    CONSTRAINT [FK_ExtraReservation_Extras_ExtrasId] FOREIGN KEY ([ExtrasId]) REFERENCES [dbo].[Extras] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExtraReservation_Reservations_ReservationsId] FOREIGN KEY ([ReservationsId]) REFERENCES [dbo].[Reservations] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ExtraReservation_ReservationsId]
    ON [dbo].[ExtraReservation]([ReservationsId] ASC);

