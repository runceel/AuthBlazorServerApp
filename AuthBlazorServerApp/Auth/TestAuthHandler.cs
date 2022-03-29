using Microsoft.AspNetCore.Authorization;

namespace AuthBlazorServerApp.Auth;

public class TestAuthHandler : AuthorizationHandler<TestRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestRequirement requirement)
    {
        // EmployeeNumber の Claim があって
        var employeeNumberClaim = context.User.Claims.FirstOrDefault(x => x.Type == "EmployeeNumber");
        if (employeeNumberClaim is null) return Task.CompletedTask;

        // 右から 3 桁目が 1 だったら OK (EmployeeNumber は 4 桁想定なので index = 1 が 3 桁目)
        if (employeeNumberClaim.Value.Length == 4 && employeeNumberClaim.Value[1] == '1')
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
