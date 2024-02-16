using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace CoreHealthCheck
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            /*
              
             * Healthy: Bile�enlerimizin t�m� sa�l�kl�.
             
             * Unhealthy: Bile�enlerimizin en az birinde bir sorun var ya da health check kontrol� yap�ld��� esnada unhandled
             bir exception f�rlat�lm�� olabilir.

             * Degraded: Bu ise bile�enlerin t�m�n�n sa�l�kl� oldu�unu ancak kontrollerin yava� veya unstable oldu�unu belirtiyor.
             (Basit bir database sorgusu ile ba�ar�l� yan�t al�yoruz ancak execution s�resi bekledi�imizden y�ksek)

             Default olarak proje dizininde olu�turdu�u healthchecks.db(sqlite), UI'da g�sterilen bilgileri saklamaktad�r.
             Bir �ok DB i�in konfig�re edilebilir.

            AspNetCore.Diagnostics.HealthChecks k�t�phanesinin kullan�ma sundu�u hali haz�rdaki health check
            yap�lar�ndan baz�lar� �unlard�r;
            System, Network, SqlServer, MongoDb, Elasticsearch, Solr, Redis, RavenDb, Kafka, RabbitMQ, Oracle, AWS.S3,
            Hangfire, SignalR, Kubernetes...
             */

            services.AddControllers();

            // https://localhost:44338/hc ile incelenir (UI olmadan) - SQL ba�lant�s� durumu
            services.AddHealthChecks()
                .AddSqlServer(
                Configuration.GetConnectionString("DevConnection"),
                "SELECT 1;",
                $"Sql => {Configuration.GetConnectionString("DevConnection").Split(";").First()}",
                HealthStatus.Degraded,
                timeout: TimeSpan.FromSeconds(30),
                tags: new[] { "db", "sql", "sqlServer", });

            // https://localhost:44338/hc-ui �zerinden incelenir benzer �ekilde appsettings.json'dan da yaz�labilir
            services
            .AddHealthChecksUI(s =>
            {
                s.AddHealthCheckEndpoint("https status", "https://localhost:44338/hc");
            })
            .AddSqliteStorage("Data Source=healthchecks.db");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = registration => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = "/hc-ui";
            }
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
        }
    }
}
