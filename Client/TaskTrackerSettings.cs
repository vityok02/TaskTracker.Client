﻿using System.ComponentModel.DataAnnotations;

namespace Client;

public sealed class TaskTrackerSettings
{
    public const string ConfigurationSection = "TaskTrackerApi";

    [Required, Url]
    public string BaseAddress { get; set; } = string.Empty;
}
