using System;
using System.Collections.Generic;
using LibraryDesktop.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace LibraryDesktop.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<PriceOffer> PriceOffers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=libreria;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");

            entity.Property(e => e.PublishedOn).HasMaxLength(6);

            entity.HasMany(d => d.TagsTags).WithMany(p => p.BooksBooks)
                .UsingEntity<Dictionary<string, object>>(
                    "BookTag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsTagId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BooksBookId"),
                    j =>
                    {
                        j.HasKey("BooksBookId", "TagsTagId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("BookTag");
                        j.HasIndex(new[] { "TagsTagId" }, "IX_BookTag_TagsTagId");
                    });
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(e => new { e.BookId, e.AuthorId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("BookAuthor");

            entity.HasIndex(e => e.AuthorId, "IX_BookAuthor_AuthorId");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors).HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors).HasForeignKey(d => d.BookId);
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<PriceOffer>(entity =>
        {
            entity.HasKey(e => e.PriceOfferId).HasName("PRIMARY");

            entity.HasIndex(e => e.BookId, "IX_PriceOffers_BookId").IsUnique();

            entity.HasOne(d => d.Book).WithOne(p => p.PriceOffer).HasForeignKey<PriceOffer>(d => d.BookId);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("Review");

            entity.HasIndex(e => e.BookId, "IX_Review_BookId");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews).HasForeignKey(d => d.BookId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PRIMARY");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
