using Microsoft.AspNetCore.Identity;

namespace ThreadLine.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        const string adminRole = "Admin";
        const string staffRole = "Staff";

        // Ensure both roles exist.
        foreach (var roleName in new[] { adminRole, staffRole })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create the {roleName} role: " +
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        await SeedAdminAsync(userManager, adminRole);
        await SeedStaffAsync(userManager, staffRole);
    }

    private static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager, string adminRole)
    {
        var email = Environment.GetEnvironmentVariable(
            "THREADLINE_ADMIN_EMAIL");

        var password = Environment.GetEnvironmentVariable(
            "THREADLINE_ADMIN_PASSWORD");

        // Create an administrator account only when credentials are configured.
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
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
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                admin, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create the administrator account: " +
                    string.Join("; ", createResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(admin, adminRole))
        {
            var addRoleResult = await userManager.AddToRoleAsync(
                admin, adminRole);

            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not assign the Admin role: " +
                    string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }

    private static async Task SeedStaffAsync(
        UserManager<ApplicationUser> userManager, string staffRole)
    {
        // Staff account is always created so non-admin login can be demoed.
        // Override via environment variables if desired.
        var email = Environment.GetEnvironmentVariable(
            "THREADLINE_STAFF_EMAIL");

        var password = Environment.GetEnvironmentVariable(
            "THREADLINE_STAFF_PASSWORD");

        email = string.IsNullOrWhiteSpace(email)
            ? "staff@threadline.com"
            : email;

        password = string.IsNullOrWhiteSpace(password)
            ? "Staff@123"
            : password;

        var staff = await userManager.FindByEmailAsync(email);

        if (staff is null)
        {
            staff = new ApplicationUser
            {
                UserName = email,
                Email = email,
                // REQUIRED: RequireConfirmedAccount = true means an
                // unconfirmed email can never sign in.
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                staff, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create the staff account: " +
                    string.Join("; ", createResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(staff, staffRole))
        {
            var addRoleResult = await userManager.AddToRoleAsync(
                staff, staffRole);

            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not assign the Staff role: " +
                    string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}