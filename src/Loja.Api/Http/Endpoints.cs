using Loja.Api.Aplicacao;
using Loja.Api.Infra;

namespace Loja.Api.Http;

/// <summary>
/// Endpoints finos: leem a requisição, chamam o serviço, convertem para o contrato.
/// Nenhuma regra de negócio aqui.
/// </summary>
public static class Endpoints
{
    public static void MapearLoja(this IEndpointRouteBuilder app)
    {
        // Catálogo: público
        app.MapGet("/produtos", (CatalogoEmMemoria catalogo) =>
            catalogo.Listar().Select(ProdutoResposta.De));

        // Carrinho: exige cliente identificado
        var carrinho = app.MapGroup("/carrinho").ExigirCliente();

        carrinho.MapGet("/", (HttpContext http, CarrinhoServico servico) =>
            CarrinhoResposta.De(servico.Obter(http.ClienteId())));

        carrinho.MapPost("/itens", (AdicionarItemRequisicao req, HttpContext http, CarrinhoServico servico) =>
            CarrinhoResposta.De(servico.AdicionarItem(http.ClienteId(), req.ProdutoId, req.Quantidade)));

        carrinho.MapDelete("/itens/{produtoId:int}", (int produtoId, HttpContext http, CarrinhoServico servico) =>
            CarrinhoResposta.De(servico.RemoverItem(http.ClienteId(), produtoId)));

        // Pedidos: exige cliente identificado e só mostra pedidos do próprio cliente
        var pedidos = app.MapGroup("/pedidos").ExigirCliente();

        pedidos.MapPost("/", (HttpContext http, PedidoServico servico) =>
        {
            var pedido = servico.Fechar(http.ClienteId());
            return Results.Created($"/pedidos/{pedido.Id}", PedidoResposta.De(pedido));
        });

        pedidos.MapGet("/", (HttpContext http, PedidoServico servico) =>
            servico.ListarDoCliente(http.ClienteId()).Select(PedidoResposta.De));

        pedidos.MapGet("/{id:int}", (int id, HttpContext http, PedidoServico servico) =>
            PedidoResposta.De(servico.ObterDoCliente(id, http.ClienteId())));
    }
}
