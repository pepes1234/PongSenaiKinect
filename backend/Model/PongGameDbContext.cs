using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Model;

public partial class PongGameDbContext : DbContext
{
    public PongGameDbContext()
    {
    }

    public PongGameDbContext(DbContextOptions<PongGameDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Score> Scores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HFCS9QS\\SQLEXPRESS;Initial Catalog=PongGameDB;Integrated Security=SSPI;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Score__3213E83F43E04EBE");

            entity.ToTable("Score");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Player1NicknameNavigation).WithMany(p => p.ScorePlayer1NicknameNavigations)
                .HasForeignKey(d => d.Player1Nickname)
                .HasConstraintName("FK__Score__Player1Ni__267ABA7A");

            entity.HasOne(d => d.Player2NicknameNavigation).WithMany(p => p.ScorePlayer2NicknameNavigations)
                .HasForeignKey(d => d.Player2Nickname)
                .HasConstraintName("FK__Score__Player2Ni__276EDEB3");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07428F47F8");

            entity.ToTable("Usuario");

            entity.Property(e => e.Nickname)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
