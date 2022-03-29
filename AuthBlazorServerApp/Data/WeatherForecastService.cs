using System.Security.Claims;
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
        // ユーザーが Test ポリシーを満たしているかどうか
        var result = await _authorizationService.AuthorizeAsync(user, "Test");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            // Test ポリシーを満たしているなら全データを Warm にする
            Summary = result.Succeeded ? "Warm" : Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
}
