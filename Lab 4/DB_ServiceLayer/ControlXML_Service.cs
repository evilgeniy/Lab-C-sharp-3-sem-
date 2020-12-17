
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_ServiceLayer
{
    public class ControlXML_Service<TEntity>
    {
        public string _Path { get; set; }

        public IEnumerable<TEntity> Entities { get; set; }
    }
}