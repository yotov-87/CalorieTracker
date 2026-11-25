using CalorieTracker.Shared.Services;
using Microsoft.JSInterop;

namespace CalorieTracker.Web.Client.Services;

public class BrowserTokenStorageService : ITokenStorageService
{
    private const string TokenKey = "calorie_tracker_jwt";
    private readonly IJSRuntime _jsRuntime;

    public BrowserTokenStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            cancellationToken,
            TokenKey);
    }

    public async Task SetTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            cancellationToken,
            TokenKey,
            token);
    }

    public async Task RemoveTokenAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            cancellationToken,
            TokenKey);
    }
}