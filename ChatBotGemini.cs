using Google.GenAI;
using Google.GenAI.Types;
using System.Text;
using Environment = System.Environment;

namespace Primerchatbot
{
    internal class ChatBotGemini
    {
        internal static async Task Correr() 
        {
            var modelo = "gemini-2.5-flash";
            var llave = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if (string.IsNullOrWhiteSpace(llave))
            {
                Console.WriteLine("No se encontró la variable de ambiente GEMINI_API_KEY.");
                return;
            }

            var cliente = new Client(apiKey: llave);

            Console.WriteLine($"IA: ¡Hola! Puedes escribir tus preguntas o presionar Enter para salir");
            Console.WriteLine();

            var mensajes = new List<Content>();

            var systemPrompt = """
            Eres un asistente que responde preguntas generales.
            Debes responder en español.
            Las respuestas deben ser en texto plano, no usar formatos como markdown.
            """;

            // Configurar la generación de contenido
            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts = new List<Part>
                    {
                        new Part { Text = systemPrompt }
                    }
                },
                Temperature = 0.7,
                MaxOutputTokens = 1024
            };

            while (true)
            {
                var sb = new StringBuilder();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Tu: ");

                var entrada = Console.ReadLine();
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    break;
                }

                // Agregar el mensaje del usuario a la conversación
                mensajes.Add(new Content
                {
                    Role = "user",
                    Parts = new List<Part>
                    {
                        new Part { Text = entrada }
                    }
                });

                Console.WriteLine();
                Console.Write("IA: ");

                // Generar la respuesta del modelo en streaming
                await foreach (var chunk in cliente.Models.GenerateContentStreamAsync(
                    model: modelo,
                    contents: mensajes,
                    config: config))
                {
                    // Extraer el texto del chunk y mostrarlo en la consola
                    var texto = chunk.Candidates?
                        .FirstOrDefault()?
                        .Content?
                        .Parts?
                        .FirstOrDefault()?
                        .Text;

                    // Agregar el texto al StringBuilder y mostrarlo en la consola
                    if (!string.IsNullOrEmpty(texto))
                    {
                        // Agregar el texto al StringBuilder
                        sb.Append(texto);
                        Console.Write(texto);
                    }
                }

                // Agregar la respuesta completa del modelo a la conversación
                mensajes.Add(new Content
                {
                    Role = "model",
                    Parts = new List<Part>
                    {
                        new Part { Text = sb.ToString() }
                    }
                });

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
