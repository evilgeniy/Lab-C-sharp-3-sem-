using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_DataAccess
{
    public interface ILogger
    {
        void ErrorToDB(string error_message);
    }
}