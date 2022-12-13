using Exentials.ReCache.Client;
using Exentials.ReCache.WebClientDemo.Data;

namespace Exentials.ReCache.WebClientDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.Configure<ReCacheClientOptions>(builder.Configuration.GetSection(ReCacheClientOptions.ReCache));
            builder.Services.AddReCacheClient();

            /*
            builder.Services.AddReCacheClient(options =>
            {
                options.SslUrl = "https://localhost:5001";
                // options.SslUrl = "https://localhost";
                options.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZGVmYXVsdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkNsaWVudCIsImV4cCI6MTcwMTYyODE0Mn0.e5gPktQXyjvpjXgTvyp-RxKwZvLu7sF4KW4C5kuV1lA";
                options.KeepAlive = true;
                options.IgnoreSslCertificate = true;
            });
            */

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}