using System;

public sealed class PedidoNacional : Pedido
{
    public PedidoNacional(Func<decimal, decimal>? frete = null, Func<decimal, decimal>? promocao = null)
        : base(frete, promocao)
    {
    }

    // Exemplo de cálculo com impostos internos
    protected override decimal CalcularSubtotal()
    {
        // Base + 10% impostos
        decimal baseSubtotal = base.CalcularSubtotal();
        return baseSubtotal * 1.10m;
    }

    protected override string EmitirRecibo(decimal total)
    {
        return $"Recibo (Nacional): {total:C} - NF-e";
    }
}
