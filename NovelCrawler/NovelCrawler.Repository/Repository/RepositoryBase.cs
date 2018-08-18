using MongoDB.Driver;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NovelCrawler.Repository.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected IMongoCollection<T> ICollection;

        public RepositoryBase(string dbName, string collectionName)
        {
            ICollection = MongoHelper.Instance.GetCollection<T>(dbName, collectionName);
        }

        public void Insert(T entity)
        {
            ICollection.InsertOne(entity);
        }

        public void InsertMany(IEnumerable<T> entitys)
        {
            ICollection.InsertMany(entitys);
        }

        public bool Delete(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.DeleteOne(filter).IsAcknowledged;
        }

        public T FindOrDefault(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.Find(filter).FirstOrDefault();
        }

        public List<T> Find(Expression<Func<T, bool>> expression, int sortCode, Expression<Func<T, object>> sortExpression, int pageIndex, int pageSize)
        {
            var filter = Builders<T>.Filter.Where(expression);
            SortDefinition<T> sort;
            switch (sortCode)
            {
                case 0:
                    sort = Builders<T>.Sort.Ascending(sortExpression);
                    break;
                case 1:
                    sort = Builders<T>.Sort.Descending(sortExpression);
                    break;
                default:
                    sort = Builders<T>.Sort.Ascending(sortExpression);
                    break;
            }
            return ICollection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }

        public List<T> Find(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.Find(filter).ToList();
        }


        public bool Update(Expression<Func<T, bool>> expression, T data)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.FindOneAndReplace(filter, data) != null;
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.Find(filter).ToList().Any();
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return ICollection.CountDocuments(filter);
        }
    }
}
