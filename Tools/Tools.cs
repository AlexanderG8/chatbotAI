using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Primerchatbot.Services;

namespace Primerchatbot.Tools
{
    internal static class Tools
    {
        internal static IEnumerable<AITool> ObtenerTools(this IServiceProvider sp) 
        {
            var servicioClima = sp.GetRequiredService<IServicioClima>();
            yield return AIFunctionFactory.Create(
                servicioClima.ObtenerClima,
                new AIFunctionFactoryOptions 
                {
                    Name = "obtener_clima",
                    Description = "Obtiene el clima de una ciudad indicada",
                }
                );

            // Creamos una herramienta para evaluar condiciones climáticas utilizando el servicio ServicioEvaluaCondiciones
            var servicioEvaluaCondiciones = sp.GetRequiredService<ServicioEvaluaCondiciones>();
            yield return AIFunctionFactory.Create(
                servicioEvaluaCondiciones.EvaluarCondiciones,
                new AIFunctionFactoryOptions 
                {
                    Name = "evaluar_condiciones_clima",
                    Description = "Evalúa una condición climática (por ejemplo: 'soleado', 'lluvia ligera', 'nublado') y determina si es un buen momento para realizar actividades al aire libre."
                }
                );

            // Aquí agregamos la herramienta para obtener un correo falso utilizando el servicio ServicioObtenerCorreoFalso.
            // Notemos que no es necesario agregar el atributo AIFunctionFactoryOptions con el name o description,
            // ya que el método ObtenerCorreoFalso ya tiene el atributo Description con esa información.
            var servicioObtenerCorreoFalso = sp.GetRequiredService<ServicioObtenerCorreoFalso>();
            yield return AIFunctionFactory.Create(servicioObtenerCorreoFalso.ObtenerCorreoFalso);


            // Luego aquí agregamos la herramienta para enviar un correo utilizando el servicio ServicioEnviarCorreoFalso.
            // Notemos que al igual que ObtenerCorreoFalso, no es necesario agregar el atributo AIFunctionFactoryOptions con el name o description,
            // ya que el método EnviarCuerpo ya tiene el atributo Description con esa información.
            var servicioCorreos = sp.GetRequiredService<ServicioEnviarCorreoFalso>();
            var functionEnviarCorreo = AIFunctionFactory.Create(servicioCorreos.EnviarCorreo);
            // Ahora agregamos el atributo ApprovalRequiredAIFunction para indicar que esta función requiere aprobación antes de ser ejecutada por el chatbot.
            yield return new ApprovalRequiredAIFunction(functionEnviarCorreo);
        }
    }
}
