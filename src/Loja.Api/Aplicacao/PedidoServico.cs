using Loja.Api.Dominio;
using Loja.Api.Infra;

namespace Loja.Api.Aplicacao;

public sealed class PedidoServico(
    CatalogoEmMemoria catalogo,
    CarrinhosEmMemoria carrinhos,
    PedidosEmMemoria pedidos,
    TimeProvider relogio)
{
    public Pedido Fechar(string clienteId)
    {
        var carrinho = carrinhos.ObterOuCriar(clienteId);
        var pedido = Pedido.FecharDe(carrinho, pedidos.ProximoId(), relogio.GetUtcNow());

        // Confere o estoque de todos os itens antes de baixar qualquer um,
        // para não deixar o estoque pela metade se um item falhar.
        var baixas = pedido.Itens.Select(i => (Produto: catalogo.Obter(i.ProdutoId), i.Quantidade)).ToList();
        var semEstoque = baixas.FirstOrDefault(b => b.Quantidade > b.Produto.Estoque);
        if (semEstoque.Produto is not null)
            throw new RegraDeNegocioException(
                "estoque-insuficiente",
                $"Estoque insuficiente para '{semEstoque.Produto.Nome}': disponível {semEstoque.Produto.Estoque}.");

        foreach (var (produto, quantidade) in baixas)
            produto.BaixarEstoque(quantidade);

        pedidos.Adicionar(pedido);
        carrinho.Limpar();
        return pedido;
    }

    /// <summary>
    /// Devolve o pedido somente se ele pertencer ao cliente.
    /// AUTORIZAÇÃO POR RECURSO: estar autenticado não basta; o pedido precisa ser seu.
    /// Pedido de outro cliente responde igual a pedido inexistente (404).
    /// </summary>
    public Pedido ObterDoCliente(int pedidoId, string clienteId)
    {
        var pedido = pedidos.Obter(pedidoId);
        if (pedido is null || !pedido.PertenceA(clienteId))
            throw new RecursoNaoEncontradoException("Pedido", pedidoId);

        return pedido;
    }

    public IReadOnlyList<Pedido> ListarDoCliente(string clienteId) => pedidos.ListarDoCliente(clienteId);
}
