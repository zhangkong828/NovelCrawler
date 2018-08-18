using MongoDB.Driver;
using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NovelCrawler.Repository.Repository
{
    public class NovelInfoRepository : RepositoryBase<NovelInfo>, INovelInfoRepository
    {
        public NovelInfoRepository() : base("novel", "infos")
        {

        }

        

      

    }
}
