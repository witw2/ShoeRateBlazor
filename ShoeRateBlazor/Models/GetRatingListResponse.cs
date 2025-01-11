namespace ShoeRateBlazor.Models;
public class GetRatingListResponse
{
    public List<RatingItem> Ratings { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}
