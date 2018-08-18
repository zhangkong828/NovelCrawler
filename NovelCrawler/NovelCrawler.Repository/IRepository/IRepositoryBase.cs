using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.Repository.IRepository
{
    /// <summary>
    /// 仓储基类接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryBase<T> where T : class
    {
        void Insert(T entity);

        void InsertMany(IEnumerable<T> entitys);

        bool Delete(Expression<Func<T, bool>> expression);

        T FindOrDefault(Expression<Func<T, bool>> expression);

        List<T> Find(Expression<Func<T, bool>> expression, int sortCode, Expression<Func<T, object>> sortExpression, int pageIndex, int pageSize);

        List<T> Find(Expression<Func<T, bool>> expression);

        bool Update(Expression<Func<T, bool>> expression, T data);

        bool Exists(Expression<Func<T, bool>> expression);

        long Count(Expression<Func<T, bool>> expression);
    }
}

