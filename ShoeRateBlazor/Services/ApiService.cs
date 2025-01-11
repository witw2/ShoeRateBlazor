using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using ShoeRateBlazor.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    private async Task AddAuthorizationHeader()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<CreateItemResponse> CreateItemAsync(CreateItemRequest request)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/item", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateItemResponse>();
    }

    public async Task<GetItemListResponse> GetItemsAsync(int pageSize, int pageNumber, string search)
    {
        var response = await _httpClient.GetAsync($"/api/item?PageSize={pageSize}&PageNumber={pageNumber}&Search={search}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetItemListResponse>();
    }

    public async Task<GetItemDetailsResponse> GetItemDetailsAsync(Guid itemId)
    {
        var response = await _httpClient.GetAsync($"/api/item/{itemId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetItemDetailsResponse>();
    }

    public async Task<CreateRatingResponse> CreateRatingAsync(Guid itemId, CreateRatingRequest request)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync($"/api/item/{itemId}/ratings", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateRatingResponse>();
    }

    public async Task<GetRatingListResponse> GetRatingsAsync(Guid itemId, int pageNumber)
    {
        var response = await _httpClient.GetAsync($"/api/item/{itemId}/ratings?PageNumber={pageNumber}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetRatingListResponse>();
    }

    public async Task RemoveRatingAsync(Guid itemId, Guid ratingId)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"/api/item/{itemId}/ratings/{ratingId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<CreateUserResponse> RegisterUserAsync(CreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/user/register", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/user/login", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginUserResponse>();
    }
}
