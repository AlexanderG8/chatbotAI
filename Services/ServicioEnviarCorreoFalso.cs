using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Primerchatbot.Services
{
    internal class ServicioEnviarCorreoFalso
    {
        [Description("Envia un correo a un destinatario.")]
        public Task EnviarCorreo(
                [Description("Cuerpo del correo")]string cuerpo,
                [Description("Asuento del correo")]string asunto,
                [Description("Correo del destintario")]string destinatario
            ) 
        {
            Console.WriteLine("Enviando el correo...");

            Console.WriteLine($"""
                Destinatario: {destinatario}
                Asunto: {asunto}
                Cuerpo: 
                {cuerpo}
            """);

            Console.WriteLine("Correo enviado...");

            return Task.CompletedTask;
        }
    }
}
