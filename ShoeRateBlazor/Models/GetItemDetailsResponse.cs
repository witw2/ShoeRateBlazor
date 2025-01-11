namespace ShoeRateBlazor.Models;
public class GetItemDetailsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedByUserName { get; set; }
    public double AverageRating { get; set; }
}
