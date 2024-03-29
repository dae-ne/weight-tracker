﻿namespace WeightTracker.Api.Models;

internal sealed class DataFilter
{
    public string UserId { get; set; } = string.Empty;

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public void Deconstruct(
        out string userId,
        out DateOnly? dateFrom,
        out DateOnly? dateTo)
    {
        userId = UserId;
        dateFrom = DateFrom;
        dateTo = DateTo;
    }
}
