namespace PrayerTime.Domain;

public class Meta
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public string Timezone { get; set; }
    
    public Method Method { get; set; }
    
    public string LatitudeAdjustmentMethod { get; set; }
    
    public string MidnightMode { get; set; }
    
    public string School { get; set; }
    
    public Offset Offset { get; set; }
}