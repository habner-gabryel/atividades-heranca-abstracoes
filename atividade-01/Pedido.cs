using System;

public class Pedido
{
    private readonly Func<decimal, decimal>? _frete;
    private readonly Func<decimal, decimal>? _promocao;

    // Construtor recebe delegates para composição (injeção de estratégias)
    public Pedido(Func<decimal, decimal>? frete = null, Func<decimal, decimal>? promocao = null)
    {
        _frete = frete;
        _promocao = promocao;
    }

    // Ritual fixo
    public void Processar()
    {
        Validar();
        decimal subtotal = CalcularSubtotal();
        decimal total = AplicarPoliticasPlugaveis(subtotal);
        string recibo = EmitirRecibo(total);
        Console.WriteLine(recibo);
    }

    // Ganchos protegidos para especialização
    protected virtual void Validar() { }

    protected virtual decimal CalcularSubtotal() => 100m;

    protected virtual string EmitirRecibo(decimal total) => $"Recibo: {total:C}";

    private decimal AplicarPoliticasPlugaveis(decimal subtotal)
    {
        decimal valorAtual = subtotal;
        if (_frete != null) valorAtual = _frete(valorAtual);
        if (_promocao != null) valorAtual = _promocao(valorAtual);
        return valorAtual;
    }
}
