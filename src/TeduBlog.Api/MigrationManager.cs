using Microsoft.EntityFrameworkCore;
using TeduBlog.Data;

namespace TeduBlog.Api
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<TeduBlogContext>())
                    {
                        context.Database.Migrate();
                        var dataSeeder = new DataSeeder();
                        Task.Run(async () => await dataSeeder.SeedAsync(context)).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger?.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }
            return app;
        }
    }
}