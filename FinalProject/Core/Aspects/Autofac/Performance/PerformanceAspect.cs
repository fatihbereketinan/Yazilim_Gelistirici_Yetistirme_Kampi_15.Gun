using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        //Eğer MethodInterception'a attribute olarak eklenirse sistemdeki her şeyi kontrol eder

        private int _interval;
        private Stopwatch _stopwatch; //timer, kronometre 

        public PerformanceAspect(int interval) //Performans değerlendirmesinde kriter olarak verilecek zaman
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
            //Stopwatch instance'ı için CoreModule'a eklenir.
            //GetService bu instance'ı kontrol edip _stopwatch'a aktarır
        }


        protected override void OnBefore(IInvocation invocation) //metodun önünde kronometre başlıyor.
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)//metot bitince;
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval) //o ana kadar geçen süreyi hesaplıyor. ve belirtilen süreyi geçerse uyarı veriyor.
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset(); //
        }
    }
}
