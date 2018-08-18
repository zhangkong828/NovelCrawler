using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NovelCrawler.Infrastructure
{
    public class Utils
    {
        /// <summary>
        /// 对象的属性是否都不为null或空,仅限string类型，可设置忽略项
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignores">忽略的属性名</param>
        /// <returns></returns>
        public static bool ObjectIsNotNull<T>(T o, params string[] ignores) where T : class
        {
            var t = typeof(T);
            foreach (var pi in t.GetProperties())
            {
                if (pi.PropertyType.Name == "String")
                {
                    if (ignores.Contains(pi.Name))
                        continue;
                    var v = pi.GetValue(o);
                    if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                        return false;
                }
            }
            return true;
        }
    }
}
