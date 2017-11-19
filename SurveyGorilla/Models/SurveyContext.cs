using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin --> Survey[]
            modelBuilder.Entity<AdminEntity>()
                .HasMany(admin => admin.Surveys)
                .WithOne(survey => survey.Admin)
                .HasForeignKey(s => s.AdminId);

            // Admin.Email is unique
            modelBuilder.Entity<AdminEntity>()
                .HasIndex(admin => admin.Email)
                .IsUnique();

            // Survey --> Admin
            modelBuilder.Entity<SurveyEntity>()
                .HasOne(survey => survey.Admin)
                .WithMany(admin => admin.Surveys)
                .HasForeignKey(r => r.AdminId);

            // Survey --> Client[]
            modelBuilder.Entity<SurveyEntity>()
                .HasMany(survey => survey.Clients)
                .WithOne(client => client.Survey)
                .HasForeignKey(client => client.SurveyId);

            // Client --> Survey
            modelBuilder.Entity<ClientEntity>()
                .HasOne(client => client.Survey)
                .WithMany(survey => survey.Clients)
                .HasForeignKey(client => client.SurveyId);

            // (Client.Email + Client.SurveyId) is unique
            modelBuilder.Entity<ClientEntity>()
                .HasIndex(client => new { client.Email, client.SurveyId })
                .IsUnique();
        }

        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<SurveyEntity> Surveys { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
    }
}
