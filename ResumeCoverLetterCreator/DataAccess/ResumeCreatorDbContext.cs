using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace ResumeCoverLetterCreator.DataAccess
{
    public class ResumeCreatorDbContext : DbContext
    {
        public ResumeCreatorDbContext(DbContextOptions<ResumeCreatorDbContext> options) : base(options)
        {
           
        }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentTag> DocumentTags { get; set; }
        public DbSet<TagContentItem> TagContent { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }
        public DbSet<GroupOptionsItem> GroupOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TagGroup>()
                .HasMany(tg => tg.GroupOptions)
                .WithOne(o => o.TagGroup);

            builder.Entity<TagContentItem>()
                .HasOne(c => c.DocumentTag)
                .WithMany(t => t.TagContent);

            builder.Entity<TagContentItem>()
                .HasMany(c => c.GroupOptions)
                .WithMany(o => o.TagContent)
                .UsingEntity<GroupOptionContentItem>(j => j.ToTable("GroupOptionContent"));
        }
    }
}
