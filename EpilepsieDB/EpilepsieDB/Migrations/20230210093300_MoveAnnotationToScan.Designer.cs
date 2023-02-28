﻿// <auto-generated />
using System;
using EpilepsieDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EpilepsieDB.Migrations
{
    [DbContext(typeof(EpilepsieDBContext))]
    [Migration("20230210093300_MoveAnnotationToScan")]
    partial class MoveAnnotationToScan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EpilepsieDB.Models.Annotation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int?>("BlockID")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Duration")
                        .HasColumnType("real");

                    b.Property<float>("Offset")
                        .HasColumnType("real");

                    b.Property<int>("ScanID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("BlockID");

                    b.HasIndex("ScanID");

                    b.ToTable("Annotations");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Block", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("Endtime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("GapToPrevious")
                        .HasColumnType("real");

                    b.Property<int>("ScanID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Starttime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ID");

                    b.HasIndex("ScanID");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Patient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContentDir")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MriImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NiftiFilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Recording", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("ContentDir")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PatientID")
                        .HasColumnType("integer");

                    b.Property<string>("RecordingNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("PatientID");

                    b.ToTable("Recordings");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Scan", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<float>("DurationOfDataRecord")
                        .HasColumnType("real");

                    b.Property<string>("EdfDisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EdfFilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Labels")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfRecords")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfSignals")
                        .HasColumnType("integer");

                    b.Property<string>("PatientInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhysicalDimensions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RecordInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RecordingID")
                        .HasColumnType("integer");

                    b.Property<string>("ScanNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransducerTypes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("RecordingID");

                    b.ToTable("Scans");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Signal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("Channel")
                        .HasColumnType("integer");

                    b.Property<short>("DigitalMaximum")
                        .HasColumnType("smallint");

                    b.Property<short>("DigitalMinimum")
                        .HasColumnType("smallint");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfSamples")
                        .HasColumnType("integer");

                    b.Property<string>("PhysicalDimension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("PhysicalMaximum")
                        .HasColumnType("double precision");

                    b.Property<double>("PhysicalMinimum")
                        .HasColumnType("double precision");

                    b.Property<string>("Prefiltering")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Reserved")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ScanID")
                        .HasColumnType("integer");

                    b.Property<string>("TransducerType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ScanID");

                    b.ToTable("Signals");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EpilepsieDB.Models.Annotation", b =>
                {
                    b.HasOne("EpilepsieDB.Models.Block", null)
                        .WithMany("Annotations")
                        .HasForeignKey("BlockID");

                    b.HasOne("EpilepsieDB.Models.Scan", "Scan")
                        .WithMany("Annotations")
                        .HasForeignKey("ScanID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scan");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Block", b =>
                {
                    b.HasOne("EpilepsieDB.Models.Scan", "Scan")
                        .WithMany("Blocks")
                        .HasForeignKey("ScanID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scan");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Recording", b =>
                {
                    b.HasOne("EpilepsieDB.Models.Patient", "Patient")
                        .WithMany("Recordings")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Scan", b =>
                {
                    b.HasOne("EpilepsieDB.Models.Recording", "Recording")
                        .WithMany("Scans")
                        .HasForeignKey("RecordingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recording");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Signal", b =>
                {
                    b.HasOne("EpilepsieDB.Models.Scan", "Scan")
                        .WithMany("Signals")
                        .HasForeignKey("ScanID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scan");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EpilepsieDB.Models.Block", b =>
                {
                    b.Navigation("Annotations");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Patient", b =>
                {
                    b.Navigation("Recordings");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Recording", b =>
                {
                    b.Navigation("Scans");
                });

            modelBuilder.Entity("EpilepsieDB.Models.Scan", b =>
                {
                    b.Navigation("Annotations");

                    b.Navigation("Blocks");

                    b.Navigation("Signals");
                });
#pragma warning restore 612, 618
        }
    }
}