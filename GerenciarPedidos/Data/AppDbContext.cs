using GerenciarPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciarPedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }

        // Criação das tabelas usando as propriedades da Model;
        public DbSet<PedidoModel> Pedido { get; set; }
        public DbSet<ItemPedidoModel> ItemPedido { get; set; }

        //Definindo PK e FK das tabelas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoModel>()
                .HasKey(p => p.IdPedido); // Configura a chave primária para PedidoModel

            modelBuilder.Entity<ItemPedidoModel>()
                .HasKey(i => i.Id); // Configura a chave primária para ItemPedidoModel

            modelBuilder.Entity<PedidoModel>()
                .HasMany(p => p.Items)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.IdPedido); // Configura a chave estrangeira
        }

    }
}
