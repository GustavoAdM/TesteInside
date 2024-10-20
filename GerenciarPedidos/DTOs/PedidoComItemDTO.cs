namespace GerenciarPedidos.DTOs
{
    public class PedidoComItemDTO
    {
        public int IdPedido { get; set; }
        public char StatusPedido { get; set; }
        public List<ItemPedidoDTO> Items { get; set; }
    }
}
