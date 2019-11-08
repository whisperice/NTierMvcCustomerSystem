using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.BusinessLogic.Interface
{
    /// <summary>
    /// This interface contains common methods should be implemented by a model service class.
    /// </summary>
    /// <typeparam name="TModel">The class of the model that are handled</typeparam>
    public interface IModelService<TModel> where TModel : class
    {
        bool Insert(TModel entity);
        bool Update(TModel entity);
        bool DeleteById(int id);
        TModel SelectById(int id);
        IList<TModel> SelectAll();
    }
}
