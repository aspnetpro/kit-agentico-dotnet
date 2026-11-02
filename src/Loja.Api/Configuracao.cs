using Loja.Api.Http;
using Loja.Api.Aplicacao;
using Loja.Api.Infra;

namespace Loja.Api;

/// <summary>Registro de dependências e pipeline HTTP, num lugar só.</summary>
public static class Configuracao
{
    public static IServiceCollection AdicionarLoja(this IServiceCollection services)
    {
        // Infra em memória: singletons para os dados sobreviverem entre requisições.
        services.AddSingleton<CatalogoEmMemoria>();
        services.AddSingleton<CarrinhosEmMemoria>();
        services.AddSingleton<PedidosEmMemoria>();

        // Relógio injetável: regras que dependem de data (validade, expiração) usam
        // TimeProvider, nunca DateTime.Now, para serem testáveis.
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<CarrinhoServico>();
        services.AddScoped<PedidoServico>();

        services.AddProblemDetails();
        services.AddExceptionHandler<TratadorDeErros>();
        return services;
    }

    public static WebApplication UsarLoja(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.MapearLoja();
        return app;
    }
}
