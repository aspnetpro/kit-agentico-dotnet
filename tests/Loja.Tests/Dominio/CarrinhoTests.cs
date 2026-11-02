using Loja.Api.Dominio;

namespace Loja.Tests.Dominio;

// Convenção de nome: Metodo_Cenario_ResultadoEsperado.
// Os testes marcados com [Spec] cobrem cenários de docs/specs/limite-por-item.md.
public class CarrinhoTests
{
    private static Produto Teclado(int estoque = 20) => new(1, "Teclado", 100m, estoque);
    private static Produto MouseEmPromocao() => new(2, "Mouse", 130m, estoque: 50, precoPromocional: 100m);

    [Fact]
    public void AdicionarItem_ProdutoValido_SomaNoSubtotal()
    {
        var carrinho = new Carrinho("ana");

        carrinho.AdicionarItem(Teclado(), 2);

        Assert.Equal(200m, carrinho.Subtotal);
        Assert.Equal(200m, carrinho.Total);
    }

    [Fact]
    public void AdicionarItem_MesmoProdutoDuasVezes_AcumulaNoMesmoItem()
    {
        var carrinho = new Carrinho("ana");
        var teclado = Teclado();

        carrinho.AdicionarItem(teclado, 2);
        carrinho.AdicionarItem(teclado, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void AdicionarItem_ProdutoEmPromocao_UsaPrecoPromocionalEMarcaItem()
    {
        var carrinho = new Carrinho("ana");

        carrinho.AdicionarItem(MouseEmPromocao(), 1);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(100m, item.PrecoUnitario);
        Assert.True(item.EmPromocao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AdicionarItem_QuantidadeNaoPositiva_LancaQuantidadeInvalida(int quantidade)
    {
        var carrinho = new Carrinho("ana");

        var erro = Assert.Throws<RegraDeNegocioException>(() => carrinho.AdicionarItem(Teclado(), quantidade));

        Assert.Equal("quantidade-invalida", erro.Codigo);
    }

    [Fact] // [Spec] RN01 - Cenário: adicionar exatamente o limite
    public void AdicionarItem_ExatamenteNoLimite_Permite()
    {
        var carrinho = new Carrinho("ana");

        carrinho.AdicionarItem(Teclado(), Carrinho.MaximoUnidadesPorItem);

        Assert.Equal(Carrinho.MaximoUnidadesPorItem, Assert.Single(carrinho.Itens).Quantidade);
    }

    [Fact] // [Spec] RN01 - Cenário: passar do limite numa adição só
    public void AdicionarItem_AcimaDoLimite_LancaLimitePorItem()
    {
        var carrinho = new Carrinho("ana");

        var erro = Assert.Throws<RegraDeNegocioException>(
            () => carrinho.AdicionarItem(Teclado(), Carrinho.MaximoUnidadesPorItem + 1));

        Assert.Equal("limite-por-item", erro.Codigo);
        Assert.True(carrinho.EstaVazio);
    }

    [Fact] // [Spec] RN02 - Cenário: passar do limite somando com o que já está no carrinho
    public void AdicionarItem_SomaUltrapassaLimite_LancaLimitePorItemEMantemQuantidade()
    {
        var carrinho = new Carrinho("ana");
        var teclado = Teclado();
        carrinho.AdicionarItem(teclado, 8);

        var erro = Assert.Throws<RegraDeNegocioException>(() => carrinho.AdicionarItem(teclado, 3));

        Assert.Equal("limite-por-item", erro.Codigo);
        Assert.Equal(8, Assert.Single(carrinho.Itens).Quantidade);
    }

    [Fact] // [Spec] RN03 - Cenário: o limite vale por produto, não por carrinho
    public void AdicionarItem_ProdutosDiferentes_LimiteValePorProduto()
    {
        var carrinho = new Carrinho("ana");

        carrinho.AdicionarItem(Teclado(), 10);
        carrinho.AdicionarItem(MouseEmPromocao(), 10);

        Assert.Equal(2, carrinho.Itens.Count);
    }

    [Fact]
    public void AdicionarItem_SemEstoqueSuficiente_LancaEstoqueInsuficiente()
    {
        var carrinho = new Carrinho("ana");

        var erro = Assert.Throws<RegraDeNegocioException>(() => carrinho.AdicionarItem(Teclado(estoque: 2), 3));

        Assert.Equal("estoque-insuficiente", erro.Codigo);
    }

    [Fact]
    public void RemoverItem_ItemExistente_TiraDoCarrinho()
    {
        var carrinho = new Carrinho("ana");
        carrinho.AdicionarItem(Teclado(), 1);

        carrinho.RemoverItem(1);

        Assert.True(carrinho.EstaVazio);
        Assert.Equal(0m, carrinho.Total);
    }

    [Fact]
    public void RemoverItem_ItemInexistente_LancaNaoEncontrado()
    {
        var carrinho = new Carrinho("ana");

        Assert.Throws<RecursoNaoEncontradoException>(() => carrinho.RemoverItem(99));
    }
}
