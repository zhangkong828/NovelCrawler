using MongoDB.Driver;
using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NovelCrawler.Repository.Repository
{
    public class NovelInfoRepository : RepositoryBase<NovelInfo>, INovelInfoRepository
    {
        public NovelInfoRepository() : base("novel", "infos")
        {

        }


        public bool Exists(Expression<Func<NovelInfo, bool>> expression, out NovelInfo model)
        {
            var filter = Builders<NovelInfo>.Filter.Where(expression);
            model = ICollection.Find(filter).FirstOrDefault();
            return model != null;
        }



    }
}
