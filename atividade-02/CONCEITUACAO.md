**Documento de Conceituação - Fase 1**

---

### **Ritual Comum de Processamento de Pagamentos**

O processamento segue um fluxo fixo de três etapas principais:

1. **Validar** - Verificar dados essenciais e viabilidade do pagamento
2. **Autorizar/Capturar** - Garantir fundos/reserva do valor e efetivar a transação  
3. **Confirmar** - Finalizar o processo e gerar comprovante

Este ritual é **invariável** para todos os meios de pagamento, garantindo consistência no processamento.

---

### **Variações entre Meios de Pagamento**

| **Meio** | **Validação Específica** | **Autorização/Captura** | **Comprovante** |
|----------|--------------------------|--------------------------|-----------------|
| **Cartão** | Dados do cartão, CVV, limite | Autorização + captura junto à operadora | Comprovante com código de autorização |
| **PIX** | Chave PIX, limite temporal | Geração de QR Code + confirmação via banco | Comprovante PIX com QR e ID da transação |
| **Boleto** | Data de vencimento, beneficiário | Geração de linha digitável + aguardar compensação | Boleto com instruções e linha digitável |

**Diferenças cruciais**:
- **Cartão**: Requer comunicação em tempo real com operadora para autorização
- **PIX**: Geração de payload dinâmico e confirmação assíncrona
- **Boleto**: Processamento assíncrono com compensação bancária

---

### **Justificativa para Herança dos Meios de Pagamento**

A **herança** é apropriada porque:

1. **Ritual Fixo**: O fluxo `Validar → Autorizar/Capturar → Confirmar` é comum a todos os meios
2. **Especialização Controlada**: Cada meio implementa o **"como"** de cada etapa, mas não altera a sequência
3. **Template Method Natural**: 
   - Classe base `Pagamento` define o esqueleto do processo
   - Subclasses (`PagamentoCartao`, `PagamentoPIX`, `PagamentoBoleto`) especializam cada etapa
4. **Manutenção Coesa**: Alterações específicas de cada meio ficam isoladas em suas classes

**Exemplo de especialização**:
```csharp
// Base
public abstract class Pagamento
{
    public void Processar() // Template Method
    {
        Validar();
        AutorizarCapturar();
        Confirmar();
    }
    
    protected abstract void Validar();
    protected abstract void AutorizarCapturar();
    protected abstract void Confirmar();
}
```

---

### **Justificativa para Composição das Políticas**

A **composição** (via delegates) é ideal para políticas porque:

1. **Independência**: Antifraude e câmbio são **ortogonais** ao meio de pagamento
   - Um pagamento por PIX pode precisar de câmbio
   - Um pagamento por cartão pode precisar de antifraude
   - Ambos podem precisar das duas políticas

2. **Combinabilidade Flexível**:
   ```csharp
   var pagamento = new PagamentoCartao();
   pagamento.Antifraude = politicaAntifraudeAvançada;
   pagamento.Cambio = conversorUSD_BRL;
   // Ou nenhuma, ou ambas, ou qualquer combinação
   ```

3. **Baixo Acoplamento**:
   - Políticas podem evoluir independentemente
   - Novas políticas (ex.: parcelamento, cashback) adicionadas sem modificar hierarquia existente
   - Fácil teste com políticas mock

4. **Open/Closed Principle**:
   - Novas políticas criadas sem alterar código dos meios de pagamento
   - Meios de pagamento novos herdam automaticamente capacidade de usar políticas existentes

---

### **Conclusão Arquitetural**

- **Herança** organiza as **variações essenciais** entre meios de pagamento (cartão/PIX/boleto)
- **Composição** gerencia **funcionalidades transversais** (antifraude, câmbio) que cortam verticalmente todos os meios
- **Separação de Concerns**: O "o quê" (ritual) é fixo na hierarquia, o "como" (implementações) é especializado, e os "extras" (políticas) são compostos