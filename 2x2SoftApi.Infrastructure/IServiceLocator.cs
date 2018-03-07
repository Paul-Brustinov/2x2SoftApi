using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Infrastructure
{
    public interface IServiceLocator
    {
        T GetService<T>() where T : class;
        void RegisterService<T>(T service) where T : class;
    }
}
