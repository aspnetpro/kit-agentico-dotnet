using Loja.Api.Dominio;

namespace Loja.Api.Http;

// Contratos HTTP (o que entra e o que sai da API). O domínio nunca é serializado direto:
// assim dá para mudar o domínio sem quebrar quem consome a API.

public sealed record AdicionarItemRequisicao(int ProdutoId, int Quantidade);

public sealed record ProdutoResposta(
    int Id, string Nome, decimal Preco, decimal? PrecoPromocional, decimal PrecoVigente, bool EmPromocao, int Estoque)
{
    public static ProdutoResposta De(Produto p) =>
        new(p.Id, p.Nome, p.Preco, p.PrecoPromocional, p.PrecoVigente, p.EmPromocao, p.Estoque);
}

public sealed record ItemResposta(
    int ProdutoId, string Nome, decimal PrecoUnitario, bool EmPromocao, int Quantidade, decimal Total);

public sealed record CarrinhoResposta(IReadOnlyList<ItemResposta> Itens, decimal Subtotal, decimal Total)
{
    public static CarrinhoResposta De(Carrinho c) =>
        new(
            c.Itens.Select(i => new ItemResposta(i.ProdutoId, i.NomeProduto, i.PrecoUnitario, i.EmPromocao, i.Quantidade, i.Total)).ToList(),
            c.Subtotal,
            c.Total);
}

public sealed record PedidoResposta(
    int Id, IReadOnlyList<ItemResposta> Itens, decimal Subtotal, decimal Total, DateTimeOffset CriadoEm)
{
    public static PedidoResposta De(Pedido p) =>
        new(
            p.Id,
            p.Itens.Select(i => new ItemResposta(i.ProdutoId, i.NomeProduto, i.PrecoUnitario, i.EmPromocao, i.Quantidade, i.Total)).ToList(),
            p.Subtotal,
            p.Total,
            p.CriadoEm);
}
