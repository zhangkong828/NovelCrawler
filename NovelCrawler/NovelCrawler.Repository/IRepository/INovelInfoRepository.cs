using NovelCrawler.Models;
using System;
using System.Linq.Expressions;

namespace NovelCrawler.Repository.IRepository
{
    public interface INovelInfoRepository : IRepositoryBase<NovelInfo>
    {
        bool Exists(Expression<Func<NovelInfo, bool>> expression, out NovelInfo model);
    }
}
