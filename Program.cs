using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Primerchatbot;
using Primerchatbot.Chatbots;
using Primerchatbot.Inyections;

Utilidades.CargarVariableDeAmbiente();
/*
 * Aquí es donde configuramos los clientes de chat para cada proveedor de IA (OpenAI, Anthropic y Gemini) y especificamos el modelo que queremos usar.
 */
var proveedor = args.Length > 0 ? args[0].ToLowerInvariant() : "gemini";
var modeloPorDefecto = proveedor == "openai" ? "gpt-5.4-nano-2026-03-17" : proveedor == "claude" ? "claude-haiku-4-5" : "gemini-2.5-flash";
var modelo = args.Length > 1 ? args[1] : modeloPorDefecto;

Console.WriteLine($"{proveedor}: {modelo}");

var builder = Host.CreateApplicationBuilder(args);
Startup.ConfigureServices(builder, proveedor, modelo);
var host = builder.Build();

var chatClient = host.Services.GetRequiredService<IChatClient>();
await Chatbot.Correr(chatClient);
