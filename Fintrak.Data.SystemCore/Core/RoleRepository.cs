using Fintrak.Data.Interface;
using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Shared.Common.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore
{
    public class RoleRepository : DataRepositoryBase<Roles>, IRoleRepository
    {
        //private readonly SystemCoreDbContext _context;
        public RoleRepository(SystemCoreDbContext context) : base(context)
        {
            //_context = context;
        }
    }
}
