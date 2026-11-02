# A pasta `.claude/`

Tudo aqui é configuração do Claude Code **para este projeto**, versionada junto com o código.
Quem clonar o repositório recebe as mesmas regras e os mesmos comandos. É assim que um time inteiro trabalha do mesmo jeito com agente.

## `settings.json`: o que o agente pode fazer sozinho

JSON não aceita comentário, então a explicação fica aqui. As regras são avaliadas nesta ordem: **deny → ask → allow**. A primeira que casar vence.

| Lista | O que tem | Por quê |
|---|---|---|
| `allow` | `dotnet build`, `dotnet test`, `git status/diff/log` | Só leem ou verificam. Pedir aprovação para isso só treina você a clicar "sim" sem ler. |
| `ask` | `dotnet add/remove`, `git add/commit/checkout/reset/stash` | Mudam dependências ou histórico. **Pacote novo sempre pergunta**: agentes às vezes sugerem pacotes que não existem, e há quem registre esses nomes para distribuir código malicioso. |
| `deny` | ler `.env` e segredos, `git push`, `rm -rf`, `curl`, `wget` | O agente não precisa de segredo para escrever código, não deve publicar nada por você e não deve baixar coisas da internet pelo terminal. |

Repare no que **não** está aqui: nenhum modo "aprova tudo". Se você roda o agente com permissões liberadas para "não ficar aprovando", qualquer instrução maliciosa escondida num arquivo, issue ou página que ele leia vira um comando executado na sua máquina.

Para ajustes pessoais (que não vão para o repositório), use `.claude/settings.local.json`. Ele já está no `.gitignore`.

## `commands/`: os comandos do método

Cada arquivo `.md` vira um slash command dentro do Claude Code:

| Comando | Etapa | O que faz |
|---|---|---|
| `/spec <feature>` | Especificar | Entrevista você sobre as decisões de negócio e gera `docs/specs/<feature>.md`. Não escreve código. |
| `/plano <spec>` | Planejar | Lê a spec, explora o código e propõe um plano em etapas com os testes de cada uma. Não escreve código até você aprovar. |
| `/verificar` | Verificar | Roda build e testes e revisa o que mudou: spec coberta, testes alterados, vícios de código, segurança. Só relata, não conserta. |

Abra os arquivos e leia: são prompts comuns, escritos em português. No seu projeto, você vai escrever os seus.
