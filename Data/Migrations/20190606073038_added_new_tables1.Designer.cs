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
    [Migration("20190606073038_added_new_tables1")]
    partial class added_new_tables1
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

                    b.Property<string>("Address");

                    b.Property<int>("Code");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("LogoUrl");

                    b.Property<string>("Name");

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

                    b.Property<int>("Caption");

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

                    b.HasKey("Id");

                    b.ToTable("PostalCodes");
                });

            modelBuilder.Entity("RightMove.Data.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<int>("AgentId");

                    b.Property<string>("City");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Description");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longtitude");

                    b.Property<byte>("NumberOfBedrooms");

                    b.Property<string>("PostalCode");

                    b.Property<string>("PriceType");

                    b.Property<string>("PropertyType");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.ToTable("Properties");
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

                    b.Property<string>("PropertyUrl");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PortalId");

                    b.HasIndex("PostalCodeId");

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
                });
#pragma warning restore 612, 618
        }
    }
}
