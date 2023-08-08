using Focus.IService.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service.Services
{
    public class DynamicProxyService : IDynamicProxyService
    {
        public void SayHello()
        {
            Console.WriteLine("Hello World！");
        }
    }
    public class DynamicProxyService2 : IDynamicProxyService2
    {
        public void SayHello(string message)
        {
            Console.WriteLine(message);
        }
    }
}
