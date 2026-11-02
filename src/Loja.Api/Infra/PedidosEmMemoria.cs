using System.Collections.Concurrent;
using Loja.Api.Dominio;

namespace Loja.Api.Infra;

public sealed class PedidosEmMemoria
{
    private readonly ConcurrentDictionary<int, Pedido> _pedidos = new();
    private int _ultimoId;

    public int ProximoId() => Interlocked.Increment(ref _ultimoId);

    public void Adicionar(Pedido pedido) => _pedidos[pedido.Id] = pedido;

    public Pedido? Obter(int id) => _pedidos.GetValueOrDefault(id);

    public IReadOnlyList<Pedido> ListarDoCliente(string clienteId) =>
        _pedidos.Values
            .Where(p => p.PertenceA(clienteId))
            .OrderByDescending(p => p.CriadoEm)
            .ToList();
}
