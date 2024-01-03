using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ElearningApp.Models
{
    public partial class ElearningContext : DbContext
    {
        public ElearningContext()
        {
        }

        public ElearningContext(DbContextOptions<ElearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAnswer> UserAnswers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=CLEMENT\\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Fullnames)
                    .HasMaxLength(50)
                    .HasColumnName("fullnames");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Upassword).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer)
                    .HasMaxLength(200)
                    .HasColumnName("answer");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Marks).HasColumnName("marks");

                entity.Property(e => e.Question1)
                    .HasMaxLength(1000)
                    .HasColumnName("question");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_question_category");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fullnames)
                    .HasMaxLength(100)
                    .HasColumnName("fullnames");

                entity.Property(e => e.TotalScore).HasColumnName("total_score");
            });

            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.ToTable("user_answers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Marks).HasColumnName("marks");

                entity.Property(e => e.Question).HasColumnName("question");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Users)
                    .HasMaxLength(50)
                    .HasColumnName("users");

                entity.HasOne(d => d.QuestionNavigation)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.Question)
                    .HasConstraintName("FK_user_answers_question");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
