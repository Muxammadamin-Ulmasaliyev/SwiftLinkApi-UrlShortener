﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwiftLink;

#nullable disable

namespace SwiftLink.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SwiftLink.Entities.ShortenedURL", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("varchar(7)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ShortUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ShortenedUrls");
                });
#pragma warning restore 612, 618
        }
    }
}
