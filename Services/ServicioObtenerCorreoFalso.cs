using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Primerchatbot.Services
{
    // Servicio para obtener un correo falso basado en el nombre de una persona
    internal class ServicioObtenerCorreoFalso
    {
        // Método que genera un correo falso utilizando el nombre proporcionado
        // Descripción es importante para que el chatbot pueda entender la función y sus parámetros
        [Description("Obtene el correo de una persona")]
        public string ObtenerCorreoFalso([Description("Nombre de la persona")]string nombre) => $"{nombre}@ejemplo.com";
    }
}
