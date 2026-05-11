using Anthropic;
using Google.GenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Primerchatbot.Services;

namespace Primerchatbot.Inyections
{
    internal static class Startup
    {
        public static void ConfigureServices(HostApplicationBuilder builder, string proveedor, string? modelo)
        {
            var modeloOpenAI = "gpt-5.4-nano-2026-03-17";
            var modeloAnthropic = "claude-haiku-4-5";
            var modeloGemini = "gemini-2.5-flash";

            string llaveOpenAI = Environment.GetEnvironmentVariable("OPENAI_KEY");
            string llaveAnthropic = Environment.GetEnvironmentVariable("ANTHROPIC_KEY")!;
            string llaveGemini = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            //builder.Services.AddTransient<IServicioClimaFalso, ServicioClimaFalso>();
            builder.Services.AddTransient<IServicioClima, ServicioClimaOpenWeather>();
            builder.Services.AddTransient<ServicioEvaluaCondiciones>();

            builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.None);
            builder.Services.AddHttpClient();

            // Agregamos servicios relacionados con correo electrónico falso
            builder.Services.AddTransient<ServicioEnviarCorreoFalso>();
            builder.Services.AddTransient<ServicioObtenerCorreoFalso>();

            builder.Services.AddSingleton<IChatClient>(sp => 
            {
                var cliente = proveedor switch
                {
                    "openai" => new OpenAI.Chat.ChatClient(modelo ?? modeloOpenAI, llaveOpenAI).AsIChatClient(),
                    "claude" => new AnthropicClient() { ApiKey = llaveAnthropic }.AsIChatClient().AsBuilder().ConfigureOptions(x => x.ModelId = modelo ?? modeloAnthropic).Build(),
                    "gemini" => new Client(apiKey: llaveGemini).AsIChatClient().AsBuilder().ConfigureOptions(x => x.ModelId = modelo ?? modeloGemini).Build(),
                    _ => throw new ArgumentException("Proveedor no soportado. Por favor, elige 'openai', 'claude' o 'gemini'.")
                };
                return cliente.AsBuilder()
                .ConfigureOptions(x => 
                {
                    x.MaxOutputTokens = 2048;
                    x.Temperature = 0.7f;
                    x.Tools = [.. Tools.Tools.ObtenerTools(sp)];
                })
                //.Use(async (mensaje, opciones, next, cancellationToken) => 
                //{
                //    Console.WriteLine();
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine($"Analizando prompt...");
                //    Console.ResetColor();

                //    await next(mensaje, opciones, cancellationToken);

                //    Console.WriteLine();
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine($"Se brindo respuesta...");
                //    Console.ResetColor();

                //})
                .UseFunctionInvocation()
                .Build(sp);
            });
        }
    }
}
