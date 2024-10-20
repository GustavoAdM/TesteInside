namespace GerenciarPedidos.Models
{
    public class ItemPedidoModel
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public required string NomeItem { get; set; }

        //Propriedade de navegação que representa o pedido ao qual este item está associado.
        public required PedidoModel Pedido { get; set; }

    }
}
