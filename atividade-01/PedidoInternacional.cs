using System;

public sealed class PedidoInternacional : Pedido
{
    public PedidoInternacional(Func<decimal, decimal>? frete = null, Func<decimal, decimal>? promocao = null)
        : base(frete, promocao)
    {
    }

    protected override void Validar()
    {
        // Preserva validações da base e adiciona checagens específicas
        base.Validar();
        // (Simulado) Ex.: verificar câmbio, documentação de exportação
    }

    // Exemplo de cálculo com taxas de importação e custo aduaneiro
    protected override decimal CalcularSubtotal()
    {
        decimal baseSubtotal = base.CalcularSubtotal();
        // +20% taxas + custo fixo de 50
        return baseSubtotal * 1.20m + 50.00m;
    }

    protected override string EmitirRecibo(decimal total)
    {
        return $"Recibo (Internacional): {total:C} - Commercial Invoice";
    }
}
