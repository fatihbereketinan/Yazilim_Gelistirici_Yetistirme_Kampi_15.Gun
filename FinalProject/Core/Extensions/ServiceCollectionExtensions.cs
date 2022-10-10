using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //Apimizin servis bağımlılıklarını ekledğimiz koleksiyondur.
        //ICoreModule[] ile istediğim kadar bağımlılık ve modül ekleyebilirim. Bir arada toplayabilirim.
        //this paremetre değil genişletmek istediğimiz belirtiyor.
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection);
            }
            return ServiceTool.Create(serviceCollection); //o servisleri create et.
        }
    }
}
