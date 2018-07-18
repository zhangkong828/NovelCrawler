using NovelCrawler.Common;
using NovelCrawler.Models;
using NovelCrawler.Processer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelCrawler.Processer
{
    public class Spider
    {
        private ProcessEngineOptions _option;
        private RuleModel _rule;

        public Spider(ProcessEngineOptions option, RuleModel rule)
        {
            _option = option;
            _rule = rule;

        }

        public void TestRule()
        {
            try
            {
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("获取更新列表");
                var novelKeys = GetUpdateList();
                foreach (var item in novelKeys)
                {
                    Logger.ColorConsole(item);
                }
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("随机获取一本小说");
                var novelKey = novelKeys[UtilityHelper.Random(0, novelKeys.Count)];
                var info = GetNovelInfo(novelKey);
                Logger.ColorConsole(string.Format("Name:{0}", info.Name));
                Logger.ColorConsole(string.Format("ImageUrl:{0}", info.ImageUrl));
                Logger.ColorConsole(string.Format("Classify:{0}", info.Classify));
                Logger.ColorConsole(string.Format("Author:{0}", info.Author));
                Logger.ColorConsole(string.Format("State:{0}", info.State));
                Logger.ColorConsole(string.Format("Des:{0}", info.Des));
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("获取章节列表");


            }
            catch (Exception ex)
            {
                Logger.ColorConsole(ex.Message, ConsoleColor.Red);
            }
        }

        public void Run()
        {
            try
            {
                //获取更新列表
                GetUpdateList();
                //并行抓取
                //Parallel.ForEach(_novelKeys, (key, state) =>
                //{
                //    //检测库里是否存在
                //    if (false)
                //    {
                //        //更新
                //        //检测最新章节

                //    }
                //    else
                //    {
                //        //新增

                //    }
                //});

            }
            catch (Exception ex)
            {
                Logger.ColorConsole(ex.Message, ConsoleColor.Red);
            }
        }

        private string RegexMatch(PatternItem rule, string html)
        {
            var result = string.Empty;
            try
            {
                result = Regex.Match(html, rule.Pattern).Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(rule.Filter) && rule.Filter.Contains("&&"))
                {

                }
            }
            catch { }
            return result;
        }


        private List<string> GetUpdateList()
        {
            var result = new List<string>();
            //最新列表地址
            var updateListHtml = HtmlHelper.Get(_rule.NovelUpdateListUrl);
            if (string.IsNullOrWhiteSpace(updateListHtml))
                throw new Exception("最新列表地址无法访问");
            //匹配正则
            var mc = Regex.Matches(updateListHtml, _rule.NovelUpdateList.Pattern);
            if (mc.Count <= 0)
            {
                throw new Exception("更新列表规则匹配失败");
            }
            foreach (Match item in mc)
            {
                var novelKey = item.Groups[1].Value;
                result.Add(novelKey);
            }
            if (result.Count <= 0)
            {
                throw new Exception("获取更新列表失败");
            }
            return result;
        }

        private NovelInfo GetNovelInfo(string novelKey)
        {
            var novelUrl = _rule.NovelUrl.Replace("{NovelKey}", novelKey);
            Logger.ColorConsole(novelUrl);
            var novelInfoHtml = HtmlHelper.Get(novelUrl);
            if (string.IsNullOrWhiteSpace(novelInfoHtml))
                throw new Exception("小说详情页无法访问");
            //匹配正则
            if (Regex.IsMatch(novelInfoHtml, _rule.NovelErr.Pattern))
            {
                throw new Exception("匹配到小说页面错误标识，失败");
            }
            var info = new NovelInfo();
            info.Name = RegexMatch(_rule.NovelName, novelInfoHtml);
            info.ImageUrl = RegexMatch(_rule.NovelImage, novelInfoHtml);
            info.Classify = RegexMatch(_rule.NovelClassify, novelInfoHtml);
            info.Author = RegexMatch(_rule.NovelAuthor, novelInfoHtml);
            info.State = RegexMatch(_rule.NovelState, novelInfoHtml);
            info.Des = RegexMatch(_rule.NovelDes, novelInfoHtml);
            //章节目录
            //var chapterIndex = "";
            //if (!string.IsNullOrWhiteSpace(_rule.ChapterIndex))
            //{
            //    if (Regex.IsMatch(novelInfoHtml, _rule.ChapterIndex))
            //    {
            //        //chapterIndex = Regex.Match(novelInfoHtml, _rule)
            //    }
            //}
            return info;
        }

        private void GetNovelChapterList()
        {

        }

    }
}
