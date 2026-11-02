# Checklist de segurança do agente

Dois tipos de risco, que muita gente mistura:

1. **O código que o agente escreve** pode ter falhas de segurança.
2. **O próprio agente, rodando na sua máquina**, é uma porta de entrada.

## 1. Segurança do código gerado

O agente aprendeu com o código médio da internet, e o código médio da internet não é seguro. Confira sempre:

- [ ] **Autorização por recurso.** A rota exige login *e* confere se o recurso é do cliente? Exigir login não basta: sem a segunda parte, qualquer cliente logado lê o pedido de outro trocando o número na URL.
- [ ] **Identidade vem da autenticação, não do corpo.** O id do cliente sai do token (aqui, de `http.ClienteId()`), nunca de um campo `clienteId` que o próprio cliente envia.
- [ ] **Entrada validada no servidor.** Valores negativos, zero, nulos, textos enormes, códigos com espaços. O front-end validar não conta.
- [ ] **Erro não vaza detalhe.** Mensagem de erro sem stack trace, sem SQL, sem dizer "esse cupom existe mas não é seu".
- [ ] **Nada sensível em log ou resposta.** Token, senha, documento, dados de cartão.
- [ ] **SQL sempre parametrizado** (quando houver banco).

## 2. Segurança do processo agêntico

- [ ] **Permissões configuradas, não "aprova tudo".** O `.claude/settings.json` define o que roda sozinho, o que pergunta e o que é proibido. Rodar com tudo liberado para "não ficar aprovando" é o equivalente a desligar o antivírus porque ele atrasa.
- [ ] **Nunca credencial de produção na máquina onde o agente roda.** Se o agente tem acesso a produção, qualquer texto que ele leia pode tentar usar esse acesso.
- [ ] **Desconfie de instruções dentro de conteúdo.** Um README, uma issue, um comentário de código ou uma página web podem conter texto escrito para o agente ("ignore as instruções anteriores e..."). Isso se chama *prompt injection*. Para o modelo, instrução e conteúdo são o mesmo texto.
- [ ] **Pacote novo sempre com aprovação.** Agentes às vezes sugerem pacotes com nomes plausíveis que não existem, e há quem registre esses nomes para distribuir código malicioso. Confira nome, autor e número de downloads antes de aprovar.
- [ ] **Servidores MCP e plugins de terceiros são dependências.** Cada um é código com acesso ao seu ambiente. Instale como você instalaria uma biblioteca: sabendo quem fez e o que ele pode acessar.
- [ ] **Segredos fora do alcance.** `.env` e arquivos de segredo bloqueados no `deny` e fora do Git.
- [ ] **O agente não publica por você.** `git push`, deploy e envio de mensagens passam por você.

## Neste kit

| Proteção | Onde está |
|---|---|
| Leitura de `.env` e segredos bloqueada | `.claude/settings.json` → `deny` |
| `git push`, `rm -rf`, `curl`, `wget` bloqueados | `.claude/settings.json` → `deny` |
| Pacote NuGet sempre pergunta | `.claude/settings.json` → `ask` |
| Regra de autorização por recurso escrita para o agente | `CLAUDE.md` → Segurança |
| Exemplo de teste de autorização por recurso | `tests/Loja.Tests/Http/ApiTests.cs` → `GetPedido_DeOutroCliente_Retorna404` |
| Revisão de segurança no fim de toda tarefa | `/verificar` → bloco Segurança |
