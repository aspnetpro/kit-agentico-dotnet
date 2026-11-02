# Loja: contexto para o agente

<!--
  POR QUE ESTE ARQUIVO EXISTE
  O Claude Code lê este arquivo no início de toda sessão aberta nesta pasta.
  É a "memória" do projeto: tudo o que você explicaria a um dev novo no primeiro dia.
  Regra de ouro: curto e específico. Cada linha aqui disputa atenção com a sua tarefa.
  Se uma linha não muda o comportamento do agente, ela não deveria estar aqui.
  Os comentários como este explicam o porquê de cada bloco: são para você, aluno.
-->

API de e-commerce didática em ASP.NET Core (.NET 10, Minimal APIs). Carrinho, catálogo e pedidos.
Dados em memória (sem banco) para rodar sem infraestrutura.

## Comandos

<!-- Comandos exatos economizam tentativa e erro: o agente não precisa adivinhar como testar. -->

- Build: `dotnet build`
- Testes: `dotnet test`
- Rodar a API: `dotnet run --project src/Loja.Api` (requisições prontas em `requisicoes.http`)

## Arquitetura (detalhes em docs/arquitetura.md)

<!-- Dizer ONDE cada coisa mora é o que impede o agente de pôr regra de negócio no endpoint. -->

- `src/Loja.Api/Dominio/`: entidades e TODAS as regras de negócio. Regra violada lança `RegraDeNegocioException(codigo, mensagem)`.
- `src/Loja.Api/Aplicacao/`: serviços que orquestram casos de uso. Sem regra de negócio.
- `src/Loja.Api/Infra/`: armazenamento em memória.
- `src/Loja.Api/Http/`: endpoints finos, contratos (records `*Requisicao`/`*Resposta`), autenticação simulada e tradução de erros.
- Endpoints nunca expõem entidades do domínio: sempre um record de resposta.

## Convenções

<!-- O agente imita o que vê. Escrever a convenção evita que ele imite o exemplo errado. -->

- Código, nomes e mensagens em português do Brasil.
- Dinheiro é sempre `decimal`.
- Data e hora sempre via `TimeProvider` injetado. Nunca `DateTime.Now` ou `DateTime.UtcNow`.
- Erros de regra: código estável em kebab-case (`limite-por-item`), mensagem legível para o usuário.
- Testes em xUnit, nome `Metodo_Cenario_ResultadoEsperado`. Regras de domínio: teste de unidade em `tests/Loja.Tests/Dominio`. Comportamento HTTP: teste em `tests/Loja.Tests/Http`.
- Quando um teste cobre um cenário de spec, marque com o comentário `// [Spec] <regra> - <cenário>`.

## Segurança

<!-- As duas falhas mais comuns em código gerado: autorização por recurso e confiar na entrada. -->

- Toda rota que recebe o id de um recurso do cliente (pedido, carrinho, cupom...) confere que o recurso pertence ao cliente da requisição. Recurso de outro cliente responde 404, igual a inexistente. Use `PedidoServico.ObterDoCliente` como modelo.
- Rotas de cliente usam o grupo com `.ExigirCliente()`. O id do cliente vem de `http.ClienteId()`, nunca do corpo da requisição.
- Toda entrada é validada no domínio (valores, limites, nulos), mesmo que o front já valide.

## Nunca

<!-- Proibições explícitas para os vícios mais caros de código gerado por IA. -->

- Nunca altere ou apague um teste existente para fazê-lo passar. Se um teste parece errado, pare e explique.
- Nunca engula exceção (`catch` vazio ou que só registra e segue).
- Nunca adicione pacote NuGet sem pedir e justificar. Prefira o que o .NET já oferece.
- Nunca leia arquivos `.env` nem coloque segredo em código, log ou resposta.
- Nunca diga que terminou sem rodar `dotnet build` e `dotnet test` e mostrar o resultado.
