using CalorieTracker.Shared.Services;

namespace CalorieTracker.Services;

public class MauiTokenStorageService : ITokenStorageService
{
    private const string TokenKey = "calorie_tracker_jwt";

    public async Task<string?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await SecureStorage.Default.GetAsync(TokenKey);
    }

    public async Task SetTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        await SecureStorage.Default.SetAsync(TokenKey, token);
    }

    public Task RemoveTokenAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        SecureStorage.Default.Remove(TokenKey);
        return Task.CompletedTask;
    }
}