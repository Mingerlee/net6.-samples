using Autofac.Extras.DynamicProxy;
using Focus.DynamicProxys.Interceptors;
using Infrastructur.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.IService.IServices
{
    [Intercept(typeof(TestDynamicProxy))]
    public interface IDynamicProxyService: IAutofacDynamicProxy
    {
        void SayHello();
    }
}
