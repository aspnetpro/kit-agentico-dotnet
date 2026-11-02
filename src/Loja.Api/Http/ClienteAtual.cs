namespace Loja.Api.Http;

/// <summary>
/// AUTENTICAÇÃO SIMULADA. O cliente é identificado pelo cabeçalho X-Cliente-Id.
/// Num sistema real, o id viria do token (claim "sub" de um JWT validado), nunca de um
/// cabeçalho que o próprio cliente escolhe. A simplificação existe só para o kit rodar
/// sem servidor de identidade; o resto do código trata o id como se fosse confiável.
/// </summary>
public static class ClienteAtual
{
    public const string Cabecalho = "X-Cliente-Id";
    private const string ChaveItems = "Loja.ClienteId";

    /// <summary>Exige cliente identificado em todas as rotas do grupo (401 se faltar).</summary>
    public static RouteGroupBuilder ExigirCliente(this RouteGroupBuilder grupo) =>
        grupo.AddEndpointFilter(async (contexto, proximo) =>
        {
            var clienteId = contexto.HttpContext.Request.Headers[Cabecalho].ToString().Trim();
            if (string.IsNullOrEmpty(clienteId))
                return Results.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Cliente não identificado",
                    detail: $"Envie o cabeçalho {Cabecalho}.");

            contexto.HttpContext.Items[ChaveItems] = clienteId;
            return await proximo(contexto);
        });

    /// <summary>Id do cliente da requisição. Só use em rotas protegidas por <see cref="ExigirCliente"/>.</summary>
    public static string ClienteId(this HttpContext http) =>
        http.Items[ChaveItems] as string
        ?? throw new InvalidOperationException("Rota sem ExigirCliente(): não há cliente identificado.");
}
