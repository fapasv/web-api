﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webapi.Data;

#nullable disable

namespace webapi.Migrations.libreria
{
    [DbContext(typeof(libreriaContext))]
    partial class libreriaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("webapi.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AffectedColumns")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OldValues")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrimaryKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("webapi.Models.Ejercicio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Enunciado")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("enunciado");

                    b.Property<int>("LibroId")
                        .HasColumnType("integer")
                        .HasColumnName("id_libro");

                    b.HasKey("Id")
                        .HasName("pk_ejercicio");

                    b.HasIndex("LibroId");

                    b.ToTable("ejercicios", (string)null);
                });

            modelBuilder.Entity("webapi.Models.Libro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("titulo");

                    b.HasKey("Id")
                        .HasName("pk_libro");

                    b.ToTable("libros", (string)null);
                });

            modelBuilder.Entity("webapi.Models.Respuesta", b =>
                {
                    b.Property<int>("IdEjercicio")
                        .HasColumnType("integer")
                        .HasColumnName("id_ejercicio");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer")
                        .HasColumnName("id_usuario");

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("valor");

                    b.HasKey("IdEjercicio", "IdUsuario")
                        .HasName("pk_ejercicio_usuario");

                    b.ToTable("respuesta", (string)null);
                });

            modelBuilder.Entity("webapi.Models.Ejercicio", b =>
                {
                    b.HasOne("webapi.Models.Libro", "Libro")
                        .WithMany("Ejercicios")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_libro");

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("webapi.Models.Respuesta", b =>
                {
                    b.HasOne("webapi.Models.Ejercicio", "Ejercicio")
                        .WithMany("Respuestas")
                        .HasForeignKey("IdEjercicio")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Ejercicio");
                });

            modelBuilder.Entity("webapi.Models.Ejercicio", b =>
                {
                    b.Navigation("Respuestas");
                });

            modelBuilder.Entity("webapi.Models.Libro", b =>
                {
                    b.Navigation("Ejercicios");
                });
#pragma warning restore 612, 618
        }
    }
}
