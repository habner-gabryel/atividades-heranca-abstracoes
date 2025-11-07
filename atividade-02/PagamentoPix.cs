using System;

public sealed class PagamentoPix : Pagamento
{
    public PagamentoPix(Func<decimal, bool>? antifraude = null, Func<decimal, decimal>? cambio = null)
        : base(antifraude, cambio)
    {
    }

    protected override void Validar()
    {
        base.Validar();
    }

    protected override void AutorizarOuCapturar()
    {
        // Simula geração de payload e confirmação
    }

    protected override void Confirmar()
    {
        Console.WriteLine($"Comprovante PIX: Valor processado = {ValorProcessado:C}");
    }
}
