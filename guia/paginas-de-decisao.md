# Páginas de decisão

Ferramenta muda toda semana. Saber **quando** usar cada coisa não muda. Responda de cima para baixo: o primeiro "sim" decide.

---

## 1. Isso precisa de spec?

| Pergunta | Se sim |
|---|---|
| Mexe com dinheiro, permissão ou dado de cliente? | **Spec.** O custo de um erro aqui é alto demais para deixar o agente decidir. |
| Tem mais de um jeito razoável de interpretar o pedido? | **Spec.** Cada interpretação que você não escreve, o agente escolhe sozinho. |
| Cria ou muda uma regra de negócio? | **Spec.** |
| Vai levar mais de uma sessão de trabalho? | **Spec + plano.** O plano vira a memória entre as sessões. |
| É um ajuste mecânico (renomear, mover, formatar, corrigir um bug óbvio)? | **Sem spec.** Um prompt claro resolve. |

**Teste rápido:** se você tentar escrever três cenários Given/When/Then e travar, o problema não está claro nem para você. O agente não vai deixar mais claro.

> 📌 **[HISTÓRIA DO MICK]**
> *Sugestão: um pedido de cliente que parecia simples e escondia três decisões de negócio, descobertas só depois da entrega.*

---

## 2. Hora de limpar o contexto?

O agente não tem memória entre mensagens: a cada uma, ele relê a conversa inteira. Conversa longa custa mais e, bem antes de acabar o espaço, deixa as respostas menos precisas.

| Sinal | O que fazer |
|---|---|
| Ele esqueceu algo que vocês combinaram lá atrás | Registre o estado num arquivo e abra uma sessão nova. |
| A conversa mudou de assunto (terminou a feature, vai começar outra) | Sessão nova. |
| Ele começou a repetir um erro que você já corrigiu | Sessão nova, com a correção escrita no `CLAUDE.md` ou no plano. |
| Você está colando o mesmo texto pela terceira vez | Esse texto devia estar num arquivo do projeto. |

**Como retomar:** `/clear` (ou uma sessão nova) e depois *"retome a partir de docs/planos/<arquivo>"*. O plano com checkboxes é exatamente para isso.

---

## 3. Subagente ou não?

Um subagente é uma instância separada que recebe uma tarefa, trabalha no próprio contexto e devolve só o resultado.

| Situação | Decisão |
|---|---|
| Busca ampla em que você só quer a conclusão ("onde esse método é chamado em todo o repositório?") | **Subagente.** Mantém o seu contexto limpo. |
| Várias investigações independentes que podem rodar em paralelo | **Subagentes.** |
| Revisão independente de algo que o agente principal escreveu | **Subagente.** Quem escreveu não deveria se corrigir sozinho. |
| A tarefa depende do que vocês discutiram nesta conversa | **Não.** O subagente começa do zero e vai ter que reaprender tudo. |
| É uma tarefa pequena | **Não.** O custo de explicar é maior que o de fazer. |

Mais agentes não é mais qualidade. Comece com um bem instruído e divida só quando o problema pedir.

---

## 4. Quando NÃO usar o agente

| Situação | Por quê |
|---|---|
| Mudança de três linhas que você já sabe exatamente como fazer | Explicar demora mais que fazer. |
| Você não conseguiria verificar se o resultado está certo | Se você não sabe avaliar, não sabe se ele acertou. Aprenda o assunto primeiro, ou peça para ele te ensinar em vez de fazer. |
| Exige credencial ou acesso de produção | Não dê ao agente um acesso que você não daria a um estagiário no primeiro dia. |
| Decisão que só você ou o seu cliente podem tomar | O agente pode listar opções e trade-offs. A escolha é sua. |
| Você está aprendendo exatamente aquilo | Se o objetivo é aprender a escrever um teste, escreva o teste. Use o agente para revisar. |

**O agente digita. Você continua dono das decisões e das consequências.**
