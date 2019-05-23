using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IRepository
    {
        IEntity GetById(Object id);
        ISubCollection<IEntity> GetByIds(Object[] ids);
        ISubCollection<IEntity> List();
        IInsertOperationResult Insert(IEntity entity);
        IUpdateOperationResult Update(IEntity entity);
        IDeleteOperationResult DeleteById(Object id);

        IEntity CreateEntityInstance();
    }

    public interface IRepository<T>
        where T : class, IEntity
    {
        T GetById(Object id);
        ISubCollection<T> GetByIds(Object[] ids);
        ISubCollection<T> List();
        IInsertOperationResult Insert(T entity);
        IUpdateOperationResult Update(T entity);
        IDeleteOperationResult DeleteById(Object id);

        T CreateEntityInstance();
    }
}
