using App.DAL.EF;
using App.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.WebApp;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddMvc().AddApplicationPart(typeof(TStartup).Assembly);
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // and new DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();

            try
            {
                if (!db.Units.Any())
                {
                    var kg = new Unit
                    {
                        Name = new LangStr("Kilogram", LangStr.DefaultCulture),
                        Symbol = new LangStr("kg", LangStr.DefaultCulture),
                    };
                    db.Units.Add(kg);
                }

                if (!db.MeasurementTypes.Any())
                {
                    var measurementType = new MeasurementType
                    {
                        Name = new LangStr("Weight", LangStr.DefaultCulture)
                    };
                    db.MeasurementTypes.Add(measurementType);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        });
    }
}