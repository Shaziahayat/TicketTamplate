using Configuration;
using DbContext;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Services;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TicketTamplate.Pages;
using TicketTamplate.Services;

namespace TicketTamplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<csMainDbContext>(options =>
            {
                var connectionString = AppConfig.ConfigurationRoot["DbLogins"];
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddSingleton<LanguageService>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
           
            builder.Services.AddRazorPages().AddViewLocalization().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                    return factory.Create("SharedResource", assemblyName.Name);
                };
            });

            var defaultCulture = builder.Configuration["DefaultCulture"];

            //  defaultCulture string to a CultureInfo object
            var defaultCultureInfo = new CultureInfo(defaultCulture);

            //  Request Localization Options
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCultureInfo);
                options.SupportedCultures = new[] { defaultCultureInfo };
                options.SupportedUICultures = new[] { defaultCultureInfo };
            });


            builder.Services.AddScoped<ITicketService, TicketService>();
          
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }

}