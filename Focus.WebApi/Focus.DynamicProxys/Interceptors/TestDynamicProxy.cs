using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Infrastructur.AutofacManager;

namespace Focus.DynamicProxys.Interceptors
{
    public class TestDynamicProxy : IInterceptor, IDependency
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("代理：处理程序之前");
            invocation.Proceed();
            Console.WriteLine("代理：处理程序之后");
            //throw new NotImplementedException();
        }
    }
}
