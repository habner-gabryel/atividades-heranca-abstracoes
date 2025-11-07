**Documento de Design - Fase 2: Sistema de Pagamentos**

---

### **Contrato da Classe Base `Pagamento`**

```csharp
public abstract class Pagamento
{
    // Template Method - Ritual fixo NÃO virtual
    public void Processar()
    {
        Validar();
        AutorizarOuCapturar();
        Confirmar();
    }

    // Ganchos para especialização (protected virtual)
    protected virtual void Validar()
    {
        // Validações mínimas comuns
        // - Valor positivo
        // - Dados do pagador presentes
        // - Meio de pagamento válido
    }

    protected virtual void AutorizarOuCapturar()
    {
        // Implementação base vazia - será sobrescrita
    }

    protected virtual void Confirmar()
    {
        // Implementação base vazia - será sobrescrita
    }

    // Aplicação de políticas plugáveis
    protected bool AplicarAntifraude(decimal valor)
    {
        return _antifraude?.Invoke(valor) ?? true; // Default: aprova se não houver política
    }

    protected decimal AplicarCambio(decimal valor)
    {
        return _cambio?.Invoke(valor) ?? valor; // Default: mantém valor original
    }

    // Delegates para políticas
    public Func<decimal, bool> Antifraude { set => _antifraude = value; }
    public Func<decimal, decimal> Cambio { set => _cambio = value; }

    private Func<decimal, bool>? _antifraude;
    private Func<decimal, decimal>? _cambio;
}
```

---

### **Regras de LSP (Liskov Substitution Principle)**

#### **1. Substituibilidade Total**
- **Regra**: Qualquer cliente que usa `Pagamento` via `Processar()` deve funcionar identicamente com `PagamentoCartao`, `PagamentoPIX`, `PagamentoBoleto`
- **Garantia**:
  - Cliente trabalha exclusivamente com tipo base `Pagamento`
  - Sem necessidade de `is`, `as` ou `downcast`
  - Exemplo:
    ```csharp
    Pagamento pagamento = new PagamentoCartao(); // Ou PIX, Boleto
    pagamento.Processar(); // Comportamento consistente
    ```

#### **2. Invariantes de Validação Preservadas**
- **Regra**: Validações mínimas da base não podem ser enfraquecidas
- **Implementação**:
  - Derivadas podem **adicionar** validações específicas, mas não **remover** as básicas
  - Padrão obrigatório: chamada a `base.Validar()`
  ```csharp
  protected override void Validar()
  {
      base.Validar(); // Preserva validações mínimas
      ValidarCVV();   // Validações específicas do cartão
      ValidarLimite();
  }
  ```

#### **3. Contrato de Processamento Consistente**
- **Regra**: `Processar()` sempre completa o ciclo e produz resultado coerente
- **Garantias**:
  - Sempre executa as 3 etapas na ordem definida
  - Estado final do pagamento é consistente (autorizado/confirmado ou falha clara)
  - Não introduz exceções inesperadas em relação ao contrato base
  - Comportamentos assíncronos (ex.: boleto) são encapsulados internamente

---

### **Eixos Plugáveis (Delegates)**

| **Política** | **Delegate** | **Assinatura** | **Papel** | **Exemplo de Implementação** |
|--------------|--------------|----------------|-----------|-----------------------------|
| **Antifraude** | `Func<decimal, bool>` | `decimal → bool` | Analisa risco do pagamento. Retorna `true` se seguro, `false` se suspeito. | `valor => valor < 5000.00m` |
| **Cambio** | `Func<decimal, decimal>` | `decimal → decimal` | Converte valor entre moedas. Recebe valor original, retorna valor convertido. | `valor => valor * 5.40m` (USD→BRL) |

**Outras Políticas Opcionais:**
| **Parcelamento** | `Func<decimal, decimal[]>` | `decimal → decimal[]` | Divide valor em parcelas. Retorna array com valores das parcelas. | `valor => new[] {valor/3, valor/3, valor/3}` |
| **Cashback** | `Func<decimal, decimal>` | `decimal → decimal` | Calcula benefício de cashback. Retorna valor do cashback. | `valor => valor * 0.05m` (5% de volta) |

---

### **Fluxo de Execução com Políticas**

```csharp
public class PagamentoCartao : Pagamento
{
    protected override void Validar()
    {
        base.Validar(); // Validações básicas
        
        // Validações específicas do cartão
        ValidarNumeroCartao();
        ValidarDataValidade();
        
        // Aplica política de antifraude
        if (!AplicarAntifraude(Valor))
            throw new FraudeDetectadaException();
    }
    
    protected override void AutorizarOuCapturar()
    {
        decimal valorConvertido = AplicarCambio(Valor);
        
        // Comunicação com operadora de cartão
        var autorizacao = _gatewayCartao.Autorizar(valorConvertido);
        _gatewayCartao.Capturar(autorizacao);
    }
    
    protected override void Confirmar()
    {
        // Gera comprovante com código de autorização
        _comprovante = new ComprovanteCartao(_dadosTransacao);
    }
}
```

---

### **Exemplo de Uso das Políticas**

```csharp
// Configuração flexível de políticas
var pagamento = new PagamentoCartao();

// Política de antifraude personalizada
pagamento.Antifraude = valor => {
    if (valor > 10000m) return false; // Bloqueia valores muito altos
    if (valor < 1m) return false;     // Bloqueia valores muito baixos
    return true;                      // Aprova os demais
};

// Política de câmbio USD para BRL
pagamento.Cambio = valor => valor * 5.40m;

// Processa com políticas aplicadas
pagamento.Processar(); // Agnóstico às políticas específicas
```

---

### **Vantagens do Design**

1. **Extensibilidade**: Novos meios de pagamento herdam o ritual automaticamente
2. **Flexibilidade**: Políticas podem ser combinadas dinamicamente
3. **Manutenibilidade**: Alterações em políticas não afetam meios de pagamento
4. **Testabilidade**: Fácil mock de políticas para testes unitários
5. **Coesão**: Cada classe tem responsabilidade bem definida