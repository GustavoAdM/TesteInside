using Azure;
using GerenciarPedidos.Models;
using GerenciarPedidos.DTOs;
using GerenciarPedidos.Services.Pedido;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GerenciarPedidos.Services.ItemPedido;

namespace GerenciarPedidos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoInterface _pedidointerface;
        private readonly ItemPedidoInterface _itempedidointerface;
        public PedidoController(PedidoInterface pedidoInterface, ItemPedidoInterface itemPedidoInterface)
        {
            _pedidointerface = pedidoInterface;
            _itempedidointerface = itemPedidoInterface;

        }


        [HttpGet("ListarPedidos")]
        public async Task<ActionResult<ResponseModel<List<PedidoModel>>>> ListarPedido([FromQuery] char? Filtro = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pedidos = await _pedidointerface.ListarPedidos(Filtro ?? '\0', pageNumber, pageSize);
            return Ok(pedidos);
        }

        [HttpGet("BuscarItemPedido/{IdPedido}")]
        public async Task<ActionResult<ResponseModel<PedidoComItemDTO>>> BuscarItemPedido(int IdPedido)
        {
            var ItemPedido = await _pedidointerface.BuscarItemPedido(IdPedido);
            return Ok(ItemPedido);

        }

        [HttpPost("GerarPedido")]
        public async Task<ActionResult<ResponseModel<PedidoComItemDTO>>> GerarPedidoComItem(ItemPedidoNomeDTO itemPedidoNomeDTO)
        {
            var itemspedido = await _itempedidointerface.GerarPedidoComItem(itemPedidoNomeDTO);
            return Ok(itemspedido);

        }

        [HttpPut("FecharPedido/{IdPedido}")]
        public async Task<ActionResult<ResponseModel<PedidoModel>>> FecharPedido(int IdPedido)
        {
            var pedidos = await _pedidointerface.FecharPedido(IdPedido);
            return Ok(pedidos); 
        }

        [HttpPut("InserirProduto/{IdPedido}/{NomeItem}")]
        public async Task<ActionResult<ResponseModel<ItemPedidoModel>>> InserirProduto(int IdPedido, string NomeItem)
        {
            var pedidos = await _itempedidointerface.AdicionarItem(IdPedido, NomeItem);
            return Ok(pedidos);
        }

        [HttpPut("RemoverProduto/{IdPedido}/{IdProduto}")]
        public async Task<ActionResult<ResponseModel<ItemPedidoModel>>> RemoverProduto(int IdPedido, int IdProduto)
        {
            var pedidos = await _itempedidointerface.RemoverItem(IdPedido, IdProduto);
            return Ok(pedidos);
        }


    }
}
