using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IExtractionJobRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExtractionJobRepository : DataRepositoryBase<ExtractionJob>, IExtractionJobRepository
    {
        private readonly string _connectionString;
        public ExtractionJobRepository(CoreDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ExtractionJob>> GetExtractionJobByRunDate(DateTime startDate, DateTime endDate)
        {
            var query = await (from a in _context.ExtractionJobSet
                        where a.StartDate >= startDate.Date && a.EndDate <= endDate.Date
                        select a).ToListAsync();

            return query;
        }

        public async Task ClearExtractionHistory(int solutionId)
        {

            var connectionString = _connectionString;

            int status = 0;

            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("spp_clear_extraction_history", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "SolutionId",
                    Value = solutionId,
                });

                await con.OpenAsync();

                status = cmd.ExecuteNonQuery();

                await con.CloseAsync();
            }

        }
    }
}
