using Loja.Api.Dominio;

namespace Loja.Api.Infra;

/// <summary>
/// Catálogo em memória, com dados de exemplo. Num projeto real seria EF Core + banco;
/// aqui fica em memória para o kit rodar sem nenhuma infraestrutura.
/// </summary>
public sealed class CatalogoEmMemoria
{
    private readonly Dictionary<int, Produto> _produtos = new()
    {
        [1] = new Produto(1, "Teclado mecânico", 349.90m, estoque: 20),
        [2] = new Produto(2, "Mouse sem fio", 129.90m, estoque: 50, precoPromocional: 99.90m),
        [3] = new Produto(3, "Monitor 27\"", 1899.00m, estoque: 5),
        [4] = new Produto(4, "Headset", 459.00m, estoque: 15, precoPromocional: 389.00m),
        [5] = new Produto(5, "Webcam Full HD", 279.90m, estoque: 0),
    };

    public IReadOnlyCollection<Produto> Listar() => _produtos.Values;

    public Produto Obter(int id) =>
        _produtos.TryGetValue(id, out var produto)
            ? produto
            : throw new RecursoNaoEncontradoException("Produto", id);
}
