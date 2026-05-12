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
            // Validaciones básicas para simular errores comunes al enviar un correo
            if (!string.IsNullOrWhiteSpace(asunto) && asunto.Length > 0) 
            {
                var primeraLetra = asunto[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper()) 
                {
                    throw new Exception("Error con el asunto del correo. La primera letra de este debe ser en mayuscula");
                }
            }
            /*************************************************************************/

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
