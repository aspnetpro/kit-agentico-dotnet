# Spec: limite de unidades por item no carrinho

> Esta é a spec de referência do projeto. A feature já está implementada; o arquivo existe para mostrar o formato que o `/spec` deve seguir e como uma spec vira teste.

## Contexto

Clientes atacadistas estavam comprando o estoque inteiro de produtos em promoção para revender. A loja quer limitar quantas unidades de um mesmo produto cabem num carrinho.

## Fora de escopo

- Limite por pedido ou por período (por exemplo, "5 por mês").
- Limites diferentes por produto ou por categoria.
- Limite no valor total do carrinho.

## Regras de negócio

- **RN01**: Um mesmo produto pode ter no máximo **10 unidades** no carrinho. Acima disso, a operação falha com o código `limite-por-item` e o carrinho não muda.
- **RN02**: O limite considera a soma com o que já está no carrinho, não só a quantidade da adição atual.
- **RN03**: O limite vale por produto. Produtos diferentes têm limites independentes.

## Cenários

```gherkin
Funcionalidade: Limite de unidades por item no carrinho

  Cenário: adicionar exatamente o limite
    Dado um carrinho vazio
    Quando o cliente adiciona 10 unidades do Teclado
    Então o carrinho tem 10 unidades do Teclado

  Cenário: passar do limite numa adição só
    Dado um carrinho vazio
    Quando o cliente adiciona 11 unidades do Teclado
    Então a operação falha com o código "limite-por-item"
    E o carrinho continua vazio

  Cenário: passar do limite somando com o que já está no carrinho
    Dado um carrinho com 8 unidades do Teclado
    Quando o cliente adiciona mais 3 unidades do Teclado
    Então a operação falha com o código "limite-por-item"
    E o carrinho continua com 8 unidades do Teclado

  Cenário: o limite vale por produto, não por carrinho
    Dado um carrinho com 10 unidades do Teclado
    Quando o cliente adiciona 10 unidades do Mouse
    Então o carrinho tem 10 unidades do Teclado e 10 unidades do Mouse

  Cenário: a API informa o código da regra
    Dado um cliente identificado
    Quando ele envia POST /carrinho/itens com 11 unidades de um produto
    Então a resposta é 422
    E o corpo contém o código "limite-por-item"
```

## Questões em aberto

- Nenhuma.

## Rastreabilidade

| Regra | Cenário | Teste |
|---|---|---|
| RN01 | adicionar exatamente o limite | `CarrinhoTests.AdicionarItem_ExatamenteNoLimite_Permite` |
| RN01 | passar do limite numa adição só | `CarrinhoTests.AdicionarItem_AcimaDoLimite_LancaLimitePorItem` |
| RN02 | passar do limite somando com o que já está no carrinho | `CarrinhoTests.AdicionarItem_SomaUltrapassaLimite_LancaLimitePorItemEMantemQuantidade` |
| RN03 | o limite vale por produto, não por carrinho | `CarrinhoTests.AdicionarItem_ProdutosDiferentes_LimiteValePorProduto` |
| RN01 | a API informa o código da regra | `ApiTests.PostItem_AcimaDoLimite_Retorna422ComCodigo` |
