namespace Loja.Api.Dominio;

/// <summary>
/// Item do carrinho. Guarda uma "foto" do preço vigente no momento em que foi adicionado,
/// e se o produto estava em promoção (regras de desconto costumam depender disso).
/// </summary>
public sealed class ItemCarrinho
{
    internal ItemCarrinho(Produto produto, int quantidade)
    {
        ProdutoId = produto.Id;
        NomeProduto = produto.Nome;
        PrecoUnitario = produto.PrecoVigente;
        EmPromocao = produto.EmPromocao;
        Quantidade = quantidade;
    }

    public int ProdutoId { get; }
    public string NomeProduto { get; }
    public decimal PrecoUnitario { get; }
    public bool EmPromocao { get; }
    public int Quantidade { get; private set; }

    public decimal Total => PrecoUnitario * Quantidade;

    internal void Somar(int quantidade) => Quantidade += quantidade;
}
