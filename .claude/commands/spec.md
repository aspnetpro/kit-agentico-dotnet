---
description: Entrevista você sobre uma feature e gera a spec (regras + cenários Gherkin) em docs/specs/. Não escreve código.
argument-hint: <nome-da-feature> [descrição curta]
---

Você vai escrever a especificação da feature: **$ARGUMENTS**

Nesta etapa você NÃO escreve nem altera código. O único arquivo que você cria é a spec.

## 1. Entenda o terreno

Leia, nesta ordem:
- `docs/arquitetura.md`
- `docs/specs/limite-por-item.md` (é o formato de referência; siga-o)
- O código de domínio que a feature vai tocar (comece por `src/Loja.Api/Dominio/`)

## 2. Ache as decisões que o pedido não responde

Liste as decisões de negócio que precisam ser tomadas e que o pedido não deixa claras.
Cubra pelo menos estas categorias, quando se aplicarem:

- **Cálculo**: como o valor é calculado, arredondamento, mínimos e máximos.
- **Combinação**: como a feature interage com regras que já existem (promoção, limite por item, estoque...).
- **Limites**: quantas vezes, por quem, até quanto.
- **Tempo**: validade, expiração, datas de início e fim.
- **Quem pode**: a regra vale para todos os clientes ou para alguns? Um cliente pode usar algo que pertence a outro?
- **Falhas**: o que acontece em cada caso inválido, com qual código de erro e qual mensagem.

## 3. Pergunte e ESPERE

Faça todas as perguntas de uma vez, numeradas, cada uma com opções sugeridas (a), (b), (c).
Pare e espere as respostas. Não invente respostas.
Se o usuário responder "decida você" para algum item, proponha uma decisão e marque-a na spec como `[DECISÃO PROPOSTA]`.

## 4. Escreva a spec

Crie `docs/specs/<nome-da-feature>.md` com estas seções, no mesmo estilo do arquivo de referência:

1. **Contexto**: o problema, em duas ou três frases.
2. **Fora de escopo**: o que esta feature NÃO faz.
3. **Regras de negócio**: numeradas (RN01, RN02...), uma decisão por regra, cada uma com o código de erro quando houver.
4. **Cenários**: em Gherkin, em português (Funcionalidade, Cenário, Dado, Quando, Então, E). Pelo menos um cenário por regra, mais os casos de borda (exatamente no limite, um acima, combinação com outra regra).
5. **Questões em aberto**: o que ainda depende de alguém decidir.
6. **Rastreabilidade**: tabela cenário → teste, com a coluna de teste vazia (será preenchida na implementação).

## 5. Feche

Mostre um resumo curto (quantas regras, quantos cenários, decisões marcadas como propostas) e peça para o usuário revisar a spec antes de seguir para `/plano`.
