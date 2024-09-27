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
    [Export(typeof(ISolutionRunDateRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SolutionRunDateRepository : DataRepositoryBase<SolutionRunDate>, ISolutionRunDateRepository
    {
        private readonly string _connectionString;
        public SolutionRunDateRepository(CoreDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<SolutionRunDate>> GetRunDate()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["FintrakDBConnection"].ConnectionString;
            var connectionString = _connectionString;

            var solutionrundates = new List<SolutionRunDate>();
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("spp_getarchiverundate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;


                await con.OpenAsync();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var solutionrundate = new SolutionRunDate();

                    if (reader["AchRunDate"] != DBNull.Value)
                        solutionrundate.RunDate = DateTime.Parse(reader["AchRunDate"].ToString());

                    solutionrundates.Add(solutionrundate);
                }

                await con.CloseAsync();
            }

            return solutionrundates;
        }
    }
}
