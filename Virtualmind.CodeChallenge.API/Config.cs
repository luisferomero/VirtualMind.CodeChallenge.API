using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.DataAccess.Contexts;

namespace Virtualmind.CodeChallenge.API
{
    public static class Config
    {
        public static void ConfigDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CurrenciesDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString("CurrencyExchange"));
            });
        }
        public static void CofigSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Virtualmind code challenge",
                    Description = "Virtualmind codechallenge API",
                    Contact = new OpenApiContact
                    {
                        Name = "Luis Romero",
                        Email = "Rl.luisfe@gmail.com",
                        Url = new Uri("https://luisferomero.github.io/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigServices(IServiceCollection services)
        {
            string[] projects = new string[] { "Virtualmind.CodeChallenge.BusinessLogic" };

            foreach (string project in projects)
            {
                List<Type> listImplementations = Assembly.Load(project).GetTypes()
                    .Where(x => x.Name.EndsWith("Service") && !x.IsInterface)
                    .ToList();

                foreach (Type implementation in listImplementations)
                {
                    Type implentationInterface = implementation.GetInterface("I" + implementation.Name);

                    if (implentationInterface != null)
                        services.AddScoped(implentationInterface, implementation);
                    else
                        services.AddScoped(implementation);
                }
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
