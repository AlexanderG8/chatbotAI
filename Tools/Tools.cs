using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Primerchatbot.Services;

namespace Primerchatbot.Tools
{
    internal static class Tools
    {
        internal static IEnumerable<AITool> ObtenerTools(this IServiceProvider sp) 
        {
            var servicioClimaFalso = sp.GetRequiredService<IServicioClimaFalso>();
            yield return AIFunctionFactory.Create(
                servicioClimaFalso.ObtenerClima,
                new AIFunctionFactoryOptions 
                {
                    Name = "ObtenerClima",
                    Description = "Obtiene el clima de una ciudad indicada",
                }
                );
        }
    }
}
