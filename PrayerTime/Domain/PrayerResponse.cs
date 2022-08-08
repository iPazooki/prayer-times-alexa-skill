namespace PrayerTime.Domain;

/// <summary>
/// Prayer response class after calling API : https://aladhan.com/prayer-times-api
/// </summary>
public class PrayerResponse
{
    /// <summary>
    /// HTTP Status code
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// HTTP Status result
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Prayer data
    /// </summary>
    public PrayerInfo Data { get; set; }
}