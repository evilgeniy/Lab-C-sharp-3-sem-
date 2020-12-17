
using DB_Models;
using DB_Models.SearchModels;
using System.Collections.Generic;

namespace DB_DataAccess.Repository
{
    public interface IRepository<TEntity> where TEntity: class
    {
        IEnumerable<TEntity> GetALLInf(SearchCriteria searchCriteria);
        TEntity GetEntity(SearchCriteria searchCriteria);


    }
}