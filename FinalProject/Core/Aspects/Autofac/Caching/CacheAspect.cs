using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception //Attribute
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60) //default değer var biz süre vermezsek 60 dk cachede duracak.
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            //redis kullanılsa bile GetService'e dokunulmaz Sadece CoreModule değiştirilir
        }

        public override void Intercept(IInvocation invocation) //MethodInterceptiondan geliyor
        {
            //key oluşturma
            //ReflectedType:namespace+classname
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();//methodun paremetreleri varsa listeye çevir.
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            //Yukarıdaki iki değer bilerştirilerek key oluşturulur.
            //virgül ile ayrı parametreler tanımlanır eğer parametre yoksa null değeri verilir
            if (_cacheManager.IsAdd(key))//bellekte böyle bir anahtar var mı? 
            {
                invocation.ReturnValue = _cacheManager.Get(key);//sen metodu hiç çalıştırmadan geri dön _cacheManager den set et.
                                                                //metodun return değeri cachedeki değer olsun demek.
                return;
            }
            invocation.Proceed();//metodu çalıştır. veritabanına gitti datayı getirdi.
            _cacheManager.Add(key, invocation.ReturnValue, _duration); //daha önce hic cache ye eklenmemiş ama eklenmesi gerekiyor.
        }
    }
}
