﻿using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Models;

namespace XptoAPI.src.Data
{
    public class XptoContext : DbContext
    {
        public XptoContext(DbContextOptions<XptoContext> options) : base(options)
        {
        }

        public DbSet<MenuItem>? MenuItems { get; set; }
        public DbSet<Pedido>? Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais para as entidades
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Nome = "Café", Tipo = TipodeItemMenu.Bebida, TipoRefeicao = TipoRefeicao.CafedaManha },
                new MenuItem { Id = 2, Nome = "Refrigerante", Tipo = TipodeItemMenu.Bebida, TipoRefeicao = TipoRefeicao.Almoco },
                new MenuItem { Id = 3, Nome = "Torrada", Tipo = TipodeItemMenu.Acompanhamento, TipoRefeicao = TipoRefeicao.CafedaManha },
                new MenuItem { Id = 4, Nome = "Salada", Tipo = TipodeItemMenu.Acompanhamento, TipoRefeicao = TipoRefeicao.Almoco },
                new MenuItem { Id = 5, Nome = "Bacon", Tipo = TipodeItemMenu.PratoPrincipal, TipoRefeicao = TipoRefeicao.CafedaManha },
                new MenuItem { Id = 6, Nome = "Bife", Tipo = TipodeItemMenu.PratoPrincipal, TipoRefeicao = TipoRefeicao.Almoco },
                new MenuItem { Id = 7, Nome = "Bolo", Tipo = TipodeItemMenu.Sobremesa, TipoRefeicao = TipoRefeicao.Almoco }
            );

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithMany()
                .UsingEntity(j => j.ToTable("PedidoItens"));
        }
    }
}
