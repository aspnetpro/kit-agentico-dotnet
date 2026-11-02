# Ficha de comparação: Rodada A × Rodada B

Copie esta tabela para um papel ou para um arquivo seu e preencha durante a missão.

## Decisões de negócio

Na Rodada A, responda olhando o que o agente construiu (e não o que você queria). Na Rodada B, responda a partir da spec.

| Pergunta | Rodada A | Rodada B |
|---|---|---|
| Percentual, valor fixo ou os dois? | | |
| Mais de um cupom por carrinho? | | |
| Vale para produto em promoção? | | |
| Tem valor mínimo? Sobre qual valor? | | |
| Tem validade? Usa `TimeProvider` ou `DateTime.Now`? | | |
| Cupom exclusivo de um cliente pode ser usado por outro? | | |
| Desconto maior que a compra: o que acontece? | | |
| Como arredonda? | | |
| `natal10` e `NATAL10` são o mesmo cupom? | | |
| **Quantas dessas decisões o agente tomou sem perguntar?** | | **0** (se não for 0, anote quais) |

## Engenharia

| Item | Rodada A | Rodada B |
|---|---|---|
| Onde está a regra do desconto? (pasta/arquivo) | | |
| Testes antes da missão (linha de base) | | |
| Testes depois | | |
| Algum teste antigo foi alterado? | | |
| Existe teste para cupom de outro cliente? | | |
| Existe teste para cupom expirado? | | |
| Pacote NuGet novo? | | |
| Tempo gasto | | |

## As três perguntas finais

1. Em qual das duas você assinaria embaixo para ir para produção?
2. Em qual você conseguiria explicar cada linha para alguém do seu time?
3. Quanto tempo a Rodada A ainda precisaria para chegar no nível da B?
