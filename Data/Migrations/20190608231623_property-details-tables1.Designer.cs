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
    [Migration("20190608231623_property-details-tables1")]
    partial class propertydetailstables1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RightMove.Data.Agent", b =>
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

            modelBuilder.Entity("RightMove.Data.Image", b =>
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

            modelBuilder.Entity("RightMove.Data.Portal", b =>
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

            modelBuilder.Entity("RightMove.Data.PostalCode", b =>
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

            modelBuilder.Entity("RightMove.Data.Property", b =>
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

                    b.Property<int?>("PropertyDescriptionId");

                    b.Property<string>("PropertySubType");

                    b.Property<string>("PropertyType");

                    b.Property<int?>("UrlId");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("PropertyDescriptionId");

                    b.HasIndex("UrlId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("RightMove.Data.PropertyDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description");

                    b.Property<int>("PropertyId");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyDescriptions");
                });

            modelBuilder.Entity("RightMove.Data.PropertyPrice", b =>
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

            modelBuilder.Entity("RightMove.Data.Url", b =>
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

                    b.Property<int?>("PropertyId");

                    b.Property<string>("PropertyUrl");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PortalId");

                    b.HasIndex("PostalCodeId");

                    b.HasIndex("PropertyId");

                    b.ToTable("Urls");
                });

            modelBuilder.Entity("RightMove.Data.Image", b =>
                {
                    b.HasOne("RightMove.Data.Property", "Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RightMove.Data.Property", b =>
                {
                    b.HasOne("RightMove.Data.Agent", "Agent")
                        .WithMany("Properties")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RightMove.Data.PropertyDescription", "PropertyDescription")
                        .WithMany()
                        .HasForeignKey("PropertyDescriptionId");

                    b.HasOne("RightMove.Data.Url", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId");
                });

            modelBuilder.Entity("RightMove.Data.PropertyDescription", b =>
                {
                    b.HasOne("RightMove.Data.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RightMove.Data.PropertyPrice", b =>
                {
                    b.HasOne("RightMove.Data.Property", "Property")
                        .WithMany("PropertyPrices")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RightMove.Data.Url", b =>
                {
                    b.HasOne("RightMove.Data.Portal", "Portal")
                        .WithMany("Urls")
                        .HasForeignKey("PortalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RightMove.Data.PostalCode", "PostalCode")
                        .WithMany("Urls")
                        .HasForeignKey("PostalCodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RightMove.Data.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId");
                });
#pragma warning restore 612, 618
        }
    }
}
