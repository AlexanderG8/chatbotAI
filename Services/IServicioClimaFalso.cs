namespace Primerchatbot.Services
{
    internal interface IServicioClimaFalso
    {
        Task<string> ObtenerClima(string ciudad);
    }
}