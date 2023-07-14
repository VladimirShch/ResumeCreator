using Microsoft.EntityFrameworkCore;
using ResumeCoverLetterCreator.DataAccess;

namespace ResumeCoverLetterCreator
{
    public static class SeedData
    {
        public static void EnsureCreated(IServiceProvider serviceProvider)
        {
            using var scopedServices = serviceProvider.CreateScope();
            using ResumeCreatorDbContext dbContext = scopedServices.ServiceProvider.GetRequiredService<ResumeCreatorDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
