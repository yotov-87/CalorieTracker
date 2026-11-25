using CalorieTracker.Shared.DTOs;

namespace CalorieTracker.Shared.Services;

public class ClientAuthStateService
{
    private readonly ITokenStorageService _tokenStorage;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
    public string? Token { get; private set; }
    public UserProfileDto? CurrentUser { get; private set; }

    public event Action? OnChange;

    public ClientAuthStateService(ITokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        Token = await _tokenStorage.GetTokenAsync(cancellationToken);
        NotifyStateChanged();
    }

    public async Task SetSessionAsync(AuthResponseDto response, CancellationToken cancellationToken = default)
    {
        if (!response.Success || string.IsNullOrWhiteSpace(response.Token))
        {
            return;
        }

        Token = response.Token;
        CurrentUser = response.User;

        await _tokenStorage.SetTokenAsync(response.Token, cancellationToken);
        NotifyStateChanged();
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        Token = null;
        CurrentUser = null;

        await _tokenStorage.RemoveTokenAsync(cancellationToken);
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}