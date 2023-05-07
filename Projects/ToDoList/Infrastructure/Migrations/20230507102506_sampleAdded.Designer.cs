﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230507102506_sampleAdded")]
    partial class sampleAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.SampleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RandomPropertyEight")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyFive")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyFour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyNine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyOne")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertySeven")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertySix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyThree")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RandomPropertyTwo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ToDoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ToDoId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("Domain.Entities.ToDoAttachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ToDoItemId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ToDoItemId");

                    b.ToTable("ToDoAttachments");
                });

            modelBuilder.Entity("Domain.Entities.ToDoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("ToDoItems");
                });

            modelBuilder.Entity("Domain.Entities.SampleEntity", b =>
                {
                    b.HasOne("Domain.Entities.ToDoItem", "ToDo")
                        .WithMany("Samples")
                        .HasForeignKey("ToDoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToDo");
                });

            modelBuilder.Entity("Domain.Entities.ToDoAttachment", b =>
                {
                    b.HasOne("Domain.Entities.ToDoItem", "ToDoItem")
                        .WithMany("Attachments")
                        .HasForeignKey("ToDoItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToDoItem");
                });

            modelBuilder.Entity("Domain.Entities.ToDoItem", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Samples");
                });
#pragma warning restore 612, 618
        }
    }
}
