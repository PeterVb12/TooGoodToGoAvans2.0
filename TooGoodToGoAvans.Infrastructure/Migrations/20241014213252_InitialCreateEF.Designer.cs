﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TooGoodToGoAvans.Infrastructure;

#nullable disable

namespace TooGoodToGoAvans.Infrastructure.Migrations
{
    [DbContext(typeof(TooGoodToGoAvansDBContext))]
    [Migration("20241014213252_InitialCreateEF")]
    partial class InitialCreateEF
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Canteen", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CanteenLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<bool>("OffersWarmMeals")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Canteens");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Package", b =>
                {
                    b.Property<Guid>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AgeRestricted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("CanteenServedAtId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateAndTimeLastPickup")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateAndTimePickup")
                        .HasColumnType("datetime2");

                    b.Property<string>("MealType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<Guid?>("ReservedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PackageId");

                    b.HasIndex("CanteenServedAtId");

                    b.HasIndex("ReservedById");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Alcoholic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.StaffMember", b =>
                {
                    b.Property<Guid>("StaffMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EmployeeNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkLocationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StaffMemberId");

                    b.HasIndex("WorkLocationId");

                    b.ToTable("StaffMembers");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phonenumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentCity")
                        .HasColumnType("int");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Package", b =>
                {
                    b.HasOne("TooGoodToGoAvans.Domain.Models.Canteen", "CanteenServedAt")
                        .WithMany()
                        .HasForeignKey("CanteenServedAtId");

                    b.HasOne("TooGoodToGoAvans.Domain.Models.Student", "ReservedBy")
                        .WithMany()
                        .HasForeignKey("ReservedById");

                    b.Navigation("CanteenServedAt");

                    b.Navigation("ReservedBy");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Product", b =>
                {
                    b.HasOne("TooGoodToGoAvans.Domain.Models.Package", null)
                        .WithMany("Products")
                        .HasForeignKey("PackageId");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.StaffMember", b =>
                {
                    b.HasOne("TooGoodToGoAvans.Domain.Models.Canteen", "WorkLocation")
                        .WithMany()
                        .HasForeignKey("WorkLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkLocation");
                });

            modelBuilder.Entity("TooGoodToGoAvans.Domain.Models.Package", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
