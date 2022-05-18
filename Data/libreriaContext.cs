
namespace webapi.Data
{
    public partial class libreriaContext :AuditableIdentityContext
    {
        public libreriaContext()
        {
        }

        public libreriaContext(DbContextOptions<libreriaContext> options)
            : base(options)
        {
        }
       
        public virtual DbSet<Libro> Libros { get; set; } = null!;
        public virtual DbSet<Ejercicio> Ejercicios { get; set; } = null!;
        public virtual DbSet<Respuesta> Respuestas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => new { e.IdEjercicio, e.IdUsuario }).HasName("pk_ejercicio_usuario");

                entity.ToTable("respuesta");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario");

                entity.Property(e => e.IdEjercicio)
                   .HasColumnName("id_ejercicio");

                entity.Property(e => e.Valor)
                    .HasColumnType("character varying")
                    .HasColumnName("valor");

                entity.HasOne(d => d.EjercicioAsociado)
                    .WithMany(p => p.Respuestas)
                    .HasForeignKey(d => d.IdEjercicio)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ejercicio>(entity =>
            {
                entity.ToTable("ejercicios");

                entity.HasIndex(e => e.IdLibro, "IX_ejercicios_id_libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Enunciado)
                    .HasColumnType("character varying")
                    .HasColumnName("enunciado");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");

                entity.HasOne(d => d.LibroAsociado)
                    .WithMany(p => p.Ejercicios)
                    .HasForeignKey(d => d.IdLibro)
                    .HasConstraintName("fk_libro");
            });
           
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("libros");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
