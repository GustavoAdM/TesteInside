using GerenciarPedidos.DTOs;
using GerenciarPedidos.Models;

namespace GerenciarPedidos.Services.ItemPedido
{
    public interface ItemPedidoInterface
    {
        //So cria um novo pedido quando um item ser adicionado
        Task<ResponseModel<List<ItemPedidoModel>>> GerarPedidoComItem(ItemPedidoNomeDTO itemPedidoNomeDTO);
        Task<ResponseModel<ItemPedidoModel>> AdicionarItem(int IdPedido, string NomeItem);
        Task<ResponseModel<ItemPedidoModel>> RemoverItem(int IdPedido, int IdItem);
    }
}
