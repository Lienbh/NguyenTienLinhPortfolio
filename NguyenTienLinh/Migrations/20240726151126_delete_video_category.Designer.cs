﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NguyenTienLinh.Context;

#nullable disable

namespace NguyenTienLinh.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240726151126_delete_video_category")]
    partial class delete_video_category
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NguyenTienLinh.Models.About", b =>
                {
                    b.Property<int>("IdAbout")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAbout"));

                    b.Property<string>("AboutImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdAbout");

                    b.ToTable("About");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.BackGround", b =>
                {
                    b.Property<int>("IdBackGround")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBackGround"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdBackGround");

                    b.ToTable("BackGround");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.Categories", b =>
                {
                    b.Property<int>("IdCategories")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategories"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategories");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUser");

                    b.ToTable("User");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.Videos", b =>
                {
                    b.Property<int>("IdVideo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdVideo"));

                    b.Property<int>("CategoriesIdCategories")
                        .HasColumnType("int");

                    b.Property<int>("IdCategories")
                        .HasColumnType("int");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoLinks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdVideo");

                    b.HasIndex("CategoriesIdCategories");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.Videos", b =>
                {
                    b.HasOne("NguyenTienLinh.Models.Categories", "Categories")
                        .WithMany("Videos")
                        .HasForeignKey("CategoriesIdCategories")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("NguyenTienLinh.Models.Categories", b =>
                {
                    b.Navigation("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
