# Seu primeiro ciclo agêntico em .NET

**Do prompt solto ao código verificado, em cerca de 90 minutos.**

Você vai implementar a mesma feature duas vezes, num projeto ASP.NET de verdade:

- **Rodada A**: do jeito que quase todo mundo usa IA hoje. Um prompt, aceita o que vier.
- **Rodada B**: com método. Spec → plano → testes primeiro → verificação.

No final você compara os dois resultados lado a lado. Não precisa acreditar em nada do que está escrito aqui: a diferença vai estar no seu diff.

> Kit criado por **Mick Banagouro**, CTO da Leanwork e criador da ASP.NET Pro.
> Versão 1.0 · novembro de 2026

---

## O que tem neste repositório

```
CLAUDE.md                 ← o contexto que o agente lê ao abrir o projeto
.claude/
  settings.json           ← o que o agente pode fazer sozinho, o que pergunta e o que é proibido
  commands/               ← /spec, /plano e /verificar: os comandos do método
  LEIA-ME.md              ← explicação de cada regra e de cada comando
docs/
  arquitetura.md          ← onde mora cada coisa, e por quê
  specs/                  ← specs das features (já tem uma de exemplo)
  planos/                 ← planos de implementação gerados pelo /plano
src/Loja.Api/             ← a API: catálogo, carrinho e pedidos
tests/Loja.Tests/         ← testes de domínio e de API (xUnit)
guia/                     ← fichas e checklists que você vai usar na missão
requisicoes.http          ← requisições prontas para testar a API à mão
```

A API é pequena de propósito (cerca de 15 arquivos). O assunto aqui não é o projeto, é o método.

---

## Pré-requisitos (cerca de 15 minutos)

1. **SDK do .NET 10**: [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. **Git** e uma conta no GitHub.
3. **Claude Code** instalado. Siga o instalador oficial na [documentação do Claude Code](https://docs.claude.com/en/docs/claude-code/setup): o comando de instalação muda de tempos em tempos, então pegue sempre na fonte.
4. Uma **conta Claude com acesso ao Claude Code** (plano pago) ou uma chave da API da Anthropic.
5. Recomendado: VS Code, Rider ou Visual Studio, para ler o diff com conforto.

Para conferir:

```bash
dotnet --version   # 10.x
git --version
claude --version
```

---

## A missão

O dono da loja pediu: **"quero cupom de desconto no carrinho"**.

Abra a [ficha de comparação](guia/ficha-de-comparacao.md) numa aba ao lado. Você vai preenchê-la durante as duas rodadas.

### Passo 0: linha de base (5 min)

1. No GitHub, clique em **Use this template** e crie o seu repositório.
2. Clone e rode os testes:

```bash
git clone <url-do-seu-repositorio>
cd <pasta>
dotnet test
```

Todos verdes. Guarde o número de testes: é a sua linha de base.

> **Por que começar por aqui:** você só sabe se o agente quebrou alguma coisa se souber que estava funcionando antes.

### Passo 1: explorar (10 min)

Na pasta do projeto, abra o agente:

```bash
claude
```

Peça:

```
Explique a arquitetura deste projeto e onde uma regra de desconto deveria morar.
```

Repare em duas coisas. Ele sabe as camadas e as convenções sem você explicar nada: leu o `CLAUDE.md`. E ele aponta o domínio como lugar da regra: leu o `docs/arquitetura.md`.

Abra o `CLAUDE.md` e leia os comentários. Cada bloco diz por que existe.

> 📌 **[HISTÓRIA DO MICK: contexto]**
> *Sugestão: um caso real em que o agente replicou um padrão errado num projeto de cliente porque o padrão certo não estava escrito em lugar nenhum, e o que mudou depois que virou instrução no projeto. 4 a 6 linhas.*

### Passo 2: Rodada A, o jeito comum (15 min)

Saia do agente (`/exit`) e crie uma branch para esta rodada:

```bash
git switch -c rodada-a
claude
```

Mande **um único prompt**:

```
Adiciona cupom de desconto no carrinho.
```

Aprove o que ele pedir, sem discutir. Quando ele disser que terminou, **não leia o código ainda**. Preencha a coluna "Rodada A" da ficha respondendo a partir do que ele construiu:

- Desconto percentual, valor fixo ou os dois?
- Dá para usar mais de um cupom?
- O desconto vale para produto que já está em promoção?
- Existe valor mínimo de pedido? E validade?
- Um cupom exclusivo de um cliente pode ser usado por outro?
- O que acontece se o desconto for maior que o valor da compra?
- Quantos testes novos existem? Algum teste antigo mudou?

Depois salve a rodada e volte para a main:

```bash
dotnet test
git add -A
git commit -m "Rodada A: cupom com prompt único"
git switch main
```

> **Repare:** a Rodada A já não foi "pura": o projeto tem `CLAUDE.md`, então o código provavelmente seguiu as camadas e as convenções. Mesmo assim, cada pergunta da ficha foi uma **decisão de negócio que o agente tomou sozinho, sem te avisar**. Contexto bom não substitui spec.
>
> *Quer ver o efeito do contexto também?* Faça uma "Rodada A0" opcional: renomeie o `CLAUDE.md` para `CLAUDE.md.bak` numa terceira branch e repita o mesmo prompt.

### Passo 3: Rodada B, especificar (15 min)

Crie a branch da segunda rodada e abra uma sessão **nova** do agente, para nada da Rodada A contaminar o contexto:

```bash
git switch -c rodada-b
claude
```

```
/spec cupom de desconto no carrinho
```

O agente vai ler o projeto e te fazer perguntas antes de escrever qualquer coisa. Você faz o papel do dono da loja: responda usando o [cartão de respostas do dono da loja](guia/respostas-do-dono-da-loja.md), para que o resultado seja comparável entre quem fizer o kit.

Quando a spec aparecer em `docs/specs/`, **leia e corrija**. Ela é sua, não do agente. Compare as regras com as respostas que o agente "inventou" na Rodada A.

### Passo 4: planejar (10 min)

```
/plano docs/specs/cupom-de-desconto-no-carrinho.md
```

(Ajuste o caminho para o nome que a spec recebeu.)

O agente explora o código e propõe etapas pequenas, cada uma com os testes que vai escrever. Leia o plano em `docs/planos/`. Se discordar de algo, diga. Quando estiver bom:

```
Plano aprovado. Execute etapa por etapa.
```

### Passo 5: testes primeiro, depois o código (15 min)

Acompanhe a execução. Em cada etapa o agente deve:

1. escrever os testes a partir dos cenários da spec;
2. rodar e mostrar que eles **falham pelo motivo certo** (a regra ainda não existe);
3. implementar o mínimo;
4. rodar de novo: tudo verde;
5. marcar a etapa no plano.

Se ele pular a parte de mostrar o teste falhando, cobre. Um teste que nunca falhou não prova nada.

> 📌 **[HISTÓRIA DO MICK: teste como contrato]**
> *Sugestão: uma vez em que "todos os testes passando" escondia um problema (teste alterado para passar, mock que testava o mock) e como isso foi pego. 4 a 6 linhas.*

### Passo 6: verificar (15 min)

```
/verificar
```

O agente roda build e testes e revisa tudo o que mudou em relação à main, em quatro blocos: spec, testes, código e segurança. Ele **só relata**, não conserta.

Depois vem a parte que nenhum comando faz por você: **leia o diff** com o [checklist de sinais de alerta](guia/checklist-sinais-de-alerta.md) aberto ao lado.

```bash
git diff main --stat
git diff main
```

Preste atenção especial na segurança: o cartão do dono da loja tem **cupons exclusivos de um cliente**. Existe teste provando que outro cliente não consegue usar? É o mesmo tipo de falha do teste `GetPedido_DeOutroCliente_Retorna404`, que já está no projeto.

Corrigido o que for preciso:

```bash
git add -A
git commit -m "Rodada B: cupom com spec, plano, testes e verificação"
```

> 📌 **[HISTÓRIA DO MICK: segurança]**
> *Sugestão: um caso real (pode ser anonimizado) de endpoint autenticado que deixava um usuário ver dados de outro, e quanto custou achar depois. 4 a 6 linhas.*

### Passo 7: comparar (5 min)

```bash
git diff rodada-a rodada-b --stat
```

Termine a ficha de comparação. As perguntas que importam:

- Na Rodada A, quantas decisões de negócio o agente tomou sem te perguntar?
- Em qual das duas você assinaria embaixo para ir para produção?
- Em qual você conseguiria explicar cada linha para alguém do seu time?

---

## O que você acabou de fazer

Você rodou um ciclo completo de **engenharia agêntica**: o agente digitou quase tudo, e você foi dono das decisões e da verificação.

Mas repare no que **não** foi você que fez. O `CLAUDE.md`, as permissões do `settings.json` e os três comandos vieram prontos. Eles funcionaram porque foram escritos para *este* projeto.

No seu projeto de verdade, quem escreve é você.

## Próximo passo

**[NOME DO WORKSHOP]**: [descrição em uma linha: configurar o Claude Code no SEU projeto .NET: contexto, comandos, skills, MCPs e hooks]
👉 [LINK]

Ainda não fez o diagnóstico? **[Descubra seu nível de engenharia agêntica]([LINK DO QUIZ])**: 14 perguntas, cerca de 4 minutos.

---

## Material de apoio

- [Ficha de comparação das rodadas](guia/ficha-de-comparacao.md)
- [Cartão de respostas do dono da loja](guia/respostas-do-dono-da-loja.md)
- [Checklist de sinais de alerta no diff](guia/checklist-sinais-de-alerta.md)
- [Checklist de segurança do agente](guia/checklist-seguranca-agente.md)
- [Páginas de decisão: quando usar spec, quando limpar o contexto, quando usar subagente, quando não usar o agente](guia/paginas-de-decisao.md)
- [Como a pasta .claude funciona](.claude/LEIA-ME.md)

## Problemas comuns

| Sintoma | O que fazer |
|---|---|
| `dotnet test` falha no passo 0 | Confira se o SDK é o 10 (`dotnet --version`). O `global.json` exige .NET 10. |
| `/spec` não aparece como comando | Abra o `claude` na raiz do repositório (onde está a pasta `.claude`). |
| O agente pede aprovação para tudo | É o comportamento esperado para o que não está em `allow`. Leia o pedido antes de aprovar. Veja `.claude/LEIA-ME.md`. |
| O agente quer instalar um pacote NuGet | Ele vai pedir aprovação (está em `ask`). Pergunte por que precisa e se dá para fazer sem. |
| A conversa ficou longa e o agente começou a errar | Abra uma sessão nova e peça: "retome a partir de docs/planos/<arquivo>". Veja as [páginas de decisão](guia/paginas-de-decisao.md). |
