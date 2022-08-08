namespace PrayerTime.Domain;

/// <summary>
/// Prayer detail
/// </summary>
public class PrayerInfo
{
    /// <summary>
    /// Adhan time
    /// </summary>
    public Timings Timings { get; set; }
    
    /// <summary>
    /// Date info
    /// </summary>
    public Date Date { get; set; }
    
    public Meta Meta { get; set; }
}