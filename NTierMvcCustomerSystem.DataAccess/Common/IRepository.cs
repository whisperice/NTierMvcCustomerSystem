using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.DataAccess.Common
{
    // This interface contains common methods implemented by all repository classes.
    public interface IRepository<TEntity> where TEntity:class
    {
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
        bool DeleteById(int id);
        TEntity SelectById(int id);
        List<TEntity> SelectAll();
    }
}
