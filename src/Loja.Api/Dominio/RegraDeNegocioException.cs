namespace Loja.Api.Dominio;

/// <summary>
/// Violação de uma regra de negócio. Vira HTTP 422 com o <see cref="Codigo"/> no corpo.
/// O código é estável (o front-end e os testes dependem dele); a mensagem é para humanos.
/// </summary>
public sealed class RegraDeNegocioException(string codigo, string mensagem) : Exception(mensagem)
{
    public string Codigo { get; } = codigo;
}
