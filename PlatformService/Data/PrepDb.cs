using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(this IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("---> Attempting to apply migraions");
                try
                {
                context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not run migrations: {ex.Message}");
                    throw;
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data ...");
                
                context.Platforms.AddRange(
                        new Platform() {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                        new Platform() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                        new Platform() {Name = "Kubernetes", Publisher = "Microsoft", Cost = "Free"}
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
