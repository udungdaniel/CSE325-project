using Microsoft.AspNetCore.Identity;

namespace ThreadLine.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "Employee" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create the {role} role: "
                            + string.Join("; ", roleResult.Errors.Select(e => e.Description))
                    );
                }
            }
        }

        const string adminRole = "Admin";

        var email = Environment.GetEnvironmentVariable("THREADLINE_ADMIN_EMAIL");

        var password = Environment.GetEnvironmentVariable("THREADLINE_ADMIN_PASSWORD");

        // Create an administrator account only when credentials are configured.
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(admin, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create the administrator account: "
                        + string.Join("; ", createResult.Errors.Select(e => e.Description))
                );
            }
        }

        if (!await userManager.IsInRoleAsync(admin, adminRole))
        {
            var addRoleResult = await userManager.AddToRoleAsync(admin, adminRole);

            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not assign the Admin role: "
                        + string.Join("; ", addRoleResult.Errors.Select(e => e.Description))
                );
            }
        }
    }
}
