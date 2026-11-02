using Loja.Api.Dominio;

namespace Loja.Tests.Dominio;

public class PedidoTests
{
    private static readonly DateTimeOffset Agora = new(2026, 11, 2, 15, 0, 0, TimeSpan.Zero);

    [Fact]
    public void FecharDe_CarrinhoVazio_LancaCarrinhoVazio()
    {
        var erro = Assert.Throws<RegraDeNegocioException>(() => Pedido.FecharDe(new Carrinho("ana"), 1, Agora));

        Assert.Equal("carrinho-vazio", erro.Codigo);
    }

    [Fact]
    public void FecharDe_CarrinhoComItens_CopiaItensTotaisEDono()
    {
        var carrinho = new Carrinho("ana");
        carrinho.AdicionarItem(new Produto(1, "Teclado", 100m, estoque: 10), 2);
        carrinho.AdicionarItem(new Produto(2, "Mouse", 130m, estoque: 10, precoPromocional: 100m), 1);

        var pedido = Pedido.FecharDe(carrinho, 42, Agora);

        Assert.Equal(42, pedido.Id);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(300m, pedido.Subtotal);
        Assert.Equal(300m, pedido.Total);
        Assert.Equal(Agora, pedido.CriadoEm);
        Assert.True(pedido.PertenceA("ana"));
        Assert.False(pedido.PertenceA("bruno"));
    }
}
