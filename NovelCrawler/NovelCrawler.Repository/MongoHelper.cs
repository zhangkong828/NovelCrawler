using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Repository
{
    public class MongoHelper
    {
        private static readonly object _obj = new object();

        private static MongoHelper _instance;
        public static MongoHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_obj)
                    {
                        if (_instance == null)
                            _instance = new MongoHelper();
                    }
                }
                return _instance;
            }

        }

        private readonly string _constr;
        private readonly string _dbName;
        private readonly string _collectionName;
        private readonly MongoClient _client;

        private MongoHelper()
        {
            _client = new MongoClient(_constr);
        }

        public IMongoDatabase GetDb()
        {
            return GetDb(_dbName);
        }

        public IMongoDatabase GetDb(string dbName)
        {
            return _client.GetDatabase(dbName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return GetCollection<T>(_collectionName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetCollection<T>(_dbName, _collectionName);
        }

        public IMongoCollection<T> GetCollection<T>(string dbName, string collectionName)
        {
            return GetDb(dbName).GetCollection<T>(collectionName);
        }


    }
}
