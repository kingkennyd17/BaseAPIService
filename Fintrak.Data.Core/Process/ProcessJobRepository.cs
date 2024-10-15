using Fintrak.Model.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessJobRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessJobRepository : DataRepositoryBase<ProcessJob>, IProcessJobRepository
    {
        private readonly string _connectionString;
        public ProcessJobRepository(CoreDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ProcessJob>> GetProcessJobByRunDate(DateTime startDate, DateTime endDate)
        {
            var query = from a in _context.ProcessJobSet
                        where a.StartDate >= startDate.Date &&  a.EndDate <= endDate.Date
                            select a;

            return query.ToArray();
        }

        public async Task ClearProcessHistory(int solutionId)
        {
            int status = 0;

            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("spp_clear_process_history", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "SolutionId",
                    Value = solutionId,
                });

                await con.OpenAsync();

                status = await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }
    }
}
