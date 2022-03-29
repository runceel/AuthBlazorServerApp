using AuthBlazorServerApp.Auth;
using AuthBlazorServerApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// 認証系のサービスを追加
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // 独自のログインページの URL
        options.LoginPath = "/MyLogin";
    });

// カスタムのハンドラー！
builder.Services.AddSingleton<IAuthorizationHandler, TestAuthHandler>(); // Scoped でも Transient でも可

builder.Services.AddAuthorization(options =>
{
    // EmployeeNumber が 0001 か 0011 の人のみ通すポリシー
    options.AddPolicy("EmployeeNumberIs0001Or0011", builder =>
    {
        builder.RequireClaim("EmployeeNumber", "0001", "0011");
    });

    // Test という名前のポリシーを登録
    options.AddPolicy("Test", builder =>
    {
        // ここで IAuthorizationRequirement を実装したクラスを設定する。
        builder.AddRequirements(new TestRequirement());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// 認証用のミドルウェア追加
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
