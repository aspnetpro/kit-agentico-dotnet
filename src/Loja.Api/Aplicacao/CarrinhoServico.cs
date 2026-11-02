using Loja.Api.Dominio;
using Loja.Api.Infra;

namespace Loja.Api.Aplicacao;

/// <summary>
/// Orquestra os casos de uso do carrinho: busca o que precisa, chama o domínio, devolve.
/// Não contém regra de negócio: as regras estão em <see cref="Carrinho"/>.
/// </summary>
public sealed class CarrinhoServico(CatalogoEmMemoria catalogo, CarrinhosEmMemoria carrinhos)
{
    public Carrinho Obter(string clienteId) => carrinhos.ObterOuCriar(clienteId);

    public Carrinho AdicionarItem(string clienteId, int produtoId, int quantidade)
    {
        var produto = catalogo.Obter(produtoId);
        var carrinho = carrinhos.ObterOuCriar(clienteId);
        carrinho.AdicionarItem(produto, quantidade);
        return carrinho;
    }

    public Carrinho RemoverItem(string clienteId, int produtoId)
    {
        var carrinho = carrinhos.ObterOuCriar(clienteId);
        carrinho.RemoverItem(produtoId);
        return carrinho;
    }
}
