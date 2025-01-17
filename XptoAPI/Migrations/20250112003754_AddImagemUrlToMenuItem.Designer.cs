﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XptoAPI.src.Data;

#nullable disable

namespace XptoAPI.Migrations
{
    [DbContext(typeof(XptoContext))]
    [Migration("20250112003754_AddImagemUrlToMenuItem")]
    partial class AddImagemUrlToMenuItem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("MenuItemPedido", b =>
                {
                    b.Property<int>("ItensId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PedidoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItensId", "PedidoId");

                    b.HasIndex("PedidoId");

                    b.ToTable("PedidoItens", (string)null);
                });

            modelBuilder.Entity("XptoAPI.src.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Tipo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TipoRefeicao")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImagemUrl = "/static/images/cafe.jpg",
                            Nome = "Café",
                            Tipo = 0,
                            TipoRefeicao = 0
                        },
                        new
                        {
                            Id = 2,
                            ImagemUrl = "",
                            Nome = "Refrigerante",
                            Tipo = 0,
                            TipoRefeicao = 1
                        },
                        new
                        {
                            Id = 3,
                            ImagemUrl = "",
                            Nome = "Torrada",
                            Tipo = 1,
                            TipoRefeicao = 0
                        },
                        new
                        {
                            Id = 4,
                            ImagemUrl = "",
                            Nome = "Salada",
                            Tipo = 1,
                            TipoRefeicao = 1
                        },
                        new
                        {
                            Id = 5,
                            ImagemUrl = "",
                            Nome = "Bacon",
                            Tipo = 2,
                            TipoRefeicao = 0
                        },
                        new
                        {
                            Id = 6,
                            ImagemUrl = "",
                            Nome = "Bife",
                            Tipo = 2,
                            TipoRefeicao = 1
                        },
                        new
                        {
                            Id = 7,
                            ImagemUrl = "",
                            Nome = "Bolo",
                            Tipo = 3,
                            TipoRefeicao = 1
                        });
                });

            modelBuilder.Entity("XptoAPI.src.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataHoraPedido")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PedidoCompleto")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TipoRefeicao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UsuarioId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("MenuItemPedido", b =>
                {
                    b.HasOne("XptoAPI.src.Models.MenuItem", null)
                        .WithMany()
                        .HasForeignKey("ItensId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("XptoAPI.src.Models.Pedido", null)
                        .WithMany()
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
