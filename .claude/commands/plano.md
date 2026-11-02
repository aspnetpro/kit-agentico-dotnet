---
description: Lê uma spec, explora o código e propõe um plano de implementação em etapas. Não escreve código até você aprovar.
argument-hint: <caminho-da-spec, ex. docs/specs/cupom.md>
---

Você vai planejar a implementação da spec em **$ARGUMENTS**.

Nesta etapa você NÃO escreve nem altera código. O único arquivo que você cria é o plano.

## 1. Explore

- Leia a spec inteira, `CLAUDE.md` e `docs/arquitetura.md`.
- Encontre no código onde cada regra da spec vai morar e o que já existe que pode ser reaproveitado.
- Se a spec tiver contradição, lacuna ou `[DECISÃO PROPOSTA]` não confirmada, pare e pergunte antes de planejar.

## 2. Escreva o plano

Crie `docs/planos/<mesmo-nome-da-spec>.md` com:

- **Resumo**: a abordagem em três ou quatro linhas.
- **Etapas**: pequenas, cada uma deixando o build e os testes verdes. Ordem padrão:
  1. testes de domínio da regra → 2. domínio → 3. aplicação → 4. contratos e endpoint → 5. testes de API.
  Para cada etapa, em checkbox `- [ ]`:
  - arquivos criados ou alterados;
  - testes que serão escritos, com o nome no padrão `Metodo_Cenario_ResultadoEsperado` e o cenário da spec que cada um cobre;
  - critério de pronto da etapa.
- **Riscos**: o que pode quebrar, inclusive testes existentes que podem ser afetados.
- **Segurança**: como a autorização por recurso e a validação de entrada serão garantidas nesta feature.

## 3. Pare

Mostre o plano e espere aprovação. Não comece a implementar.

## Quando o usuário aprovar e pedir para executar

Siga o plano etapa por etapa:
1. Escreva os testes da etapa.
2. Rode `dotnet test` e confirme que os novos testes **falham pelo motivo certo** (a regra ainda não existe), não por erro de compilação ou de configuração.
3. Implemente o mínimo para passar.
4. Rode `dotnet build` e `dotnet test`. Tudo verde.
5. Marque a etapa como `- [x]` no arquivo do plano e preencha a coluna de testes da tabela de rastreabilidade da spec.

O arquivo do plano é o estado do trabalho: se a sessão for interrompida ou o contexto for limpo, retome a partir dele.
