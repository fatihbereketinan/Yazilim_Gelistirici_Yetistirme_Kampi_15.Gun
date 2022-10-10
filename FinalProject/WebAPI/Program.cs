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

                .UseServiceProviderFactory(new AutofacServiceProviderFactory())    //Yarýn öbürgün Autofac kullanmaktan vazgecersek,
                .ConfigureContainer<ContainerBuilder>(builder =>                   //Depency Resolvers da yeni yapmýzý oluþturup;
                {                                                                  //Wep API program.cs de yazdýðýmýz bu yapýnýn
                    builder.RegisterModule(new AutofacBusinessModule());           //sadece new le diðimiz kýsýmlarýný deðiþtirmemiz
                })                                                                 //yeterli olacaktýr.
                                                                                   //ancak startup kullanacaksan yapýyý sil.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
