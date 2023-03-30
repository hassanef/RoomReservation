﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reservation.Infrastructure.Context;

namespace Reservation.Infrastructure.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    partial class ReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.Location", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.Office", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.OfficeAggregate.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasChair")
                        .HasColumnType("bit");

                    b.Property<int>("OfficeId")
                        .HasColumnType("int");

                    b.Property<byte>("PersonCapacity")
                        .HasColumnType("tinyint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.RoomReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReserveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomReservations");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.Office", b =>
                {
                    b.HasOne("Reservation.Domain.AggregatesModel.Location", null)
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.OfficeAggregate.Room", b =>
                {
                    b.HasOne("Reservation.Domain.AggregatesModel.Office", null)
                        .WithMany("Rooms")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Reservation.Domain.AggregatesModel.OfficeAggregate.RoomResource", "RoomResources", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("ResourceId")
                                .HasColumnType("int");

                            b1.Property<int>("RoomId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("ResourceId");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomResource");

                            b1.HasOne("Reservation.Domain.AggregatesModel.Resource", null)
                                .WithMany()
                                .HasForeignKey("ResourceId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.Navigation("RoomResources");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.RoomReservation", b =>
                {
                    b.HasOne("Reservation.Domain.AggregatesModel.OfficeAggregate.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Reservation.Domain.AggregatesModel.Period", "Period", b1 =>
                        {
                            b1.Property<int>("RoomReservationId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTime>("End")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("datetime2");

                            b1.HasKey("RoomReservationId");

                            b1.ToTable("RoomReservations");

                            b1.WithOwner()
                                .HasForeignKey("RoomReservationId");
                        });

                    b.OwnsMany("Reservation.Domain.AggregatesModel.ResourceReservation", "ResourceReservations", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("ResourceId")
                                .HasColumnType("int");

                            b1.Property<int>("RoomReservationId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("ResourceId");

                            b1.HasIndex("RoomReservationId");

                            b1.ToTable("ResourceReservations");

                            b1.HasOne("Reservation.Domain.AggregatesModel.Resource", null)
                                .WithMany()
                                .HasForeignKey("ResourceId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("RoomReservationId");
                        });

                    b.Navigation("Period");

                    b.Navigation("ResourceReservations");
                });

            modelBuilder.Entity("Reservation.Domain.AggregatesModel.Office", b =>
                {
                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
