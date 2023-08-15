﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Todo.Infra.Context;

#nullable disable

namespace Todo.Infra.Migrations
{
    [DbContext(typeof(TodoDbContext))]
    [Migration("20230815181732_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Todo.Domain.Models.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AssignmentListId")
                        .HasColumnType("char(36)");

                    b.Property<sbyte>("Concluded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((sbyte)0);

                    b.Property<DateTime?>("ConcludedAt")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("VARCHAR(255)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("DATETIME");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentListId");

                    b.HasIndex("UserId");

                    b.ToTable("Tarefas", (string)null);
                });

            modelBuilder.Entity("Todo.Domain.Models.AssignmentList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Lista de Tarefas", (string)null);
                });

            modelBuilder.Entity("Todo.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("DATETIME");

                    b.HasKey("Id");

                    b.ToTable("Usuários", (string)null);
                });

            modelBuilder.Entity("Todo.Domain.Models.Assignment", b =>
                {
                    b.HasOne("Todo.Domain.Models.AssignmentList", "AssignmentList")
                        .WithMany("Assignments")
                        .HasForeignKey("AssignmentListId");

                    b.HasOne("Todo.Domain.Models.User", "User")
                        .WithMany("Assignments")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("AssignmentList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Todo.Domain.Models.AssignmentList", b =>
                {
                    b.HasOne("Todo.Domain.Models.User", "User")
                        .WithMany("AssignmentLists")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Todo.Domain.Models.AssignmentList", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Todo.Domain.Models.User", b =>
                {
                    b.Navigation("AssignmentLists");

                    b.Navigation("Assignments");
                });
#pragma warning restore 612, 618
        }
    }
}