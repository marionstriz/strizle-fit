using System.Security.Claims;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

/// <summary>
/// Application data helper class
/// </summary>
public static class AppDataHelper
{
    /// <summary>
    /// Set up dropping and migrating database, seeding identity and
    /// application data.
    /// Configurable in appsettings.json
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="env">Application environment</param>
    /// <param name="conf">Application configuration</param>
    /// <exception cref="ApplicationException">No db context in services</exception>
    /// <exception cref="NullReferenceException">User manager or role manager cannot be null</exception>
    public static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration conf)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope
            .ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No db context.");
        }

        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return;
        
        if (conf.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }
        if (conf.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }
        if (conf.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (userManager == null || roleManager == null)
            {
                throw new NullReferenceException("userManager or roleManager cannot be null");
            }
            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var roleName in roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole{Name = roleName}).Result;
                    if (!identityResult.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed");
                    }
                }
            }

            var users = new (string username, string firstname, string lastname, string password, string roles)[]
            {
                ("admin@itcollege.ee", "Big", "Admin", "Kala.maja1", "user,admin"),
                ("mastri@itcollege.ee", "Marion", "Striz", "Kala.maja1", "user,admin"),
                ("user@itcollege.ee", "Regular", "User", "Kala.maja1", "user"),
                ("newuser@itcollege.ee", "No", "Rights", "Kala.maja1", "")
            };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = userInfo.username,
                        FirstName = userInfo.firstname,
                        LastName = userInfo.lastname,
                        UserName = userInfo.username,
                        EmailConfirmed = true,
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;
                    if (!identityResult.Succeeded)
                    {
                        throw new ApplicationException("Cannot create user!");
                    }
                    var claim1 = userManager.AddClaimAsync(user, new Claim("aspnet.firstname", user.FirstName)).Result;
                    var claim2 = userManager.AddClaimAsync(user, new Claim("aspnet.lastname", user.LastName)).Result;
                }

                if (!string.IsNullOrWhiteSpace(userInfo.roles))
                {
                    var identityResultRole = 
                        userManager.AddToRolesAsync(user, userInfo.roles.Split(",")).Result;
                }
            }
        }
        if (conf.GetValue<bool>("DataInitialization:SeedData"))
        {
            var changesMade = false;
            if (!context.ExerciseTypes.Any())
            {
                var exLangStr = new LangStr("Squat", LangStr.DefaultCulture);
                exLangStr.SetTranslation("Kükk", "et-EE");
                var exerciseTypeOne = new ExerciseType {Name = exLangStr};
                context.ExerciseTypes.Add(exerciseTypeOne);
                changesMade = true;
            }
            
            if (!context.Units.Any())
            {
                var kgNameLangStr = new LangStr("Kilogram", LangStr.DefaultCulture);
                kgNameLangStr.SetTranslation("Kilogramm", "et-EE");
                var kg = new Unit
                {
                    
                    Name = kgNameLangStr,
                    Symbol = new LangStr("kg", LangStr.DefaultCulture)
                };
                context.Units.Add(kg);
                context.SaveChanges();
                changesMade = true;
            }
            if (!context.MeasurementTypes.Any())
            {
                var weightLangStr = new LangStr("Weight", LangStr.DefaultCulture);
                weightLangStr.SetTranslation("Kaal", "et-EE");
                var weight = new MeasurementType{Name = weightLangStr};
                context.MeasurementTypes.Add(weight);
                
                var heightLangStr = new LangStr("Height", LangStr.DefaultCulture);
                heightLangStr.SetTranslation("Pikkus", "et-EE");
                var height = new MeasurementType{Name = heightLangStr};
                context.MeasurementTypes.Add(height);
                
                var waistCircum = new LangStr("Waist circumference", LangStr.DefaultCulture);
                waistCircum.SetTranslation("Pihaümbermõõt", "et-EE");
                var waistCirc = new MeasurementType{Name = waistCircum};
                context.MeasurementTypes.Add(waistCirc);
                
                var hipsCircum = new LangStr("Hip circumference", LangStr.DefaultCulture);
                hipsCircum.SetTranslation("Puusaümbermõõt", "et-EE");
                var hipsCirc = new MeasurementType{Name = hipsCircum};
                context.MeasurementTypes.Add(hipsCirc);
                
                context.SaveChanges();
                changesMade = true;
            }
            if (changesMade) context.SaveChanges();
        }
    }
}