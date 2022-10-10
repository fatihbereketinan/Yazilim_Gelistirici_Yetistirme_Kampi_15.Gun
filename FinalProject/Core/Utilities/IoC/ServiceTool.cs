using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        //Bu kod bizim wepapi veya afutofacte oluşturduğumuz injectionları oluşturabilmemizi sağlıyor.
        public static IServiceCollection Create(IServiceCollection services)
        {
            //.Net'in servislerini al(IServicesCollection) ve onları build et.
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
