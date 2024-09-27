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
    public class SolutionRepository : DataRepositoryBase<Solution>, ISolutionRepository
    {
        public SolutionRepository(SystemCoreDbContext context) : base(context)
        {

        }
    }
}
