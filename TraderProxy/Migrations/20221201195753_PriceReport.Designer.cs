﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TraderProxy;

#nullable disable

namespace TraderProxy.Migrations
{
    [DbContext(typeof(ProxyEfDbContext))]
    [Migration("20221201195753_PriceReport")]
    partial class PriceReport
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TraderProxy.Entities.MarketStateSnap", b =>
                {
                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Trade")
                        .HasColumnType("int");

                    b.Property<string>("TradeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DateTime");

                    b.ToTable("MarketStateSnaps");
                });
#pragma warning restore 612, 618
        }
    }
}