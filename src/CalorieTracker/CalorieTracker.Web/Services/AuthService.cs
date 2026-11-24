using CalorieTracker.Shared.DTOs;
using CalorieTracker.Shared.Models.Entities;
using CalorieTracker.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Web.Services;

public class AuthService : IAuthService
{
    private readonly CalorieTrackerAppDbContext _dbContext;

    public AuthService(CalorieTrackerAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var exists = await _dbContext.Users
            .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (exists)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "A user with this email already exists."
            };
        }

        // DailyCalorieGoal uses the entity/database default when not provided by RegisterRequestDto.
        var user = new UserEntity
        {
            Id = Guid.NewGuid().ToString(),
            Email = normalizedEmail,
            FullName = request.FullName.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Registration succeeded.",
            User = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DailyCalorieGoal = user.DailyCalorieGoal
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        return new AuthResponseDto
        {
            Success = true,
            Message = "Login succeeded.",
            User = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DailyCalorieGoal = user.DailyCalorieGoal
            }
        };
    }
}