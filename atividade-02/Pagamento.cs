using System;

public class Pagamento
{
    private readonly Func<decimal, bool>? _antifraude;
    private readonly Func<decimal, decimal>? _cambio;

    // Valor processado disponível para as subclasses
    protected decimal ValorProcessado { get; private set; }

    public Pagamento(Func<decimal, bool>? antifraude = null, Func<decimal, decimal>? cambio = null)
    {
        _antifraude = antifraude;
        _cambio = cambio;
    }

    // Template Method - ritual fixo
    public void Processar()
    {
        Validar();

        decimal valor = ObterValor();
        valor = AplicarCambio(valor);

        if (!AplicarAntifraude(valor))
            throw new InvalidOperationException("Pagamento bloqueado pela política de antifraude");

        ValorProcessado = valor;

        AutorizarOuCapturar();
        Confirmar();
    }

    // Ganchos protegidos
    protected virtual void Validar() { }

    // Forma simples de obter valor do pagamento (padrão)
    protected virtual decimal ObterValor() => 100m;

    protected virtual void AutorizarOuCapturar() { }
    protected virtual void Confirmar() { }

    // Políticas plugáveis
    protected bool AplicarAntifraude(decimal valor)
    {
        return _antifraude?.Invoke(valor) ?? true; // aprova se não houver política
    }

    protected decimal AplicarCambio(decimal valor)
    {
        return _cambio?.Invoke(valor) ?? valor;
    }

    // Permite configurar via propriedades públicas (apenas set) - opcional, bloqueado neste exercício
    public Func<decimal, bool> Antifraude { set => throw new InvalidOperationException("Use o construtor para injetar políticas neste exercício."); }
    public Func<decimal, decimal> Cambio { set => throw new InvalidOperationException("Use o construtor para injetar políticas neste exercício."); }
}
