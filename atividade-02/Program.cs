using System;

// Função que aceita o tipo base Pagamento (teste de LSP)
static void TestProcessar(Pagamento pagamento)
{
    Console.WriteLine($"--- Processando: {pagamento.GetType().Name} ---");
    try
    {
        pagamento.Processar();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
    Console.WriteLine();
}

// Políticas de exemplo
Func<decimal, bool> antifraudeSempre = v => true;
Func<decimal, bool> antifraudeLimite = v => v < 150m; // aprova apenas valores < 150

Func<decimal, decimal> cambioSemTaxa = v => v;
Func<decimal, decimal> cambioComTaxa = v => v * 1.05m; // +5%

// Teste LSP: a função aceita a base Pagamento
TestProcessar(new PagamentoCartao());
TestProcessar(new PagamentoPix());
TestProcessar(new PagamentoBoleto());

// Demonstração de composição: injetando delegates diferentes
Console.WriteLine("=== Composição: antifraude limitadora, sem câmbio (Cartão) ===");
var cartao1 = new PagamentoCartao(antifraudeLimite, cambioSemTaxa);
TestProcessar(cartao1);

Console.WriteLine("=== Composição: antifraude sempre aprova, câmbio com taxa (PIX) ===");
var pix1 = new PagamentoPix(antifraudeSempre, cambioComTaxa);
TestProcessar(pix1);

Console.WriteLine("=== Composição: antifraude que bloqueia tudo (Boleto) ===");
var boletoBloqueado = new PagamentoBoleto(v => false, cambioSemTaxa);
TestProcessar(boletoBloqueado);

Console.WriteLine("Fim dos testes.");
