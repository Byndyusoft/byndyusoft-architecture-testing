namespace MusicalityLabs.Storage.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    internal class Program
    {
        public static void Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
    }
}