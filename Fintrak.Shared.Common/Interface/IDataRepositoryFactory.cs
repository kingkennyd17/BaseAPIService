using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.Interface
{
    public interface IDataRepositoryFactory
    {
        T GetRepository<T>() where T : class;
    }

}
