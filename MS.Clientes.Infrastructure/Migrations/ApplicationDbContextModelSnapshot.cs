﻿// <auto-generated />
using MS.Clientes.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MS.Clientes.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("MS.Clientes.Domain.Entities.Cliente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellido")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cuil")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EsEmpleadoBNA")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .HasColumnType("TEXT");

                    b.Property<int>("NroDocumento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PaisOrigen")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoDocumento")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });
#pragma warning restore 612, 618
        }
    }
}
