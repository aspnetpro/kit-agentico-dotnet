using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Loja.Api.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Loja.Tests.Http;

/// <summary>
/// Testes de API: sobem a aplicação de verdade em memória e falam HTTP com ela.
/// Cada teste usa um cliente diferente para não depender da ordem de execução.
/// </summary>
public class ApiTests(WebApplicationFactory<Program> fabrica) : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient ClienteHttp(string? clienteId)
    {
        var http = fabrica.CreateClient();
        if (clienteId is not null)
            http.DefaultRequestHeaders.Add(ClienteAtual.Cabecalho, clienteId);
        return http;
    }

    private static string NovoCliente() => $"cliente-{Guid.NewGuid():N}";

    [Fact]
    public async Task GetProdutos_SemCliente_ListaCatalogo()
    {
        var produtos = await ClienteHttp(null).GetFromJsonAsync<List<ProdutoResposta>>("/produtos");

        Assert.NotNull(produtos);
        Assert.Contains(produtos, p => p.EmPromocao);
    }

    [Fact]
    public async Task GetCarrinho_SemCabecalhoDeCliente_Retorna401()
    {
        var resposta = await ClienteHttp(null).GetAsync("/carrinho");

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);
    }

    [Fact]
    public async Task PostItem_ProdutoValido_RetornaCarrinhoAtualizado()
    {
        var http = ClienteHttp(NovoCliente());

        var resposta = await http.PostAsJsonAsync("/carrinho/itens", new AdicionarItemRequisicao(ProdutoId: 1, Quantidade: 2));

        resposta.EnsureSuccessStatusCode();
        var carrinho = await resposta.Content.ReadFromJsonAsync<CarrinhoResposta>();
        Assert.Equal(699.80m, carrinho!.Total);
    }

    [Fact]
    public async Task PostItem_ProdutoInexistente_Retorna404()
    {
        var http = ClienteHttp(NovoCliente());

        var resposta = await http.PostAsJsonAsync("/carrinho/itens", new AdicionarItemRequisicao(ProdutoId: 999, Quantidade: 1));

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    [Fact] // [Spec] limite-por-item RN01 - a API devolve 422 com o código da regra
    public async Task PostItem_AcimaDoLimite_Retorna422ComCodigo()
    {
        var http = ClienteHttp(NovoCliente());

        var resposta = await http.PostAsJsonAsync("/carrinho/itens", new AdicionarItemRequisicao(ProdutoId: 1, Quantidade: 11));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, resposta.StatusCode);
        Assert.Equal("limite-por-item", await CodigoDoErro(resposta));
    }

    [Fact]
    public async Task PostPedido_CarrinhoComItens_CriaPedidoEEsvaziaCarrinho()
    {
        var http = ClienteHttp(NovoCliente());
        await http.PostAsJsonAsync("/carrinho/itens", new AdicionarItemRequisicao(ProdutoId: 2, Quantidade: 1));

        var resposta = await http.PostAsync("/pedidos", content: null);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        var carrinho = await http.GetFromJsonAsync<CarrinhoResposta>("/carrinho");
        Assert.Empty(carrinho!.Itens);
    }

    [Fact]
    public async Task GetPedido_DoProprioCliente_RetornaPedido()
    {
        var http = ClienteHttp(NovoCliente());
        var pedido = await CriarPedido(http);

        var resposta = await http.GetAsync($"/pedidos/{pedido.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }

    // AUTORIZAÇÃO POR RECURSO (IDOR). Este é o teste que mais falta em código gerado por IA:
    // o endpoint exige login, mas sem este cuidado qualquer cliente logado leria o pedido de outro.
    [Fact]
    public async Task GetPedido_DeOutroCliente_Retorna404()
    {
        var pedidoDaAna = await CriarPedido(ClienteHttp(NovoCliente()));
        var bruno = ClienteHttp(NovoCliente());

        var resposta = await bruno.GetAsync($"/pedidos/{pedidoDaAna.Id}");

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    private static async Task<PedidoResposta> CriarPedido(HttpClient http)
    {
        (await http.PostAsJsonAsync("/carrinho/itens", new AdicionarItemRequisicao(ProdutoId: 1, Quantidade: 1))).EnsureSuccessStatusCode();
        var resposta = await http.PostAsync("/pedidos", content: null);
        resposta.EnsureSuccessStatusCode();
        return (await resposta.Content.ReadFromJsonAsync<PedidoResposta>())!;
    }

    private static async Task<string?> CodigoDoErro(HttpResponseMessage resposta)
    {
        using var json = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        return json.RootElement.TryGetProperty("codigo", out var codigo) ? codigo.GetString() : null;
    }
}
