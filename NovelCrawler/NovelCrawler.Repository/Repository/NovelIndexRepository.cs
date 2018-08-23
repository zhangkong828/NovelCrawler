using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Repository.Repository
{
    public class NovelIndexRepository : RepositoryBase<NovelIndex>, INovelIndexRepository
    {
        public NovelIndexRepository() : base("novel", "indexes")
        {

        }
    }
}
