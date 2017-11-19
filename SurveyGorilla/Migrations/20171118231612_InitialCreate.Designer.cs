﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SurveyGorilla.Models;
using System;

namespace SurveyGorilla.Migrations
{
    [DbContext(typeof(SurveyContext))]
    [Migration("20171118231612_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SurveyGorilla.Models.AdminEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("Info")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("SurveyGorilla.Models.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("Info")
                        .IsRequired();

                    b.Property<int>("SurveyId");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.HasIndex("EmailAddress", "SurveyId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SurveyGorilla.Models.SurveyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdminId");

                    b.Property<string>("Info")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("SurveyGorilla.Models.ClientEntity", b =>
                {
                    b.HasOne("SurveyGorilla.Models.SurveyEntity", "Survey")
                        .WithMany("Clients")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SurveyGorilla.Models.SurveyEntity", b =>
                {
                    b.HasOne("SurveyGorilla.Models.AdminEntity", "Admin")
                        .WithMany("Surveys")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}