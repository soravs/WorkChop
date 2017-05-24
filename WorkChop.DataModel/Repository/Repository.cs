using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WorkChop.DataModel.Repository
{
    /// <summary>
    /// Repository Class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Variables

        private MongoDatabase _context;
        private string _tableName;
        private MongoCollection<TEntity> _collection;
        
        #endregion
        /// <summary>
        /// Constructor for Repository
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tblName"></param>
        public Repository(MongoDatabase context, string tblName)
        {
            _context = context;
            _tableName = tblName;
            _collection = _context.GetCollection<TEntity>(tblName);
        }

        /// <summary>
        ///  Generic Get method to get record on the basis of id  
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TEntity Get(int id)
        {
            return _collection.FindOneById(id);
        }
     
        /// <summary>
        ///  Generic Get method to get record on the basis of object Id  
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TEntity Get(Guid id)
        {
            return _collection.FindOneById(id);
        }

        ///<summary>  
        /// Get all records   
        ///</summary>  
        ///<returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            MongoCursor<TEntity> cursor = _collection.FindAll();
            return cursor.AsQueryable<TEntity>();
        }
      
        /// <summary>
        /// Generic add method to insert enities to collection  
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            _collection.Insert(entity);
        }

        /// <summary>
        /// Generic update method to delete record on the basis of id 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Expression<Func<TEntity, int>> queryExpression, int id, TEntity entity)
        {
            var query = Query<TEntity>.EQ(queryExpression, id);
            _collection.Update(query, Update<TEntity>.Replace(entity));
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual TEntity Update(TEntity entity)
        {
            this._collection.Save<TEntity>(entity);

            return entity;
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                this._collection.Save<TEntity>(entity);
            }
        }

        /// <summary>
        /// Generic delete method to delete record on the basis of id  
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="id"></param>
        public void Delete(Expression<Func<TEntity, int>> queryExpression, int id)
        {
            var query = Query<TEntity>.EQ(queryExpression, id);
            _collection.Remove(query);
        }
        
        /// <summary>
        /// Generic get method for specific record on basis of query
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <param name="name"></param>
        public TEntity GetByQuery(Expression<Func<TEntity,string>> queryExpression,string name)
        {
            var query = Query<TEntity>.EQ(queryExpression, name);
            return _collection.FindOne(query);
        }

        public IQueryable<TEntity> GetDbSet(Expression<Func<TEntity, bool>> queryExpression)
        {
            var query = Query<TEntity>.Where(queryExpression);
            MongoCursor<TEntity> cursor = _collection.Find(query);
            return cursor.AsQueryable<TEntity>();
        }
    }
}
