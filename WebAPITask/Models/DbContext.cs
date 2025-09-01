using Microsoft.EntityFrameworkCore;

namespace WebAPITask.Models;

public partial class MainDbContext : DbContext
{
    public MainDbContext() { }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }
    public virtual DbSet<SpResult> SpResults { get; set; }
    public virtual DbSet<UserDto> UserResults { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.n_id).HasName("PK__Usuario__3213E83F3561C769");
            entity.ToTable("users");
            entity.Property(e => e.n_id).HasColumnName("n_id");
            entity.Property(e => e.s_name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_name");
            entity.Property(e => e.s_lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_lastname");
            entity.Property(e => e.s_username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("s_username");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.n_id).HasName("PK__Usuario__3213E83F3561C769");
            entity.ToTable("tasks");
            entity.Property(e => e.n_id).HasColumnName("n_id");
            entity.Property(e => e.s_name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_name");
            entity.Property(e => e.s_lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_lastname");
            entity.Property(e => e.s_title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_title");
            entity.Property(e => e.s_description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_description");
            entity.Property(e => e.d_created)
                .HasColumnType("datetime")
                .HasColumnName("d_created");
        });
        modelBuilder.Entity<UserDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<SpResult>().HasNoKey().ToView(null);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}