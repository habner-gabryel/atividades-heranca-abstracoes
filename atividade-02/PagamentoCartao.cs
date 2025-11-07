using System;

public sealed class PagamentoCartao : Pagamento
{
    public PagamentoCartao(Func<decimal, bool>? antifraude = null, Func<decimal, decimal>? cambio = null)
        : base(antifraude, cambio)
    {
    }

    protected override void Validar()
    {
        base.Validar();
        // Validações do cartão (número, CVV, validade) - simuladas
    }

    protected override void AutorizarOuCapturar()
    {
        // Simula chamada à operadora
        Console.WriteLine($"Autorizando cartão (simulado): {ValorProcessado:C}");
    }

    protected override void Confirmar()
    {
        Console.WriteLine($"Comprovante Cartão: Valor processado = {ValorProcessado:C}");
    }
}
