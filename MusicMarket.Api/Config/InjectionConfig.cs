using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using MusicMarket.Core.Services;
using MusicMarket.Core;
using MusicMarket.Data;
using MusicMarket.Services;

namespace MusicMarket.Api.Config
{
    public static class InjectionConfig
    {
        public static void AddInjections(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();
        }
    }
}
