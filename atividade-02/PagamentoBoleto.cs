using System;

public sealed class PagamentoBoleto : Pagamento
{
    public PagamentoBoleto(Func<decimal, bool>? antifraude = null, Func<decimal, decimal>? cambio = null)
        : base(antifraude, cambio)
    {
    }

    protected override void Validar()
    {
        base.Validar();
        // Validações de boleto (data de vencimento etc.)
    }

    protected override void AutorizarOuCapturar()
    {
        // Gera linha digitável (simulado)
    }

    protected override void Confirmar()
    {
        Console.WriteLine($"Boleto gerado: Valor processado = {ValorProcessado:C}");
    }
}
