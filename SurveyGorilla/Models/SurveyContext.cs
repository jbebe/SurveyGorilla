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
            modelBuilder.Entity<AdminEntity>().HasKey(a => a.Id);
            modelBuilder.Entity<SurveyEntity>().HasKey(s => s.Id);
            modelBuilder.Entity<ClientEntity>().HasKey(c => c.Id);

            // Admin.Survey => []
            modelBuilder.Entity<AdminEntity>()
                .HasMany<SurveyEntity>(a => a.Surveys)
                .WithOne(s => s.Admin)
                .HasForeignKey(s => s.AdminId)
                .HasPrincipalKey(a => a.Id);

            // Survey.Admin => Admin
            modelBuilder.Entity<SurveyEntity>()
                .HasOne<AdminEntity>(s => s.Admin)
                .WithMany(a => a.Surveys)
                .HasForeignKey(s => s.AdminId)
                .HasPrincipalKey(a => a.Id);

            // Survey.Clients => []
            modelBuilder.Entity<SurveyEntity>()
                .HasMany<ClientEntity>(s => s.Clients)
                .WithOne(c => c.Survey)
                .HasForeignKey(c => c.SurveyId)
                .HasPrincipalKey(s => s.Id);

            // Client.Survey => Survey
            modelBuilder.Entity<ClientEntity>()
                .HasOne<SurveyEntity>(c => c.Survey)
                .WithMany(s => s.Clients)
                .HasForeignKey(c => c.SurveyId)
                .HasPrincipalKey(s => s.Id);

            // Admin.Email is unique
            modelBuilder.Entity<AdminEntity>()
                .HasIndex(admin => admin.Email)
                .IsUnique();

            // Survey.Name is unique
            modelBuilder.Entity<SurveyEntity>()
                .HasIndex(survey => survey.Name)
                .IsUnique();

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
