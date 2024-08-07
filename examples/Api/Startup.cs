namespace MusicalityLabs.Storage.Api
{
    using System;
    using DataAccess.SoundSignatures;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Npgsql;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers();

            services
                .AddRouting(o => o.LowercaseUrls = true);

            services
                .AddRelationalDb(NpgsqlFactory.Instance, _configuration.GetConnectionString("Main"))
                .AddSingleton<SoundSignaturesRepository>();
        }

        public void Configure(IApplicationBuilder app)
            => app
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}