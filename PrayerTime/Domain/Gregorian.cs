namespace PrayerTime.Domain;

/// <summary>
/// Gregorian detail
/// </summary>
public class Gregorian
{
    public string Date { get; set; }
    
    public string Format { get; set; }
    
    public string Day { get; set; }
    
    public Weekday Weekday { get; set; }
    
    public Month Month { get; set; }
    
    public string Year { get; set; }
    
    public Designation Designation { get; set; }
}