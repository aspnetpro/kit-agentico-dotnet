using System.Collections.Concurrent;
using Loja.Api.Dominio;

namespace Loja.Api.Infra;

/// <summary>Um carrinho por cliente, guardado em memória.</summary>
public sealed class CarrinhosEmMemoria
{
    private readonly ConcurrentDictionary<string, Carrinho> _carrinhos = new();

    public Carrinho ObterOuCriar(string clienteId) =>
        _carrinhos.GetOrAdd(clienteId, id => new Carrinho(id));
}
