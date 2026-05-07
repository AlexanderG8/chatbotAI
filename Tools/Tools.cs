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
        }
    }
}
