using GerenciarPedidos.Data;
using GerenciarPedidos.DTOs;
using GerenciarPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciarPedidos.Services.ItemPedido
{
    public class ItemPedidoService : ItemPedidoInterface
    {
        private readonly AppDbContext _context;
        public ItemPedidoService(AppDbContext context)
        {
            _context = context; // Usar o _context para poder ter acesso
        }

        public async Task<ResponseModel<ItemPedidoModel>> AdicionarItem(int IdPedido, string NomeItem)
        {
            ResponseModel<ItemPedidoModel> resposta = new ResponseModel<ItemPedidoModel>();

            try
            {
                var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

                if (pedido == null)
                {
                    resposta.Mensagem = "Pedido não localizado";
                    resposta.Status = false;
                    return resposta;
                }

                if (pedido.StatusPedido == 'F') 
                {
                    resposta.Mensagem = "Pedido está fechado e não é possível adicionar um novo item.";
                    resposta.Status = false;
                    return resposta;
                }

                var AdicionarItem = new ItemPedidoModel
                {
                    IdPedido = IdPedido,
                    NomeItem = NomeItem,
                    Pedido = pedido
                };

                // Adicione o item ao contexto e salve as mudanças
                _context.ItemPedido.Add(AdicionarItem);
                await _context.SaveChangesAsync();

               
                resposta.Status = true;
                resposta.Mensagem = "Item adicionado com sucesso.";

                return resposta;

            }
            catch (Exception ex) 
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<ItemPedidoModel>>> GerarPedidoComItem(ItemPedidoNomeDTO itemPedidoNomeDTO)
        {
            ResponseModel<List<ItemPedidoModel>> resposta = new ResponseModel<List<ItemPedidoModel>>();

            try
            {

                // Verifique se há itens informados antes de gerar o pedido
                if (itemPedidoNomeDTO == null || string.IsNullOrEmpty(itemPedidoNomeDTO.NomeItem))
                {
                    resposta.Mensagem = "Nenhum item informado.";
                    resposta.Status = false;
                    return resposta;
                }

                var novoPedido = new PedidoModel
                {
                    StatusPedido = 'A',
                    Items = new List<ItemPedidoModel>()
                };
                
                // Adicione o pedido ao contexto e salve as mudanças para gerar o IdPedido
                _context.Pedido.Add(novoPedido);
                await _context.SaveChangesAsync();

                //Pegar o IdPedido que foi gerado
                int idPedido = novoPedido.IdPedido;

                // Crie o item associado ao pedido
                var itemProduto = new ItemPedidoModel
                {
                    IdPedido = idPedido,
                    NomeItem = itemPedidoNomeDTO.NomeItem,
                    Pedido = novoPedido
                };

                _context.ItemPedido.Add(itemProduto);
                await _context.SaveChangesAsync();

                //resposta.Dados = await _context.ItemPedido.ToListAsync();
                resposta.Mensagem = "Adicionado Produto";
                resposta.Status = true;  
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;

            }

        }

        public async Task<ResponseModel<ItemPedidoModel>> RemoverItem(int IdPedido, int IdItem)
        {
            ResponseModel<ItemPedidoModel> resposta = new ResponseModel<ItemPedidoModel>();
            try
            {
                var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

                if (pedido == null)
                {
                    resposta.Mensagem = "Pedido não localizado";
                    resposta.Status = false;
                    return resposta;
                }

                if (pedido.StatusPedido == 'F')
                {
                    resposta.Mensagem = "Pedido está fechado e não é possível adicionar um novo item.";
                    resposta.Status = false;
                    return resposta;
                }

                var itemPedido = await _context.ItemPedido.FirstOrDefaultAsync(p => p.IdPedido == IdPedido && p.Id == IdItem);

                if (itemPedido == null)
                {
                    resposta.Mensagem = "Item não localizado no pedido.";
                    resposta.Status = false;
                    return resposta;
                }

                //Remove o item do pedido
                _context.ItemPedido.Remove(itemPedido);
                await _context.SaveChangesAsync();

             
                resposta.Status = true;
                resposta.Mensagem = "Item removido com sucesso.";

                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}
