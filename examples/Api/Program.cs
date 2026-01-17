namespace MusicalityLabs.Storage.Api;

using DataAccess.SoundSignatures;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddRouting(o => o.LowercaseUrls = true);
        
        builder.Services
            .AddRelationalDb(NpgsqlFactory.Instance, builder.Configuration.GetConnectionString("Main")!)
            .AddSingleton<SoundSignaturesRepository>();

        var app = builder.Build();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}