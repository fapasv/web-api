
namespace webapi.Models
{
    public partial class libreriaContext : DbContext
    {
        public libreriaContext()
        {
        }

        public libreriaContext(DbContextOptions<libreriaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Autor> Autors { get; set; } = null!;
        public virtual DbSet<Libro> Libros { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("autor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Alias)
                    .HasColumnType("character varying")
                    .HasColumnName("alias");

                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");
            });
           
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AutorId).HasColumnName("autor_id");

                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");

                    entity.Property(e => e.Precio)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("precio");

                entity.Property(e => e.FechaPublicacion)
                    .HasColumnType("date")
                    .HasColumnName("fecha_publicacion");

                entity.HasOne(d => d.Autor)
                    .WithMany(p => p.Libros)
                    .HasForeignKey(d => d.AutorId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fk_autor");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
