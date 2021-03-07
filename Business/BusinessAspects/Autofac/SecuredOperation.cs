using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;  //request yaparken her bir kişi için bir httpcontext oluşur. 

        public SecuredOperation(string roles) //rolleri almak için kullanılır
        {
            _roles = roles.Split(','); //metni belirttiğimiz karaktere göre bölüm array'e atar
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); //bizim

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles(); //bu kullanıcıların rollerini gez claimlerinin içerisinde ilgili rol varsa claim et çalıştır.
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied); // yetkisi yoksa bu mesajı ver
        }
    }
}
