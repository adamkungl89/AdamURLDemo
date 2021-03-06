﻿// <auto-generated />
using AdamURL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdamURL.Migrations
{
    [DbContext(typeof(ShortUrlContext))]
    partial class ShortUrlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdamURL.Models.ShortUrl", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("longUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("ShortUrl");
                });
#pragma warning restore 612, 618
        }
    }
}
