using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(!isUseInMemories)
                optionsBuilder.UseSqlServer(GetConnectionString());
            else 
                optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>().HasKey(p => new { p.ClubActivityId, p.MembershipId });
            //modelBuilder.Entity<Membership>()
            //    .HasMany(ms => ms.ClubBoards)
            //    .WithMany(cb => cb.Memberships)
            //    .UsingEntity(
            //    "MemberShipClubBoard",
            //    l => l.HasOne(typeof(ClubBoard)).WithMany().HasForeignKey("ClubBoardId").HasPrincipalKey(nameof(ClubBoard.Id)),
            //    r => r.HasOne(typeof(Membership)).WithMany().HasForeignKey("MembershipId").HasPrincipalKey(nameof(Membership.Id)),
            //    j => j.HasKey("MembershipId", "ClubBoardId"));
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
