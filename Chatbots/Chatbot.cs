using Microsoft.Extensions.AI;
using System.Text;

namespace Primerchatbot.Chatbots
{
    internal class Chatbot
    {
        internal static async Task Correr(IChatClient cliente) 
        {
            Console.WriteLine($"IA: ¡Hola! Puedes escribir tus preguntas o presionar Enter para salir");
            Console.WriteLine();

            var mensajes = new List<ChatMessage>();

            /*
             * Primero especificamos el System Prompt; 
             * que es un mensaje que le damos al modelo de lenguaje para establecer el contexto y las instrucciones para la conversación.
             
             * System Prompt
             * El System Prompt es un mensaje que se le da al modelo de lenguaje para establecer el contexto
             * y las instrucciones para la conversación. Es importante para guiar al modelo sobre cómo debe responder a las preguntas del usuario.
             * En este caso, el System Prompt establece que el asistente debe responder preguntas generales en español y
             * que las respuestas deben ser en texto plano, sin usar formatos como markdown.
             * También se pueden crear System Prompts específicos para temas como C# o Python,
             * indicando que el asistente es un experto en esos temas y que debe dar ejemplos en sus respuestas.
            */

            var systemPrompt = """
            Eres un asistente que responde preguntas generales.
            Debes responder en español.
            Las respuestas deben ser en texto plano, no usar formatos como markdown.
            """;

            var systemPromptCsharp = """
            Eres un asistente experto en C# y .NET.
            Debes responder en español y dando ejemplos.
            Las respuestas deben ser en un texto plano, no usar formatos como markdown.
            """;

            // Luego le pasamos el Rol System para indicar que este mensaje es una instrucción para el modelo de lenguaje,
            // es decir, es un mensaje que establece el contexto y las reglas para la conversación.
            mensajes.Add(new ChatMessage(role: ChatRole.System, systemPrompt));

            //Despues, iniciamos un bucle infinito para mantener el programa en ejecución
            while (true)
            {
                // Usamos StringBuilder para construir la respuesta del asistente de manera eficiente, especialmente cuando se van a concatenar muchas partes.
                var sb = new StringBuilder();

                //Indicamos el color de la consola para diferenciar la entrada del usuario
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Tu: ");

                //Luego leemos la entrada del usuario
                var entrada = Console.ReadLine();
                Console.ResetColor();

                //Si el usuario presiona Enter sin escribir nada, salir del programa
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    break;
                }

                // Luego agregamos el rol de User para indicar que este mensaje es una pregunta del usuario,
                // esto es importante para que el modelo de lenguaje entienda el contexto de la conversación
                // y pueda responder de manera adecuada. 
                mensajes.Add(new ChatMessage(role: ChatRole.User, entrada));

                // Imprimimos un mensaje para indicar que el asistente está respondiendo
                Console.WriteLine();
                Console.Write($"IA: ");

                // Obtenemos la respuesta del asistente de manera incremental (streaming)
                await foreach (var fragmento in cliente.GetStreamingResponseAsync(mensajes))
                {
                    // Agrega cada fragmento de la respuesta al StringBuilder y mostrarlo en la consola
                    sb.Append(fragmento);
                    // Muestra el fragmento en la consola a medida que se recibe para dar una experiencia de respuesta en tiempo real
                    Console.Write(fragmento);
                }

                // Por ultimo, agregamos la respuesta del asistente al historial de mensajes
                mensajes.Add(new ChatMessage(role: ChatRole.Assistant, sb.ToString()));
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
