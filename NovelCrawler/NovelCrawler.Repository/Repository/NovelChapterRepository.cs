using MongoDB.Driver;
using NovelCrawler.Infrastructure.Router;
using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NovelCrawler.Repository.Repository
{
    public class NovelChapterRepository : INovelChapterRepository
    {
        private IMongoCollection<NovelChapter> GetCollection(string id)
        {
            var shardingId = Route.GetShardingId(id);
            var dbName = "novel";
            var collectionName = "chapter" + shardingId;
            return MongoHelper.Instance.GetCollection<NovelChapter>(dbName, collectionName);
        }

        public bool Delete(string id, Expression<Func<NovelChapter, bool>> expression)
        {
            var collection = GetCollection(id);
            var filter = Builders<NovelChapter>.Filter.Where(expression);
            return collection.DeleteOne(filter).IsAcknowledged;
        }

        public NovelChapter Find(string id, Expression<Func<NovelChapter, bool>> expression)
        {
            var collection = GetCollection(id);
            var filter = Builders<NovelChapter>.Filter.Where(expression);
            return collection.Find(filter).FirstOrDefault();
        }

        public void Insert(string id, NovelChapter entity)
        {
            var collection = GetCollection(id);
            collection.InsertOne(entity);
        }

        public bool Update(string id, Expression<Func<NovelChapter, bool>> expression, NovelChapter data)
        {
            var collection = GetCollection(id);
            var filter = Builders<NovelChapter>.Filter.Where(expression);
            return collection.FindOneAndReplace(filter, data) != null;
        }
    }
}
