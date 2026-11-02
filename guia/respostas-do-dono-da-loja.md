# Cartão de respostas do dono da loja

Use na **Rodada B**, quando o `/spec` fizer perguntas. Você faz o papel do dono da loja.

As perguntas do agente não vão vir nesta ordem nem com estas palavras. Ache a resposta que corresponde. Se ele perguntar algo que não está aqui, decida você mesmo e anote a decisão na ficha de comparação: é exatamente o tipo de coisa que a Rodada A decidiu sem perguntar.

---

**Tipos de cupom**
Dois tipos: **percentual** (ex.: 10% de desconto) e **valor fixo** (ex.: R$ 50 de desconto).

**Quantos cupons por carrinho**
**Um só.** Aplicar um cupom novo substitui o anterior.

**Produtos em promoção**
O cupom **não vale** para itens que já estão em promoção. O desconto é calculado só sobre os itens com preço cheio.

**Valor mínimo**
Alguns cupons exigem valor mínimo. O mínimo é comparado com o **subtotal do carrinho inteiro** (inclusive itens em promoção).

**Validade**
Todo cupom tem data de validade. Depois dela, não pode mais ser aplicado. Se o cupom expirar com o carrinho aberto, ele cai no fechamento do pedido.

**Cupom exclusivo**
Alguns cupons são **exclusivos de um cliente** (ex.: cupom de aniversário da Ana). Outro cliente que tentar usar recebe a mesma resposta de um cupom que não existe.

**Desconto maior que a compra**
O desconto nunca passa do valor dos itens elegíveis. O total nunca fica negativo.

**Arredondamento**
Duas casas decimais, arredondando para o centavo mais próximo (meio centavo para cima).

**Código do cupom**
Não diferencia maiúsculas de minúsculas (`natal10` = `NATAL10`) e ignora espaços nas pontas.

**Como o cliente usa**
Aplica o cupom no carrinho e pode remover. O carrinho mostra subtotal, desconto e total. O pedido fechado guarda qual cupom foi usado e o desconto.

**Limite de usos**
Fora de escopo nesta versão.

**Cupons para testar** (pode pedir para o agente cadastrar em memória, igual ao catálogo)

| Código | Tipo | Valor | Mínimo | Validade | Exclusivo de |
|---|---|---|---|---|---|
| `BEMVINDO10` | percentual | 10% | nenhum | 31/12/2099 | todos |
| `MENOS50` | valor fixo | R$ 50 | R$ 300 | 31/12/2099 | todos |
| `EXPIRADO` | percentual | 20% | nenhum | 01/01/2020 | todos |
| `ANIVERSARIO-ANA` | percentual | 15% | nenhum | 31/12/2099 | ana |
