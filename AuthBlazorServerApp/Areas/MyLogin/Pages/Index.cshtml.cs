using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthBlazorServerApp.Areas.MySignin.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required]
    public string? UserName { get; set; }
    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid is false) return Page();

        var userName = UserName!;
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            CreateClaims(userName),
            CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
        return Redirect("/");
    }

    private IEnumerable<Claim> CreateClaims(string userName)
    {
        yield return new Claim(ClaimTypes.Name, userName);
        yield return new Claim(ClaimTypes.Role, "User");
        if (userName == "Admin")
        {
            yield return new Claim(ClaimTypes.Role, "Administrator");
        }

        yield return userName switch
        {
            "Admin" => new Claim("EmployeeNumber", "0001"),
            "Kazuki" => new Claim("EmployeeNumber", "0011"),
            "Shinji" => new Claim("EmployeeNumber", "0111"),
            "Kazuaki" => new Claim("EmployeeNumber", "1111"),
            _ => new Claim("EmployeeNumber", "9999"),
        };
    }
}
