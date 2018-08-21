using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Repository.Repository
{
    class NovelIndexRepository : RepositoryBase<NovelIndex>, INovelIndexRepository
    {
        public NovelIndexRepository() : base("novel", "indexes")
        {

        }
    }
}
