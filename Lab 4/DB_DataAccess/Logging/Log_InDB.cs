
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_DataAccess.Logging
{
    public class Log_InDB  ILogger
    {
        public void ErrorToDB(string error_message)
        {
            using (var connection = new SqlConnection(Data.ConnectionString.ConnectionStringToErrorLog))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var command = new SqlCommand(AddError, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Transaction = transaction;

                try
                {
                    command.Parameters.Add(new SqlParameter(@Error, error_message));
                    command.Parameters.Add(new SqlParameter(@Time, DateTime.Now.ToString(s)));
                    var reader = command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            throw new NotImplementedException();
        }
    }
}