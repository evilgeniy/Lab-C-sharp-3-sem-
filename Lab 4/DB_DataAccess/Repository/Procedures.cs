using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_DataAccess.Repository
{
    public class Procedures
    {
        public const string GetPersonByID = "[dbo].[Person_GetData]";
        public const string GetAllPersons = "[dbo].[GetAllPersons]";
    }
}