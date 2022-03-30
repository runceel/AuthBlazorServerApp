using System.Security.Claims;
using AuthBlazorServerApp.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AuthBlazorServerApp.Data;

public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private readonly IAuthorizationService _authorizationService;

    public WeatherForecastService(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate, ClaimsPrincipal user)
    {
        // ���[�U�[�� Test �|���V�[�𖞂����Ă��邩�ǂ���
        var result = await _authorizationService.AuthorizeAsync(user, new AuthorizationPolicy(
            new[] { new TestRequirement() },
            Enumerable.Empty<string>()));
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            // Test �|���V�[�𖞂����Ă���Ȃ�S�f�[�^�� Warm �ɂ���
            Summary = result.Succeeded ? "Warm" : Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
}
