using Castle.DynamicProxy;
using Infrastructur.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.DynamicProxys.Interceptors
{
    public class TestDynamicProxy2 : IInterceptor, IDependency
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("代理2：处理程序之前");
            invocation.Proceed();
            Console.WriteLine("代理2：处理程序之后");
            //throw new NotImplementedException();
        }
    }
}
