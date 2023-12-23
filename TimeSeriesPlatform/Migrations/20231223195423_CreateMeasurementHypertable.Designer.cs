﻿// <auto-generated />
using System;
using Iiroki.TimeSeriesPlatform.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Iiroki.TimeSeriesPlatform.Migrations
{
    [DbContext(typeof(TspDbContext))]
    [Migration("20231223195423_CreateMeasurementHypertable")]
    partial class CreateMeasurementHypertable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("tsp")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Iiroki.TimeSeriesPlatform.Database.Entities.IntegrationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Integration", "tsp");
                });

            modelBuilder.Entity("Iiroki.TimeSeriesPlatform.Database.Entities.MeasurementEntity", b =>
                {
                    b.Property<long>("IntegrationId")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasIndex("TagId");

                    b.HasIndex("IntegrationId", "TagId", "Timestamp")
                        .IsUnique();

                    b.ToTable("Measurement", "tsp");
                });

            modelBuilder.Entity("Iiroki.TimeSeriesPlatform.Database.Entities.TagEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Tag", "tsp");
                });

            modelBuilder.Entity("Iiroki.TimeSeriesPlatform.Database.Entities.MeasurementEntity", b =>
                {
                    b.HasOne("Iiroki.TimeSeriesPlatform.Database.Entities.IntegrationEntity", "Integration")
                        .WithMany()
                        .HasForeignKey("IntegrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Iiroki.TimeSeriesPlatform.Database.Entities.TagEntity", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Integration");

                    b.Navigation("Tag");
                });
#pragma warning restore 612, 618
        }
    }
}
