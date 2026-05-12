using Anthropic.Models.Beta.Messages;
using Microsoft.Extensions.AI;
using System.Text;
using System.Text.Json;

namespace Primerchatbot.Chatbots
{
    internal class Chatbot
    {
        internal static async Task Correr(IChatClient cliente) 
        {
            Console.WriteLine($"IA: ¡Hola! Puedes escribir tus preguntas o presionar Enter para salir");
            Console.WriteLine();

            var mensajes = new List<ChatMessage>();

            var systemPrompt = """
            Eres un asistente que responde preguntas generales.
            Debes responder en español.
            Las respuestas deben ser en texto plano, no usar formatos como markdown.

            Si un tool falla lee el mensaje de la excepción para ver si puedes arreglarlo haciendo algun ajuste. Comunicale al usuario cualquier ajuste que vayas a hacer.
            """;

            var systemPromptCsharp = """
            Eres un asistente experto en C# y .NET.
            Debes responder en español y dando ejemplos.
            Las respuestas deben ser en un texto plano, no usar formatos como markdown.
            """;

            mensajes.Add(new ChatMessage(role: ChatRole.System, systemPrompt));

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Tu: ");

                var entrada = Console.ReadLine();
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    break;
                }

                mensajes.Add(new ChatMessage(role: ChatRole.User, entrada));

                Console.WriteLine();
                Console.Write($"IA: ");

                while (true) 
                {
                    var updates = new List<ChatResponseUpdate>();

                    await foreach (var responseUpdate in cliente.GetStreamingResponseAsync(mensajes))
                    {
                        updates.Add(responseUpdate);

                        foreach (var contenido in responseUpdate.Contents) 
                        {
                            if (contenido is TextContent contenidoTexto) 
                            {
                                Console.Write(contenidoTexto);
                            }
                        }
                    }

                    var respuesta = updates.ToChatResponse();
                    mensajes.AddMessages(respuesta);

                    var solicitudAprobacion = respuesta.Messages
                                            .SelectMany(x => x.Contents)
                                            .OfType<ToolApprovalRequestContent>()
                                            .FirstOrDefault();

                    if (solicitudAprobacion is not null)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("La IA desea ejecutar la siguiente función:");

                        if (solicitudAprobacion.ToolCall is FunctionCallContent functionCall) 
                        {
                            Console.WriteLine($"Tool: {ConvertirNombreDeFuncion(functionCall.Name)}");

                            if (functionCall.Arguments is not null) 
                            {
                                foreach (var argumento in functionCall.Arguments) 
                                {
                                    Console.WriteLine($"{argumento.Key}: {argumento.Value}");
                                }
                            }
                        }

                        Console.ResetColor();
                        Console.Write("¿Desea aprobar esta acción? (s/n): ");

                        var aprobada = Console.ReadLine()?.Trim().ToLower() == "s";
                        var respuestaAprobacion = solicitudAprobacion.CreateResponse(aprobada);

                        mensajes.Add(new ChatMessage(ChatRole.User, [respuestaAprobacion]));

                        Console.WriteLine();
                        Console.Write("IA: ");
                        continue;
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                }
            }
        }

        private static string ConvertirNombreDeFuncion(string nombre) 
        {
            return nombre switch
            {
                "EnviarCorreo" => "Enviar correo",
                _ => nombre
            };
        }
    }
}
