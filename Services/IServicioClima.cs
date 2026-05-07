namespace Primerchatbot.Services
{
    internal interface IServicioClima
    {
        Task<string> ObtenerClima(string ciudad);
    }
}