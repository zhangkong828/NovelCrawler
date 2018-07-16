using NovelCrawler.Common;
using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelCrawler.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>("testRule.xml", Encoding.UTF8);
            TestLog("开始测试");
            Run(rule);
            TestLog("测试结束");

            Logger.Info(sb.ToString());

            Console.WriteLine("over");

            Console.ReadKey();
        }
        static void TestLog(string msg)
        {
            Console.WriteLine(msg);

        }

        static void Run(RuleModel rule)
        {
            //获取列表
            var novelList = new List<string>();
            TestLog("---------------------------------------");
            TestLog("获取列表");
            var updateListHtml = HtmlHelper.Get(rule.NovelUpdateListUrl);
            if (string.IsNullOrEmpty(updateListHtml))
            {
                TestLog("更新列表无法访问");
                return;
            }
            //匹配
            var mc = Regex.Matches(updateListHtml, rule.NovelUpdateList.Pattern);
            if (mc.Count <= 0)
            {
                TestLog("更新列表规则失败");
                return;
            }
            foreach (Match item in mc)
            {
                var url = UtilityHelper.Combine(rule.SiteUrl, item.Groups[1].Value);
                TestLog(url);
                novelList.Add(url);
            }
            if (novelList.Count <= 0)
            {
                TestLog("获取更新列表失败");
                return;
            }

            //随机获取小说
            rule.NovelUrl.Pattern = novelList[UtilityHelper.Random(0, novelList.Count)];
            TestLog($"随机获取小说：{rule.NovelUrl.Pattern}");

            //获取小说详情页
            TestLog("---------------------------------------");
            TestLog("获取小说详情页");
            var novelInfoHtml = HtmlHelper.Get(rule.NovelUrl.Pattern);
            if (string.IsNullOrEmpty(novelInfoHtml))
            {
                TestLog("小说详情页无法访问");
                return;
            }
            //匹配
            if (Regex.IsMatch(novelInfoHtml, rule.NovelErr.Pattern))
            {
                TestLog(rule.NovelErr.Pattern);
                return;
            }
            TestLog($"Name:{RegexMatch(rule.NovelName, novelInfoHtml)}");
            TestLog($"Image:{RegexMatch(rule.NovelImage, novelInfoHtml)}");
            TestLog($"Classify:{ RegexMatch(rule.NovelClassify, novelInfoHtml)}");
            TestLog($"Author:{RegexMatch(rule.NovelAuthor, novelInfoHtml)}");
            TestLog($"State:{ RegexMatch(rule.NovelState, novelInfoHtml)}");
            TestLog($"Des:{ RegexMatch(rule.NovelDes, novelInfoHtml)}");

            //章节列表
            rule.ChapterList.Pattern = rule.NovelUrl.Pattern;

            //获取小说章节页
            TestLog("---------------------------------------");
            TestLog("获取小说章节页");
            var chapterList = new List<KeyValuePair<string, string>>();
            var chapterListHtml = HtmlHelper.Get(rule.ChapterList.Pattern);
            if (string.IsNullOrEmpty(chapterListHtml))
            {
                TestLog("小说章节页无法访问");
                return;
            }
            var chapterNameMc = Regex.Matches(chapterListHtml, rule.ChapterName.Pattern);
            var chapterUrlMc = Regex.Matches(chapterListHtml, rule.ChapterUrl.Pattern);
            if (chapterNameMc.Count <= 0 || chapterUrlMc.Count <= 0 || chapterNameMc.Count != chapterUrlMc.Count)
            {
                TestLog("获取小说章节页失败");
                return;
            }
            for (int i = 0; i < chapterNameMc.Count; i++)
            {
                var name = chapterNameMc[i].Groups[1].Value.Trim();
                var url = chapterUrlMc[i].Groups[1].Value.Trim();
                TestLog($"{name} — {url}");
                chapterList.Add(new KeyValuePair<string, string>(name, url));
            }

            //随机获取章节
            var randomChapter = chapterList[UtilityHelper.Random(0, chapterList.Count)];
            TestLog("---------------------------------------");
            TestLog("随机获取章节：");
            TestLog(randomChapter.Key);
            TestLog(randomChapter.Value);
            var chapterUrl = UtilityHelper.Combine(rule.SiteUrl, randomChapter.Value);
            var chapterHtml = HtmlHelper.Get(chapterUrl);
            if (Regex.IsMatch(chapterHtml, rule.ContentErr.Pattern))
            {
                TestLog(rule.ContentErr.Pattern);
                return;
            }
            TestLog(RegexMatch(rule.ContentText, chapterHtml));
        }


        static string RegexMatch(PatternItem rule, string html)
        {
            var result = string.Empty;
            try
            {
                result = Regex.Match(html, rule.Pattern).Groups[1].Value;
            }
            catch { }
            return result;
        }

        static StringBuilder sb = new StringBuilder();
        
    }
}
