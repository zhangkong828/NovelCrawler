using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.Common
{
    public class UrlHelper
    {
        public static string Combine(params string[] parts)
        {
            string url = string.Empty;
            if (parts != null && parts.Any())
            {
                char[] trims = new char[] { '\\', '/' };
                url = (parts[0] ?? string.Empty).TrimEnd(trims);

                for (int i = 1; i < parts.Length; i++)
                {
                    url = string.Format("{0}/{1}", url.TrimEnd(trims), (parts[i] ?? string.Empty).TrimStart(trims));
                }
            }
            return url;
        }
    }
}
