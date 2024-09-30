using Fintrak.Shared.Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.Data
{
    public static class SqlDataManager
    {
        public static async Task RunProcedure(string connectionString, string actionText, SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(actionText, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public static async Task RunProcedure(string connectionString, string actionText)
        {
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(actionText, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public static async Task<string> RunProcedureWithMessage(string actionText, SqlParameter[] parameters, string connectionString)
        {
            var result = string.Empty;

            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(actionText, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddRange(parameters);

                SqlDataReader reader = null;

                await con.OpenAsync();

                reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value)
                            result = reader[0].ToString();
                    }
                }

                await con.CloseAsync();
            }

            return result;
        }

        public static async Task<string> RunProcedureWithMessage(string connectionString, string actionText, SqlParameter[] parameters)
        {
            var result = string.Empty;

            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(actionText, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddRange(parameters);

                SqlDataReader reader = null;

                await con.OpenAsync();

                reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value)
                            result = reader[0].ToString();
                    }
                }

                await con.CloseAsync();
            }

            return result;
        }
    }
}
