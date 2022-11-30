﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RabbitMq.DAL;

#nullable disable

namespace RabbitMq.DAL.Migrations
{
    [DbContext(typeof(RabbitMqDb))]
    [Migration("20221126211822_RemovedSender")]
    partial class RemovedSender
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RabbitMq.Common.Entities.Notifications.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RecieverConnectionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RecieverId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RecieverId");

                    b.ToTable("Notification");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Notification");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.Notifications.PrivateNotification", b =>
                {
                    b.HasBaseType("RabbitMq.Common.Entities.Notifications.Notification");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("PrivateNotification");
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.Notifications.PublicNotification", b =>
                {
                    b.HasBaseType("RabbitMq.Common.Entities.Notifications.Notification");

                    b.HasDiscriminator().HasValue("PublicNotification");
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.Notifications.SimpleNotification", b =>
                {
                    b.HasBaseType("RabbitMq.Common.Entities.Notifications.Notification");

                    b.HasDiscriminator().HasValue("SimpleNotification");
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.Notifications.Notification", b =>
                {
                    b.HasOne("RabbitMq.Common.Entities.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("RecieverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitMq.Common.Entities.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
