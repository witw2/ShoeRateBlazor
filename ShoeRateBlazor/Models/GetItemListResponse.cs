namespace ShoeRateBlazor.Models;
public class GetItemListResponse
{
    public List<Item> Items { get; set; }
    public int ItemCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}
