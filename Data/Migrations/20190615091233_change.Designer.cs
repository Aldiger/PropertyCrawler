﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PropertyCrawler.Data;

namespace PropertyCrawler.Data.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20190615091233_change")]
    partial class change
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PropertyCrawler.Data.Agent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("AgentCode");

                    b.Property<string>("AgentType");

                    b.Property<string>("BranchName");

                    b.Property<string>("BranchPostcode");

                    b.Property<string>("BrandName");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CompanyType");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("DisplayAddress");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("PropertyCrawler.Data.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Caption");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("PropertyId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("PropertyCrawler.Data.Portal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Portals");
                });

            modelBuilder.Entity("PropertyCrawler.Data.PostalCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("OpCode");

                    b.Property<int>("OutCode");

                    b.HasKey("Id");

                    b.ToTable("PostalCodes");
                });

            modelBuilder.Entity("PropertyCrawler.Data.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Added");

                    b.Property<string>("Address");

                    b.Property<int>("AgentId");

                    b.Property<byte>("BedroomsCount");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("FloorPlanCount");

                    b.Property<double>("Latitude");

                    b.Property<string>("LettingType");

                    b.Property<double>("Longtitude");

                    b.Property<string>("PostalCode");

                    b.Property<string>("PostalCodeExtended");

                    b.Property<string>("PostalCodeFull");

                    b.Property<string>("PostalCodePrefix");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("PropertyAdded");

                    b.Property<int?>("PropertyDescriptionId");

                    b.Property<string>("PropertySubType");

                    b.Property<string>("PropertyType");

                    b.Property<int>("Type");

                    b.Property<int?>("UrlId");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("PropertyDescriptionId")
                        .IsUnique()
                        .HasFilter("[PropertyDescriptionId] IS NOT NULL");

                    b.HasIndex("UrlId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("PropertyCrawler.Data.PropertyDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description");

                    b.Property<int?>("PropertyId");

                    b.HasKey("Id");

                    b.ToTable("PropertyDescriptions");
                });

            modelBuilder.Entity("PropertyCrawler.Data.PropertyPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<decimal>("Price");

                    b.Property<string>("PriceQualifier");

                    b.Property<int>("PropertyId");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyPrices");
                });

            modelBuilder.Entity("PropertyCrawler.Data.Url", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("PortalId");

                    b.Property<int>("PostalCodeId");

                    b.Property<int>("PropertyCode");

                    b.Property<string>("PropertyUrl");

                    b.Property<int>("Type");

                    b.Property<int?>("UrlTypeId");

                    b.HasKey("Id");

                    b.HasIndex("PortalId");

                    b.HasIndex("PostalCodeId");

                    b.HasIndex("UrlTypeId");

                    b.ToTable("Urls");
                });

            modelBuilder.Entity("PropertyCrawler.Data.UrlType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("UrlPortion");

                    b.HasKey("Id");

                    b.ToTable("UrlType");
                });

            modelBuilder.Entity("PropertyCrawler.Data.Image", b =>
                {
                    b.HasOne("PropertyCrawler.Data.Property", "Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PropertyCrawler.Data.Property", b =>
                {
                    b.HasOne("PropertyCrawler.Data.Agent", "Agent")
                        .WithMany("Properties")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PropertyCrawler.Data.PropertyDescription", "PropertyDescription")
                        .WithOne("Property")
                        .HasForeignKey("PropertyCrawler.Data.Property", "PropertyDescriptionId");

                    b.HasOne("PropertyCrawler.Data.Url", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId");
                });

            modelBuilder.Entity("PropertyCrawler.Data.PropertyPrice", b =>
                {
                    b.HasOne("PropertyCrawler.Data.Property", "Property")
                        .WithMany("PropertyPrices")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PropertyCrawler.Data.Url", b =>
                {
                    b.HasOne("PropertyCrawler.Data.Portal", "Portal")
                        .WithMany("Urls")
                        .HasForeignKey("PortalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PropertyCrawler.Data.PostalCode", "PostalCode")
                        .WithMany("Urls")
                        .HasForeignKey("PostalCodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PropertyCrawler.Data.UrlType", "UrlType")
                        .WithMany("Urls")
                        .HasForeignKey("UrlTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
