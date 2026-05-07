namespace Primerchatbot.Services
{
    // Esta clase es una implementación falsa del servicio de clima,
    // que simula la obtención del clima para diferentes ciudades sin necesidad de conectarse a un servicio real.
    internal class ServicioClimaFalso : IServicioClimaFalso
    {
        public async Task<string> ObtenerClima(string ciudad)
        {
            return ciudad.ToLower() switch
            {
                "londres" => "El clima en Londres es lluvioso con una temperatura de 15°C.",
                "paris" => "El clima en París es soleado con una temperatura de 22°C.",
                "nueva york" => "El clima en Nueva York es nublado con una temperatura de 18°C.",
                _ => $"Lo siento, no tengo información sobre el clima en {ciudad}."
            };
        }
    }
}
