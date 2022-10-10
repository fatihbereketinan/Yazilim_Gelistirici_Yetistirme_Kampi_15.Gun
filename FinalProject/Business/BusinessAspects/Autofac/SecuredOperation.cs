using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.BusinessAspects.Autofac
{
    //SecuredOperation JWT için
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor; //Her istek için bir httpcontexti oluşturur.

        public SecuredOperation(string roles) //bana rolleri ver.
        {
            _roles = roles.Split(','); //senin belirttiğin metni ayırıp arraye atıyor.//[product.add,admin]
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation) //MethodInterceptiondan gelir. Önünde çalıştır.
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles(); //rollerini çöz
            foreach (var role in _roles) //rollerini gez
            {
                if (roleClaims.Contains(role)) //rolü varsa return et metodu çalıştırmay devam et
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied); //claimi yoksa yetkin yok hatası ver.
        }
    }
}
