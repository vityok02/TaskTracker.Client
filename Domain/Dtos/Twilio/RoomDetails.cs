﻿namespace Domain.Dtos.Twilio;

public class RoomDetails
{
    public string? Id { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public int ParticipantCount { get; set; }

    public int MaxParticipants { get; set; }
}
