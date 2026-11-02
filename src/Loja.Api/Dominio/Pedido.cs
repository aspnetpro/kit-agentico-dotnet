namespace Loja.Api.Dominio;

/// <summary>Item congelado no momento do fechamento do pedido.</summary>
public sealed record ItemPedido(
    int ProdutoId,
    string NomeProduto,
    decimal PrecoUnitario,
    bool EmPromocao,
    int Quantidade)
{
    public decimal Total => PrecoUnitario * Quantidade;
}

/// <summary>
/// Pedido fechado a partir de um carrinho. Imutável depois de criado.
/// </summary>
public sealed class Pedido
{
    private Pedido(int id, string clienteId, IReadOnlyList<ItemPedido> itens, decimal subtotal, decimal total, DateTimeOffset criadoEm)
    {
        Id = id;
        ClienteId = clienteId;
        Itens = itens;
        Subtotal = subtotal;
        Total = total;
        CriadoEm = criadoEm;
    }

    public int Id { get; }
    public string ClienteId { get; }
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal Subtotal { get; }
    public decimal Total { get; }
    public DateTimeOffset CriadoEm { get; }

    public bool PertenceA(string clienteId) => ClienteId == clienteId;

    public static Pedido FecharDe(Carrinho carrinho, int id, DateTimeOffset agora)
    {
        if (carrinho.EstaVazio)
            throw new RegraDeNegocioException("carrinho-vazio", "Não é possível fechar um pedido com o carrinho vazio.");

        var itens = carrinho.Itens
            .Select(i => new ItemPedido(i.ProdutoId, i.NomeProduto, i.PrecoUnitario, i.EmPromocao, i.Quantidade))
            .ToList();

        return new Pedido(id, carrinho.ClienteId, itens, carrinho.Subtotal, carrinho.Total, agora);
    }
}
