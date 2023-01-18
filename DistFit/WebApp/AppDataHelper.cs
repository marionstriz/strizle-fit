using System.Security.Claims;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;

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
        
        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

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
            if (context.ExerciseTypes.Any() || context.Units.Any() ||
                context.MeasurementTypes.Any() || context.Measurements.Any()) return;
            
            var squatLangStr = new LangStr("Squat", LangStr.DefaultCulture);
            squatLangStr.SetTranslation("Kükk", "et-EE");
            var squat = new ExerciseType {Name = squatLangStr};
            context.ExerciseTypes.Add(squat);
            
            var dlLangStr = new LangStr("Deadlift", LangStr.DefaultCulture);
            dlLangStr.SetTranslation("Jõutõmme", "et-EE");
            var deadlift = new ExerciseType {Name = dlLangStr};
            context.ExerciseTypes.Add(deadlift);
            
            var kgNameLangStr = new LangStr("Kilogram", LangStr.DefaultCulture);
            kgNameLangStr.SetTranslation("Kilogramm", "et-EE");
            var kg = new Unit
            {
                Name = kgNameLangStr,
                Symbol = new LangStr("kg", LangStr.DefaultCulture)
            };
            context.Units.Add(kg);
            
            var timesLangStr = new LangStr("Time", LangStr.DefaultCulture);
            timesLangStr.SetTranslation("Kord", "et-EE");
            var times = new Unit
            {
                Name = timesLangStr,
                Symbol = new LangStr("x", LangStr.DefaultCulture)
            };
            context.Units.Add(times);
            
            var lbsLangString = new LangStr("Pound", LangStr.DefaultCulture);
            lbsLangString.SetTranslation("Nael", "et-EE");
            var pounds = new Unit
            {
                Name = lbsLangString,
                Symbol = new LangStr("lbs", LangStr.DefaultCulture)
            };
            context.Units.Add(pounds);
            
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

            if (userManager == null)
            {
                throw new NullReferenceException("userManager cannot be null");
            }
            var user = userManager.FindByEmailAsync("mastri@itcollege.ee").Result;
            if (user == null)
            {
                throw new NullReferenceException("Cannot find user with email mastri@itcollege.ee");
            }

            context.SaveChanges();

            var mastriWeightMeasurement1 = new Measurement
            {
                AppUserId = user.Id,
                MeasuredAt = new DateTime(2022, 12, 15, 12, 0, 0, DateTimeKind.Utc),
                Value = 70,
                ValueUnitId = kg.Id,
                MeasurementTypeId = weight.Id
            };
            context.Measurements.Add(mastriWeightMeasurement1);
            
            var mastriWeightMeasurement2 = new Measurement
            {
                AppUserId = user.Id,
                MeasuredAt = new DateTime(2023, 1, 15, 15, 30, 0, DateTimeKind.Utc),
                Value = 65,
                ValueUnitId = kg.Id,
                MeasurementTypeId = weight.Id
            };
            context.Measurements.Add(mastriWeightMeasurement2);
            
            var mastriUserExercise1 = new UserExercise
            {
                AppUserId = user.Id,
                ExerciseTypeId = squat.Id
            };
            context.UserExercises.Add(mastriUserExercise1);
            
            var mastriPerformance1 = new Performance
            {
                UserExerciseId = mastriUserExercise1.Id,
                PerformedAt = new DateTime(2023, 1, 15, 12, 0, 0, DateTimeKind.Utc)
            };
            context.Performances.Add(mastriPerformance1);
            
            var mastriSetEntry1 = new SetEntry
            {
                PerformanceId = mastriPerformance1.Id,
                Quantity = 12,
                QuantityUnitId = times.Id,
                Weight = 60,
                WeightUnitId = kg.Id
            };
            context.SetEntries.Add(mastriSetEntry1);
            
            var mastriSetEntry2 = new SetEntry
            {
                PerformanceId = mastriPerformance1.Id,
                Quantity = 10,
                QuantityUnitId = times.Id,
                Weight = 65,
                WeightUnitId = kg.Id
            };
            context.SetEntries.Add(mastriSetEntry2);
            
            var mastriSetEntry3 = new SetEntry
            {
                PerformanceId = mastriPerformance1.Id,
                Quantity = 6,
                QuantityUnitId = times.Id,
                Weight = 70,
                WeightUnitId = kg.Id
            };
            context.SetEntries.Add(mastriSetEntry3);

            context.SaveChanges();
        }
    }
}