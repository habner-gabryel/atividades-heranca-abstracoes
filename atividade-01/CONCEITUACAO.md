## Ritual Comum de Processamento
### O processamento de pedidos segue um fluxo fixo de três etapas:

**Validar o pedido:** Verificar dados essenciais (ex.: itens, cliente, origem).

**Calcular o total:** Somar valores dos itens, aplicando regras específicas de impostos/taxas.

**Emitir recibo:** Gerar um documento formal conforme o tipo de pedido (ex.: NF-e para nacional, Commercial Invoice para internacional).

## Variações: Nacional vs. Internacional


| Aspecto | Pedido Nacional | Pedido Internacional |
| --- | --- | --- |
| Impostos/Taxas | Impostos internos (ICMS, IPI) | Taxas de importação, custos aduaneiros, câmbio |
| Formato do Recibo | Nota Fiscal Eletrônica (NF-e) | Commercial Invoice (com dados de exportação) |

### Justificativa para Herança:
- A herança por especialização é adequada porque:

- O ritual central (Validar → Calcular → Emitir) é invariável, mas a implementação de cada etapa varia conforme a origem do pedido.

- Classes base (ex.: Pedido) podem definir o esqueleto do processo (via Template Method), enquanto subclasses (ex.: PedidoNacional, PedidoInternacional) especializam:

    - Cálculos fiscais.

    - Validações regionais.

    - Formatação de recibos.

- Isso evita duplicação de código do ritual comum e isolas diferenças nas subclasses.

## Políticas Extras: Composição
**Exemplos de políticas:** Frete Expresso, Embalagem para Presente, Seguro contra Avarias, Promoção de Desconto.

### Justificativa para Composição:

- As políticas são independentes e combináveis (ex.: um pedido pode ter frete expresso + seguro).

- Herança seria inflexível (explosão de subclasses como PedidoComFreteESeguro).

- Composição permite:

    - Injetar políticas como dependências (ex.: Pedido com IFrete, IEmbalagem).

    - Alterar políticas sem modificar a lógica central do pedido.

    - Cumprir o Open/Closed Principle: novas políticas são adicionadas sem alterar código existente.

## Conclusão
Herança organiza as variações inerentes ao tipo de pedido (nacional/internacional).

Composição gerencia políticas acessórias e combináveis (frete, embalagem, etc.), garantindo flexibilidade e baixo acoplamento.