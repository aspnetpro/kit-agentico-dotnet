namespace Loja.Api.Dominio;

/// <summary>
/// Carrinho de compras de um cliente. Todas as regras do carrinho moram aqui,
/// não nos endpoints nem nos serviços.
/// </summary>
public sealed class Carrinho(string clienteId)
{
    /// <summary>Regra RN01 de docs/specs/limite-por-item.md.</summary>
    public const int MaximoUnidadesPorItem = 10;

    private readonly List<ItemCarrinho> _itens = [];

    public string ClienteId { get; } = clienteId;
    public IReadOnlyList<ItemCarrinho> Itens => _itens;
    public bool EstaVazio => _itens.Count == 0;

    public decimal Subtotal => _itens.Sum(i => i.Total);

    /// <summary>
    /// Valor final a pagar. Hoje é igual ao subtotal porque a loja ainda não tem descontos.
    /// </summary>
    public decimal Total => Subtotal;

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (quantidade <= 0)
            throw new RegraDeNegocioException("quantidade-invalida", "A quantidade deve ser maior que zero.");

        var existente = _itens.FirstOrDefault(i => i.ProdutoId == produto.Id);
        var quantidadeFinal = (existente?.Quantidade ?? 0) + quantidade;

        if (quantidadeFinal > MaximoUnidadesPorItem)
            throw new RegraDeNegocioException(
                "limite-por-item",
                $"Máximo de {MaximoUnidadesPorItem} unidades por produto no carrinho.");

        if (quantidadeFinal > produto.Estoque)
            throw new RegraDeNegocioException(
                "estoque-insuficiente",
                $"Estoque insuficiente para '{produto.Nome}': disponível {produto.Estoque}.");

        if (existente is null)
            _itens.Add(new ItemCarrinho(produto, quantidade));
        else
            existente.Somar(quantidade);
    }

    public void RemoverItem(int produtoId)
    {
        var removidos = _itens.RemoveAll(i => i.ProdutoId == produtoId);
        if (removidos == 0)
            throw new RecursoNaoEncontradoException("Item do carrinho", produtoId);
    }

    public void Limpar() => _itens.Clear();
}
