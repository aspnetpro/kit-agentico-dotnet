# Sinais de alerta no diff

"Todos os testes passando" é o que o agente diz. O diff é o que ele fez. Use esta lista para ler o diff como um auditor: você não está procurando se funciona, está procurando **onde ele pode ter cortado caminho**.

Os sinais estão em ordem de gravidade.

## 🔴 Pare e investigue

**Teste existente alterado ou apagado**
Um teste que passava antes e foi mudado na mesma tarefa. Às vezes é legítimo (a regra mudou), mas muitas vezes o agente ajustou o teste para combinar com o código, e não o contrário.
Como achar: `git diff main -- tests/` e olhe as linhas com `-`.

**Mock ou dado falso no lugar do que deveria ser testado**
O teste "do cálculo do desconto" substitui justamente o calculador de desconto por um valor fixo. Ele passa sempre e não prova nada.

**Exceção engolida**
`catch { }` vazio, ou `catch` que só escreve um log e segue em frente. O sistema "funciona" escondendo o erro.

**Recurso de cliente sem conferir o dono**
Rota nova que recebe um código ou id (de cupom, pedido, endereço...) e não confere se aquilo pertence ao cliente da requisição.

## 🟡 Atenção

**Regra de negócio fora do domínio**
Cálculo de desconto, validação de validade ou de valor mínimo dentro do endpoint ou do serviço. Funciona hoje e vira duplicação amanhã.

**`DateTime.Now` ou `DateTime.UtcNow`**
Regra de validade que lê o relógio direto não dá para testar de forma confiável. O projeto tem `TimeProvider` para isso.

**Assert fraco**
O teste só confere que a resposta não é nula, ou que o status é 200, quando deveria conferir o valor exato do desconto.

**Lógica duplicada**
Um segundo jeito de calcular subtotal, de buscar produto ou de identificar o cliente, quando o projeto já tinha um.

**Pacote novo**
Um NuGet que não estava no projeto. Pergunte: precisa mesmo? O pacote existe e é mantido? O .NET já resolve isso sozinho?

## 🟢 Bons sinais

- Cada cenário da spec tem um teste com o nome que diz o cenário.
- Os testes novos foram vistos falhando antes da implementação.
- O diff é do tamanho da feature: não mexeu em nada que a feature não precisava.
- Os nomes, mensagens e códigos de erro seguem o padrão que já existia no projeto.
