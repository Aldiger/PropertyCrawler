﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RightMove;

namespace RightMove.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20190603085040_update-schema-portal")]
    partial class updateschemaportal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RightMove.Portal", b =>
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

                    b.ToTable("Portal");
                });

            modelBuilder.Entity("RightMove.PostalCode", b =>
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

            modelBuilder.Entity("RightMove.Url", b =>
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

                    b.ToTable("Url");
                });

            modelBuilder.Entity("RightMove.Url", b =>
                {
                    b.HasOne("RightMove.Portal")
                        .WithMany("Urls")
                        .HasForeignKey("PortalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RightMove.PostalCode")
                        .WithMany("Urls")
                        .HasForeignKey("PostalCodeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
