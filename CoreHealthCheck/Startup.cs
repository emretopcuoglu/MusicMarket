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
              
             * Healthy: Bileþenlerimizin tümü saðlýklý.
             
             * Unhealthy: Bileþenlerimizin en az birinde bir sorun var ya da health check kontrolü yapýldýðý esnada unhandled
             bir exception fýrlatýlmýþ olabilir.

             * Degraded: Bu ise bileþenlerin tümünün saðlýklý olduðunu ancak kontrollerin yavaþ veya unstable olduðunu belirtiyor.
             (Basit bir database sorgusu ile baþarýlý yanýt alýyoruz ancak execution süresi beklediðimizden yüksek)

             Default olarak proje dizininde oluþturduðu healthchecks.db(sqlite), UI'da gösterilen bilgileri saklamaktadýr.
             Bir çok DB için konfigüre edilebilir.

            AspNetCore.Diagnostics.HealthChecks kütüphanesinin kullanýma sunduðu hali hazýrdaki health check
            yapýlarýndan bazýlarý þunlardýr;
            System, Network, SqlServer, MongoDb, Elasticsearch, Solr, Redis, RavenDb, Kafka, RabbitMQ, Oracle, AWS.S3,
            Hangfire, SignalR, Kubernetes...
             */

            services.AddControllers();

            // https://localhost:44338/hc ile incelenir (UI olmadan) - SQL baðlantýsý durumu
            services.AddHealthChecks()
                .AddSqlServer(
                Configuration.GetConnectionString("DevConnection"),
                "SELECT 1;",
                $"Sql => {Configuration.GetConnectionString("DevConnection").Split(";").First()}",
                HealthStatus.Degraded,
                timeout: TimeSpan.FromSeconds(30),
                tags: new[] { "db", "sql", "sqlServer", });

            // https://localhost:44338/hc-ui üzerinden incelenir benzer þekilde appsettings.json'dan da yazýlabilir
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
