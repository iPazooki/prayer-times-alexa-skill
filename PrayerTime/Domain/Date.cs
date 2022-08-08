namespace PrayerTime.Domain;

/// <summary>
/// Date information
/// </summary>
public class Date
{
    public string Readable { get; set; }

    public string Timestamp { get; set; }

    public Hijri Hijri { get; set; }

    public Gregorian Gregorian { get; set; }
}