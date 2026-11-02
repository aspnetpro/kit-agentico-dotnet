using Loja.Api.Dominio;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Http;

/// <summary>
/// Traduz exceções do domínio em respostas HTTP (ProblemDetails), num lugar só:
///   RegraDeNegocioException      → 422, com "codigo" no corpo
///   RecursoNaoEncontradoException → 404
/// Qualquer outra exceção segue para o tratamento padrão (500, sem detalhes internos).
/// </summary>
public sealed class TratadorDeErros(IProblemDetailsService problemDetails) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception excecao, CancellationToken cancelamento)
    {
        var problema = excecao switch
        {
            RegraDeNegocioException regra => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Regra de negócio violada",
                Detail = regra.Message,
                Extensions = { ["codigo"] = regra.Codigo },
            },
            RecursoNaoEncontradoException naoEncontrado => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Não encontrado",
                Detail = naoEncontrado.Message,
            },
            _ => null,
        };

        if (problema is null)
            return false;

        http.Response.StatusCode = problema.Status!.Value;
        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = http,
            ProblemDetails = problema,
            Exception = excecao,
        });
    }
}
