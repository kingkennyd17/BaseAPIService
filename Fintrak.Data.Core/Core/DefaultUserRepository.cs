using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IDefaultUserRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DefaultUserRepository : DataRepositoryBase<DefaultUser>, IDefaultUserRepository
    {
        public DefaultUserRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
