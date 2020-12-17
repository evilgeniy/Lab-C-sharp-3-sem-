using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DB_DataAccess.Data
{
    class ConnectionString
    {

        
        public const string ConnectionStringToAdventureWorks2019 = @"Data Source=NASTIONA;Initial Catalog=AdventureWork2019;Integrated Security=True";

        public const string ConnectionStringToErrorLog = @"Data Source=NASTIONA;Initial Catalog=ErrorLog;Integrated Security=True";
    }
}
