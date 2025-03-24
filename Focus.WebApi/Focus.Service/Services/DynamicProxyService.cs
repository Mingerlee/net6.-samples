using Focus.IService.IServices;
using Infrastructure.Models;
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
        public UserToken SayHello(string message)
        {
            Console.WriteLine(message);
            return new UserToken
            {
                Account = 3,
                Channel = "dsa",
                Email = "23456@qq.com",
                IP = "127.0.0.1",
                Mobile = "1234564",
                MobileArea = "0731",
                Name = "sadf",
                UserCode = "U0005"
            };
        }

        public void SayHello2(string message)
        {
            Console.WriteLine(message);
        }
    }
}
