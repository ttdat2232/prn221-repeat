using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Repository.Models
{
    public class ClubMembershipContext : DbContext
    {
        private bool isUseInMemories = false;
        public ClubMembershipContext(bool isUseInMemories = false)
        {
            this.isUseInMemories = isUseInMemories;
        }
        private string GetConnectionString()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return config.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Cannot get connection string");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!isUseInMemories)
                optionsBuilder.UseSqlServer(GetConnectionString());
            else
                optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"))
                    .ConfigureWarnings(warmingBuilder => warmingBuilder.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>().HasKey(p => new { p.ClubActivityId, p.MembershipId });
        }

        public virtual DbSet<Major>? Majors { get; set; }
        public virtual DbSet<Grade>? Grades { get; set; }
        public virtual DbSet<Student>? Students { get; set; }
        public virtual DbSet<Membership>? Memberships { get; set; }
        public virtual DbSet<Club>? Clubs { get; set; }
        public virtual DbSet<Participant>? Participants { get; set; }
        public virtual DbSet<ClubBoard>? ClubBoards { get; set; }
        public virtual DbSet<ClubActivity>? ClubActivities { get; set; }
    }
}
