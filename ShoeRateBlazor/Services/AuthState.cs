using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public class AuthState
{
    private readonly IJSRuntime _jsRuntime;

    public AuthState(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public event Action OnChange;

    public async Task<bool> IsUserLoggedIn()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task Login(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        NotifyStateChanged();
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
