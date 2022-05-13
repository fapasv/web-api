
namespace webapi.Data
{
    public partial class membresiaContext : DbContext
    {
        public membresiaContext()
        {
        }

        public membresiaContext(DbContextOptions<membresiaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<RolPermiso> RolPermisos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_usuario");

                entity.ToTable("usuario");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("character varying")
                    .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExpiracion)
                    .HasColumnType("timestamp")
                    .HasColumnName("refresh_token_expiracion");
            });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdRol }).HasName("pk_usuario_rol");

                entity.ToTable("usuario_rol");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario");

                entity.Property(e => e.IdRol)
                   .HasColumnName("id_rol");

                entity.HasOne(e => e.UsuarioAsociado)
                    .WithMany(u => u.UsuarioRoles)
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.RolAsociado)
                    .WithMany(r => r.RolUsuarios)
                    .HasForeignKey(e => e.IdRol)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_rol");

                entity.ToTable("rol");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.HasKey(e => new { e.IdRol, e.IdPermiso });

                entity.ToTable("rol_permiso");

                entity.Property(e => e.IdRol)
                    .HasColumnName("id_rol");

                entity.Property(e => e.IdPermiso)
                   .HasColumnName("id_permiso");

                entity.HasOne(e => e.RolAsociado)
                    .WithMany(r => r.RolPermisos)
                    .HasForeignKey(e => e.IdRol)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                entity.HasOne(e => e.PermisoAsociado)
                    .WithMany(p => p.PermisoRoles)
                    .HasForeignKey(e => e.IdPermiso)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_permiso");

                entity.ToTable("permiso");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Controlador)
                    .HasColumnType("character varying")
                    .HasColumnName("controlador");

                entity.Property(e => e.Accion)
                    .HasColumnType("character varying")
                    .HasColumnName("accion");

                entity.Property(e => e.Tipo)
                   .HasColumnType("character varying")
                   .HasColumnName("tipo")
                   .IsRequired(false);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}