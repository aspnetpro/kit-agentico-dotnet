namespace Loja.Api.Dominio;

/// <summary>
/// O recurso não existe OU não pertence a quem pediu. Vira HTTP 404.
/// As duas situações devolvem a mesma resposta de propósito: responder 403 para
/// "existe, mas não é seu" já revelaria que o recurso existe.
/// </summary>
public sealed class RecursoNaoEncontradoException(string recurso, object id)
    : Exception($"{recurso} {id} não encontrado.")
{
    public string Recurso { get; } = recurso;
}
