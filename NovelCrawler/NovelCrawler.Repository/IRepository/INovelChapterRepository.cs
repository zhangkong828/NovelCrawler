using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NovelCrawler.Repository.IRepository
{
    public interface INovelChapterRepository
    {
        void Insert(string id, NovelChapter entity);

        bool Delete(string id, Expression<Func<NovelChapter, bool>> expression);

        NovelChapter Find(string id, Expression<Func<NovelChapter, bool>> expression);

        bool Update(string id, Expression<Func<NovelChapter, bool>> expression, NovelChapter data);

    }
}
