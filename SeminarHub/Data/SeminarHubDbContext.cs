using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SeminarHub.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SeminarParticipant>()
                .HasKey(sp => new {sp.SeminarId, sp.ParticipantId});

            builder.Entity<SeminarParticipant>()
                .HasOne(s => s.Seminar)
                .WithMany(p => p.SeminarParticipants)
                .OnDelete(DeleteBehavior.NoAction);
            builder
               .Entity<Category>()
               .HasData(new Category()               
               {
                   Id = 1,
                   Name = "technology & innovation"
               },
               new Category()
               {
                   Id = 2,
                   Name = "business & entrepreneurship"
               },
               new Category()
               {
                   Id = 3,
                   Name = "science & research"
               },
               new Category()
               {
                   Id = 4,
                   Name = "arts & culture"
               });

            base.OnModelCreating(builder);
        }

        public DbSet<Seminar> Seminars { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<SeminarParticipant> SeminaarParticipants { get; set; }
    }
}