using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .UseServiceProviderFactory(new AutofacServiceProviderFactory())    //Yar�n �b�rg�n Autofac kullanmaktan vazgecersek,
                .ConfigureContainer<ContainerBuilder>(builder =>                   //Depency Resolvers da yeni yapm�z� olu�turup;
                {                                                                  //Wep API program.cs de yazd���m�z bu yap�n�n
                    builder.RegisterModule(new AutofacBusinessModule());           //sadece new le di�imiz k�s�mlar�n� de�i�tirmemiz
                })                                                                 //yeterli olacakt�r.
                                                                                   //ancak startup kullanacaksan yap�y� sil.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
