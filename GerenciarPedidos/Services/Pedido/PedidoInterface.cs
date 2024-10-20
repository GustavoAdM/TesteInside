using GerenciarPedidos.Models;
using GerenciarPedidos.DTOs;

namespace GerenciarPedidos.Services.Pedido
{
    public interface PedidoInterface
    {
        Task<ResponseModel<List<PedidoDTO>>> ListarPedidos(char FiltrarStatus, int pageNumber, int pageSize);
        Task<ResponseModel<PedidoComItemDTO>> BuscarItemPedido(int IdPedido);
        Task<ResponseModel<PedidoDTO>> FecharPedido(int IdPedido);
    }
}
