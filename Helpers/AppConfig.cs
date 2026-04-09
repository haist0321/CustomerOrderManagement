using Microsoft.Extensions.Configuration;
using System.IO;

namespace CustomerOrderManagement.Helpers
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; }

        static AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // nơi chạy exe
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static string GetUsername() => Configuration["Login:Username"];
        public static string GetPassword() => Configuration["Login:Password"];
    }
}
