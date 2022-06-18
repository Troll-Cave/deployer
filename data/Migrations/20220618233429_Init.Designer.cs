﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using data;
using data.Models;

#nullable disable

namespace data.Migrations
{
    [DbContext(typeof(DeployerContext))]
    [Migration("20220618233429_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("data.DataModels.ApplicationDTO", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("Organization")
                        .HasColumnType("uuid")
                        .HasColumnName("org");

                    b.Property<Guid?>("PipelineVersionId")
                        .HasColumnType("uuid")
                        .HasColumnName("pipeline");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("source");

                    b.Property<Dictionary<string, string>>("Variables")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("variables");

                    b.HasKey("ID");

                    b.HasIndex("Organization");

                    b.HasIndex("PipelineVersionId");

                    b.ToTable("application");
                });

            modelBuilder.Entity("data.DataModels.ConfigDTO", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Key");

                    b.ToTable("config");
                });

            modelBuilder.Entity("data.DataModels.OrganizationDTO", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("ID");

                    b.ToTable("organization");
                });

            modelBuilder.Entity("data.DataModels.PipelineDTO", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("Organization")
                        .HasColumnType("uuid")
                        .HasColumnName("org");

                    b.HasKey("ID");

                    b.HasIndex("Organization");

                    b.ToTable("pipeline");
                });

            modelBuilder.Entity("data.DataModels.PipelineVersionDTO", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Pipeline>("Code")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("code");

                    b.Property<Dictionary<string, string>>("Files")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("files");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("PipelineId")
                        .HasColumnType("uuid")
                        .HasColumnName("pipeline");

                    b.HasKey("ID");

                    b.HasIndex("PipelineId");

                    b.ToTable("pipeline_version");
                });

            modelBuilder.Entity("data.DataModels.ApplicationDTO", b =>
                {
                    b.HasOne("data.DataModels.OrganizationDTO", null)
                        .WithMany()
                        .HasForeignKey("Organization")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("data.DataModels.PipelineVersionDTO", null)
                        .WithMany()
                        .HasForeignKey("PipelineVersionId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("data.DataModels.PipelineDTO", b =>
                {
                    b.HasOne("data.DataModels.OrganizationDTO", null)
                        .WithMany()
                        .HasForeignKey("Organization")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("data.DataModels.PipelineVersionDTO", b =>
                {
                    b.HasOne("data.DataModels.PipelineDTO", null)
                        .WithMany()
                        .HasForeignKey("PipelineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}