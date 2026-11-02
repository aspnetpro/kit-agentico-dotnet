using Loja.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AdicionarLoja();

var app = builder.Build();
app.UsarLoja();
app.Run();

// Expõe a classe Program para os testes de API (WebApplicationFactory<Program>).
public partial class Program;
