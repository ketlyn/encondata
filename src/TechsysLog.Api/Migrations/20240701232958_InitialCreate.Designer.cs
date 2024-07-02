﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechsysLog.Api.Data;

#nullable disable

namespace TechsysLog.Api.Migrations
{
    [DbContext(typeof(TechsysLogDBContext))]
    [Migration("20240701232958_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TechsysLog.Api.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TechsysLog.Api.Entities.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid?>("OrderNumberId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrderNumberId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("TechsysLog.Api.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TechsysLog.Api.Entities.Delivery", b =>
                {
                    b.HasOne("TechsysLog.Api.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderNumberId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("TechsysLog.Api.Entities.Order", b =>
                {
                    b.HasOne("TechsysLog.Api.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}