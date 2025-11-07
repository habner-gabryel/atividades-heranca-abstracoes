using System;

// Demo: testes de LSP e composição por delegates

static void TestProcessar(Pedido pedido)
{
	Console.WriteLine($"--- Processando: {pedido.GetType().Name} ---");
	pedido.Processar();
	Console.WriteLine();
}

// Delegates de composição (frete e promoção)
Func<decimal, decimal> freteFixo = valor => valor + 25.00m;
Func<decimal, decimal> fretePercentual = valor => valor * 1.10m; // +10%

Func<decimal, decimal> promocaoNenhuma = valor => valor;
Func<decimal, decimal> promocaoCupom = valor => valor - 15.00m;

// 1) Teste LSP: a função aceita a base Pedido e funciona para as derivadas
TestProcessar(new PedidoNacional());
TestProcessar(new PedidoInternacional());

// 2) Demonstração de composição: injetando delegates diferentes
Console.WriteLine("=== Composição: frete fixo, sem promoção (Nacional) ===");
var p1 = new PedidoNacional(freteFixo, promocaoNenhuma);
p1.Processar();

Console.WriteLine();
Console.WriteLine("=== Composição: frete percentual, cupom (Internacional) ===");
var p2 = new PedidoInternacional(fretePercentual, promocaoCupom);
p2.Processar();

Console.WriteLine();
Console.WriteLine("=== Troca de políticas criando nova instância (mesma classe) ===");
Console.WriteLine("Demonstrando sem criar subclasses: reutilizamos a classe com delegates diferentes.");
var p3 = new PedidoNacional(fretePercentual, promocaoCupom);
p3.Processar();

Console.WriteLine("Fim dos testes.");
