using GerenciarPedidos.Data;
using GerenciarPedidos.DTOs;
using GerenciarPedidos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GerenciarPedidos.Services.Pedido
{
    public class PedidoService : PedidoInterface
    {
        //Acessar o banco de dados
        private readonly AppDbContext _context;
        public PedidoService(AppDbContext context) 
        {
            _context = context; // Usar o _context para poder ter acesso
        }

        public async Task<ResponseModel<PedidoComItemDTO>> BuscarItemPedido(int IdPedido)
        {
            ResponseModel<PedidoComItemDTO> resposta = new ResponseModel<PedidoComItemDTO>();
            try
            {
                var pedidoComItem = await _context.Pedido
                    .Include(p => p.Items)
                    .Where(p => p.IdPedido == IdPedido)
                    .Select(p => new PedidoComItemDTO
                    {
                        IdPedido = p.IdPedido,
                        StatusPedido = p.StatusPedido,
                        Items = p.Items.Select(i => new ItemPedidoDTO
                        {
                            Id = i.Id,
                            NomeItem = i.NomeItem
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();


                if (pedidoComItem == null)
                {
                    resposta.Mensagem = "Pedido não localizado";
                    resposta.Status = false;
                }

                resposta.Dados = pedidoComItem;
                resposta.Mensagem = "Dados do pedido retornado";
                resposta.Status = true;

                return resposta;


            }catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;

            }
        }

        public async Task<ResponseModel<PedidoDTO>> FecharPedido(int IdPedido)
        {
            ResponseModel<PedidoDTO> resposta = new ResponseModel<PedidoDTO>();
            try
            {
                var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);
                var itempedido = await _context.ItemPedido.FirstOrDefaultAsync(p => p.IdPedido == IdPedido);

                if (pedido == null)
                {
                    resposta.Mensagem = "Pedido não encontrado.";
                    resposta.Status = false;
                    return resposta;
                }

                if (itempedido == null)
                {
                    resposta.Mensagem = "Não é possível fechar o pedido. Por favor, adicione pelo menos um produto ao pedido antes de tentar fechá-lo.";
                    resposta.Status = false;
                    return resposta;
                }

                // Atualize o status do pedido para "F" (Fechado)
                pedido.StatusPedido = 'F';

                _context.Pedido.Update(pedido);
                await _context.SaveChangesAsync();

                //resposta.Dados = pedido;
                resposta.Status = true;
                resposta.Mensagem = "Status do pedido atualizado com sucesso.";

                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<PedidoDTO>>> ListarPedidos(char filtrarStatus, int pageNumber, int pageSize)
        {
            ResponseModel<List<PedidoDTO>> resposta = new ResponseModel<List<PedidoDTO>>();
            try
            {
                if (filtrarStatus != '\0' && filtrarStatus != 'A' && filtrarStatus != 'F')
                {
                    resposta.Mensagem = "Status de filtro inválido. Use 'A' para aberto ou 'F' para fechado.";
                    resposta.Status = false;
                    return resposta;
                }

                // Calcule o número de itens a serem pulados
                int skip = (pageNumber - 1) * pageSize;

                List<PedidoDTO> pedidos;

                if (filtrarStatus == '\0')
                {
                    // Listar todos os pedidos
                    pedidos = await _context.Pedido
                        .Skip(skip)
                        .Take(pageSize)
                        .Select(pedido => new PedidoDTO
                            {
                                IdPedido = pedido.IdPedido,
                                StatusPedido = pedido.StatusPedido
                            })
                        .ToListAsync();

                    resposta.Mensagem = "Todos os pedidos listados.";
                }
                else
                {
                    // Filtrar pedidos pelo status fornecido
                    pedidos = await _context.Pedido
                        .Where(p => p.StatusPedido == filtrarStatus)
                        .Skip(skip)
                        .Take(pageSize)
                        .Select(pedido => new PedidoDTO
                        {
                            IdPedido = pedido.IdPedido,
                            StatusPedido = pedido.StatusPedido
                        }).ToListAsync();

                    resposta.Mensagem = $"Pedidos com status '{filtrarStatus}' listados.";
                }

                resposta.Dados = pedidos;
                resposta.Status = true;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
            }

            return resposta;
        }

    }
}
