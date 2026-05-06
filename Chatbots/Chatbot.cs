using Google.GenAI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
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

            mensajes.Add(new ChatMessage(role: ChatRole.System, systemPrompt));

            //Bucle infinito para mantener el programa en ejecución
            while (true)
            {
                //StringBuilder es una clase que permite construir cadenas de texto de manera eficiente, especialmente cuando se van a concatenar muchas partes.
                var sb = new StringBuilder();

                //Cambiar el color de la consola para diferenciar la entrada del usuario
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Tu: ");

                //Leer la entrada del usuario
                var entrada = Console.ReadLine();
                Console.ResetColor();

                //Si el usuario presiona Enter sin escribir nada, salir del programa
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    break;
                }

                //Agregar la entrada del usuario al historial de mensajes
                mensajes.Add(new ChatMessage(role: ChatRole.User, entrada));

                Console.WriteLine();
                Console.Write($"IA: ");

                // Obtener la respuesta del asistente de manera incremental (streaming)
                await foreach (var fragmento in cliente.GetStreamingResponseAsync(mensajes))
                {
                    sb.Append(fragmento);
                    Console.Write(fragmento);
                }

                // Agregar la respuesta del asistente al historial de mensajes
                mensajes.Add(new ChatMessage(role: ChatRole.Assistant, sb.ToString()));
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
