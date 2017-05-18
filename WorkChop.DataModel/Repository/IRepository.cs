using System;
using System.Linq;
using System.Linq.Expressions;

namespace WorkChop.DataModel.Repository
{
    /// <summary>
    /// Generic repository pattern
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///  Generic Get method to get record on the basis of id  
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        TEntity Get(int id);

        /// <summary>
        ///  Generic Get method to get record on the basis of object id  
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        TEntity Get(Guid id);

        /// <summary>
        ///  Get all records   
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Generic add method to insert enities to collection  
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Generic update method to delete record on the basis of id 
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        void Update(Expression<Func<TEntity, int>> queryExpression, int id, TEntity entity);
      
        /// <summary>
        /// Generic delete method to delete record on the basis of id  
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="id"></param>
        void Delete(Expression<Func<TEntity, int>> queryExpression, int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TEntity GetByQuery(Expression<Func<TEntity, string>> queryExpression, string name);

       IQueryable<TEntity> GetDbSet(Expression<Func<TEntity, bool>> queryExpression);
    }
}
