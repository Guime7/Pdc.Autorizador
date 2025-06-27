using Microsoft.Extensions.DependencyInjection;
using Pdc.Autorizador.Dominio.Factory;
using Pdc.Autorizador.Dominio.Interfaces.Factory;

namespace Pdc.Autorizador.Aplicacao.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioFactory, UsuarioFactory>();
        
        return services;
    }
}