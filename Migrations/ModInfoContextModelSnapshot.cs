﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SWGEmuModManagerApi.Models;

#nullable disable

namespace SWGEmuModManagerApi.Migrations
{
    [DbContext(typeof(ModInfoContext))]
    partial class ModInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("SWGEmuModManagerApi.Models.ModInfo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Downloads")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ModInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
