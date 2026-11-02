---
description: Roda build e testes e revisa tudo o que mudou em relação à main (spec, testes, código, segurança). Só relata, não conserta.
argument-hint: "[branch de comparação, padrão: main]"
---

Você vai verificar o trabalho feito nesta branch. Compare com: **$ARGUMENTS** (se vazio, use `main`).

Nesta etapa você NÃO altera nenhum arquivo. Você só investiga e relata.

## 1. Levante o que mudou

- `git status` (inclua arquivos novos ainda não versionados)
- `git diff <branch-de-comparacao> --stat` e depois o diff completo

## 2. Rode as verificações automáticas

- `dotnet build`: avisos contam.
- `dotnet test`: quantos testes, quantos novos, todos verdes?

## 3. Revise em quatro blocos

**Spec**
- Se existe spec em `docs/specs/` para esta mudança: todo cenário tem um teste correspondente? Liste cenários sem teste.
- Há comportamento implementado que a spec não pede?

**Testes**
- Algum teste que já existia na branch de comparação foi alterado ou removido? Mostre o antes e o depois. Isso é o sinal de alerta mais importante.
- Há asserts fracos (só `NotNull`, só status 200) onde deveria haver valor exato?
- Há mock ou dado falso substituindo exatamente o que o teste deveria testar?

**Código**
- Regra de negócio fora de `Dominio/` (em endpoint, serviço ou contrato)?
- `catch` vazio, ou que registra e segue em frente?
- `DateTime.Now`/`UtcNow` em vez de `TimeProvider`? Dinheiro que não é `decimal`?
- Lógica duplicada de algo que já existia?
- Pacote NuGet novo? Se sim: é necessário? Existe mesmo?

**Segurança**
- Toda rota nova que recebe id de recurso confere se o recurso pertence ao cliente (`http.ClienteId()`)?
- Algum dado do cliente vem do corpo da requisição quando deveria vir do cliente autenticado?
- Entrada validada no domínio (valores negativos, nulos, limites)?
- Algum dado sensível em log, mensagem de erro ou resposta?

## 4. Relate

Formato:
- Resultado do build e dos testes, em uma linha cada.
- Uma lista por bloco com ✅ ok, ⚠️ atenção ou ❌ problema, sempre com `arquivo:linha`.
- No final: **"O que um humano precisa olhar"**, com no máximo três itens, os mais arriscados primeiro.

Não conserte nada. Se o usuário quiser correções, ele vai pedir depois de ler o relatório.
