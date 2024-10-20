using System.Collections;
using System.Text.Json.Serialization;

namespace GerenciarPedidos.Models
{
    public class PedidoModel
    {
        //IdPedido Cria um ID unico para cada pedido
        public int IdPedido { get; set; }
        public required char StatusPedido { get; set; } // Status A - Aberto e F - Fechado

        //Coleção de item associado ao pedido.
        public required ICollection<ItemPedidoModel> Items { get; set; }
    }
}
