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

// �F�،n�̃T�[�r�X��ǉ�
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // �Ǝ��̃��O�C���y�[�W�� URL
        options.LoginPath = "/MyLogin";
    });

// �J�X�^���̃n���h���[�I
builder.Services.AddSingleton<IAuthorizationHandler, TestAuthHandler>(); // Scoped �ł� Transient �ł���

builder.Services.AddAuthorization(options =>
{
    // EmployeeNumber �� 0001 �� 0011 �̐l�̂ݒʂ��|���V�[
    options.AddPolicy("EmployeeNumberIs0001Or0011", builder =>
    {
        builder.RequireClaim("EmployeeNumber", "0001", "0011");
    });

    // Test �Ƃ������O�̃|���V�[��o�^
    options.AddPolicy("Test", builder =>
    {
        // ������ IAuthorizationRequirement �����������N���X��ݒ肷��B
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

// �F�ؗp�̃~�h���E�F�A�ǉ�
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
