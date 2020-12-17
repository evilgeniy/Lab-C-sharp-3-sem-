using System.Collections.Generic;

namespace DB_ServiceLayer.Generating
{
    public interface IGenerator<in TEntity>
    {
        void XML_Generating();

        void XSD_Generating();
    }
}