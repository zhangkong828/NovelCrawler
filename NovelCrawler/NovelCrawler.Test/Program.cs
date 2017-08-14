using NovelCrawler.Common;
using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rule = new Rule()
            {
                SiteName = "name",
                SiteUrl = "www.xxx.com",
                NovelUpdateListUrl = "www.update.com",
                NovelUpdateList = new RuleItem() { Pattern = "aaaa" },
                NovelUrl = new RuleItem() { Pattern = "111" },
                NovelErr = new RuleItem() { Pattern = "aa222aa" },
                NovelName = new RuleItem() { Pattern = "333" },
                NovelImage = new RuleItem() { Pattern = "444" },
                NovelClassify = new RuleItem() { Pattern = "555" }
            };

            //XmlHelper.XmlSerializeToFile(rule, "rule.xml", Encoding.UTF8);

            var xml = XmlHelper.XmlDeserializeFromFile<Rule>("rule.xml", Encoding.UTF8);

            Console.WriteLine("ok");



            Console.ReadKey();
        }
    }
}
