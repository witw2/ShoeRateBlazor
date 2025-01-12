using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using ShoeRateBlazor.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly AuthState _authState;

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager, AuthState authState)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _authState = authState;
    }

    private async Task AddAuthorizationHeader()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task EnsureAuthenticated()
    {
        if (!await _authState.IsUserLoggedIn())
        {
            _navigationManager.NavigateTo("/login");
            throw new UnauthorizedAccessException("User is not authenticated");
        }
    }

    public async Task<CreateItemResponse> CreateItemAsync(CreateItemRequest request)
    {
        await EnsureAuthenticated();
        await AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync("/api/item", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateItemResponse>();
    }

    public async Task<GetItemListResponse> GetItemsAsync(int pageSize, int pageNumber, string search)
    {
        await EnsureAuthenticated();
        var response = await _httpClient.GetAsync($"/api/item?PageSize={pageSize}&PageNumber={pageNumber}&Search={search}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetItemListResponse>();
    }

    public async Task<GetItemDetailsResponse> GetItemDetailsAsync(Guid itemId)
    {
        await EnsureAuthenticated();
        var response = await _httpClient.GetAsync($"/api/item/{itemId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetItemDetailsResponse>();
    }

    public async Task<CreateRatingResponse> CreateRatingAsync(Guid itemId, CreateRatingRequest request)
    {
        await EnsureAuthenticated();
        await AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync($"/api/item/{itemId}/ratings", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateRatingResponse>();
    }

    public async Task<GetRatingListResponse> GetRatingsAsync(Guid itemId, int pageNumber)
    {
        await EnsureAuthenticated();
        var response = await _httpClient.GetAsync($"/api/item/{itemId}/ratings?PageNumber={pageNumber}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetRatingListResponse>();
    }

    public async Task RemoveRatingAsync(Guid itemId)
    {
        await EnsureAuthenticated();
        await AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"/api/item/{itemId}/ratings");
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
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
        await _authState.Login(loginResponse.Token);
        _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        return loginResponse;
    }
}
