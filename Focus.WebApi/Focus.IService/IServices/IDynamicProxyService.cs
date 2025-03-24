using Autofac.Extras.DynamicProxy;
using Focus.DynamicProxys.Interceptors;
using Infrastructur.AutofacManager;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.IService.IServices
{
    [Intercept(typeof(TestDynamicProxy))]
    [Intercept(typeof(TestDynamicProxy2))]
    public interface IDynamicProxyService: IAutofacDynamicProxy
    {
        void SayHello();

    }
    [Intercept(typeof(TestDynamicProxy2))]
    public interface IDynamicProxyService2 : IAutofacDynamicProxy
    {
        UserToken SayHello(string message);
        void SayHello2(string message);
    }
}
