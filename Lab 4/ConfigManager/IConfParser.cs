using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config_Manager
{

    public interface IConfParser<out T>
    {
        T Parse();
    }
}