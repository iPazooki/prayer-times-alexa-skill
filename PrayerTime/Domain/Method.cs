namespace PrayerTime.Domain;

public class Method
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public Params Params { get; set; }
    
    public Location Location { get; set; }
}