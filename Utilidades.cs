using System;
using System.Collections.Generic;
using System.Text;

namespace Primerchatbot
{
    internal class Utilidades
    {
        internal static void CargarVariableDeAmbiente() 
        {
            foreach (var linea in File.ReadAllLines(".env")) 
            {
                var partes = linea.Split("=");
                if (partes.Length == 2) 
                {
                    // Llave valor
                    // Establecer la variable de ambiente
                    Environment.SetEnvironmentVariable(partes[0], partes[1]);
                }
            }
        }
    }
}
