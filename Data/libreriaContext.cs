
namespace webapi.Data
{
    public partial class libreriaContext : AuditableIdentityContext
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

                entity.HasOne(r => r.Ejercicio)
                    .WithMany(e => e.Respuestas)
                    .HasForeignKey(r => r.IdEjercicio)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Ejercicio>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_ejercicio");
                entity.ToTable("ejercicios");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd(); 

                entity.Property(e => e.Enunciado)
                    .HasColumnType("character varying")
                    .HasColumnName("enunciado");

                entity.Property(e => e.IdLibro)
                   .HasColumnName("id_libro");               

                entity.HasOne(d => d.Libro)
                   .WithMany(p => p.Ejercicios)
                   .HasForeignKey(d => d.IdLibro)                   
                   .HasConstraintName("fk_libro");
            });
           
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_libro");

                entity.ToTable("libros");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
               
                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
