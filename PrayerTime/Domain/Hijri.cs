using System.Collections.Generic;

namespace PrayerTime.Domain;

/// <summary>
/// Hijri date
/// </summary>
public class Hijri
{
    public string Date { get; set; }
    
    public string Format { get; set; }
    
    public string Day { get; set; }
    
    public Weekday Weekday { get; set; }
    
    public Month Month { get; set; }
    
    public string Year { get; set; }
    
    public Designation Designation { get; set; }
    
    public List<object> Holidays { get; set; }
}