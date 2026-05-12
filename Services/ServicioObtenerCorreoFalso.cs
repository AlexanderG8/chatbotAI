using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Primerchatbot.Services
{
    internal class ServicioObtenerCorreoFalso
    {
        [Description("Obtene el correo de una persona")]
        public string ObtenerCorreoFalso([Description("Nombre de la persona")]string nombre) => $"{nombre}@ejemplo.com";
    }
}
