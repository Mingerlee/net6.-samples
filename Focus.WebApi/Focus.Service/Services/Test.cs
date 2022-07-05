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
            //throw new NotImplementedException();
        }
    }
}
