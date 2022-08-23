using Focus.IService;
using Infrastructur.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service
{
    public class Test : IDependency, ITest
    {
        public void Get()
        {
            Console.WriteLine("Test:Get");
            //throw new NotImplementedException();
        }
    }
}
