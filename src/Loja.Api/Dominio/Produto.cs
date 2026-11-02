namespace Loja.Api.Dominio;

/// <summary>
/// Produto do catálogo. Pode estar em promoção (PrecoPromocional preenchido).
/// </summary>
public sealed class Produto
{
    public Produto(int id, string nome, decimal preco, int estoque, decimal? precoPromocional = null)
    {
        if (preco <= 0)
            throw new ArgumentOutOfRangeException(nameof(preco), "Preço deve ser maior que zero.");
        if (precoPromocional is not null && (precoPromocional <= 0 || precoPromocional >= preco))
            throw new ArgumentOutOfRangeException(nameof(precoPromocional), "Preço promocional deve ser maior que zero e menor que o preço cheio.");
        if (estoque < 0)
            throw new ArgumentOutOfRangeException(nameof(estoque), "Estoque não pode ser negativo.");

        Id = id;
        Nome = nome;
        Preco = preco;
        PrecoPromocional = precoPromocional;
        Estoque = estoque;
    }

    public int Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }
    public decimal? PrecoPromocional { get; }
    public int Estoque { get; private set; }

    public bool EmPromocao => PrecoPromocional is not null;

    /// <summary>Preço que o cliente paga hoje: o promocional, se houver.</summary>
    public decimal PrecoVigente => PrecoPromocional ?? Preco;

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade > Estoque)
            throw new RegraDeNegocioException(
                "estoque-insuficiente",
                $"Estoque insuficiente para '{Nome}': disponível {Estoque}, solicitado {quantidade}.");

        Estoque -= quantidade;
    }
}
