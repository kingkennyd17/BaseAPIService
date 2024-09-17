using Fintrak.Shared.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Fintrak.Shared.Common.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Fintrak.Shared.Common.Base
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataRepositoryFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public T GetRepository<T>() where T : class
        {
            var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
