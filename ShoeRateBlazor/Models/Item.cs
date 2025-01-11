namespace ShoeRateBlazor.Models;
public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double AverageRating { get; set; }
    public string CreatedByUserName { get; set; }
}
