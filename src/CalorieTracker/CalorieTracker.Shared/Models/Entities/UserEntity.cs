namespace CalorieTracker.Shared.Models.Entities;

public class UserEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int DailyCalorieGoal { get; set; } = 2000;

    // Stored only on server/auth provider; kept optional locally
    public string? PasswordHash { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    // Indicates whether the user profile/state is synced with PostgreSQL
    public bool IsSynced { get; set; }
}