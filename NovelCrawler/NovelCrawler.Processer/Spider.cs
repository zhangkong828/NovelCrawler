using NovelCrawler.Infrastructure;
using NovelCrawler.Infrastructure.Configuration;
using NovelCrawler.Models;
using NovelCrawler.Processer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace NovelCrawler.Processer
{
    public class Spider
    {
        private ProcessEngineOptions _option;
        private RuleModel _rule;
        private List<NovelSortSettings> _novelSorts;

        public Spider(ProcessEngineOptions option, RuleModel rule)
        {
            _rule = rule;

            _novelSorts = ConfigurationManager.GetSection<List<NovelSortSettings>>("SpiderSettings:NovelSort");
        }

        /// <summary>
        /// 测试规则
        /// </summary>
        public async Task TestRule()
        {
            try
            {
                Logger.ColorConsole("开始测试");
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("获取更新列表");
                var novelKeys = await GetUpdateList();
                foreach (var item in novelKeys)
                {
                    Logger.ColorConsole(item);
                }
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("随机获取小说");
                var novelKey = novelKeys[UtilityHelper.Random(0, novelKeys.Count)];
                var info = await GetNovelInfo(novelKey);
                Logger.ColorConsole(string.Format("Name:{0}", info.Name));
                Logger.ColorConsole(string.Format("ImageUrl:{0}", info.ImageUrl));
                Logger.ColorConsole(string.Format("Sort:{0}", info.Sort));
                Logger.ColorConsole(string.Format("Author:{0}", info.Author));
                Logger.ColorConsole(string.Format("State:{0}", info.State));
                Logger.ColorConsole(string.Format("Des:{0}", info.Des));
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("获取章节列表");
                var chapterIndex = info.ChapterIndex;
                var chapterList = await GetNovelChapterList(novelKey, chapterIndex);
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("随机获取章节");
                var randomChapter = chapterList[UtilityHelper.Random(0, chapterList.Count)];
                Logger.ColorConsole(randomChapter.Key);
                Logger.ColorConsole(randomChapter.Value);
                var chapterKey = randomChapter.Value;
                var content = await GetContent(novelKey, chapterIndex, chapterKey);
                Logger.ColorConsole(content);
                Logger.ColorConsole("---------------------------------------");
                Logger.ColorConsole("测试结束");
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

            }
            catch (Exception ex)
            {
                Logger.ColorConsole(ex.Message, ConsoleColor.Red);
            }
        }


        /// <summary>
        /// 获取小说更新列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetUpdateList()
        {
            var result = new List<string>();
            //最新列表地址
            var updateListHtml = await HtmlHelper.Get(_rule.NovelUpdateListUrl);
            if (string.IsNullOrWhiteSpace(updateListHtml))
                throw new SpiderException("最新列表地址无法访问");
            //匹配正则
            var mc = Regex.Matches(updateListHtml, _rule.NovelUpdateList.Pattern);
            if (mc.Count <= 0)
            {
                throw new SpiderException("更新列表规则匹配失败");
            }
            foreach (Match item in mc)
            {
                var novelKey = item.Groups[1].Value;
                result.Add(novelKey);
            }
            if (result.Count <= 0)
            {
                throw new SpiderException("获取更新列表失败");
            }
            return result;
        }

        /// <summary>
        /// 获取小说详情
        /// </summary>
        /// <param name="novelKey"></param>
        /// <returns></returns>
        public async Task<NovelDetails> GetNovelInfo(string novelKey)
        {
            //小说信息页url处理
            var novelUrl = _rule.NovelUrl.Replace("{NovelKey}", novelKey);
            if (!novelUrl.Contains(_rule.SiteUrl))
                novelUrl = UtilityHelper.Combine(_rule.SiteUrl, novelUrl);
            Logger.ColorConsole("抓取小说详情:" + novelUrl);
            var novelInfoHtml = await HtmlHelper.Get(novelUrl);
            if (string.IsNullOrWhiteSpace(novelInfoHtml))
                throw new SpiderException("小说详情页无法访问");
            //匹配正则
            if (Regex.IsMatch(novelInfoHtml, _rule.NovelErr.Pattern))
            {
                throw new SpiderException("匹配到小说页面错误标识，失败");
            }
            var info = new NovelDetails();
            info.Name = RegexMatch(_rule.NovelName, novelInfoHtml);
            info.ImageUrl = RegexMatch(_rule.NovelImage, novelInfoHtml);
            info.Sort = RegexMatch(_rule.NovelClassify, novelInfoHtml);
            info.Author = RegexMatch(_rule.NovelAuthor, novelInfoHtml);
            info.State = RegexIsMatch(_rule.NovelState, novelInfoHtml) ? 1 : 0;
            info.Des = RegexMatch(_rule.NovelDes, novelInfoHtml);
            info.ChapterIndex = RegexMatch(_rule.ChapterIndex, novelInfoHtml); //章节目录

            if (!Utils.ObjectIsNotNull<NovelDetails>(info, "Des", "ChapterIndex"))
            {
                throw new SpiderException("获取小说详情失败，有匹配不到的值");
            }
            return info;
        }

        /// <summary>
        /// 获取小说章节目录
        /// </summary>
        /// <param name="novelKey"></param>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetNovelChapterList(string novelKey, string chapterIndex)
        {
            var result = new List<KeyValuePair<string, string>>();
            //章节目录页url处理
            var chapterList = _rule.ChapterList.Replace("{NovelKey}", novelKey).Replace("{ChapterIndex}", chapterIndex);
            if (!chapterList.Contains(_rule.SiteUrl))
                chapterList = UtilityHelper.Combine(_rule.SiteUrl, chapterList);
            Logger.ColorConsole("抓取章节目录:" + chapterList);
            var chapterListHtml = await HtmlHelper.Get(chapterList);
            if (string.IsNullOrEmpty(chapterListHtml))
            {
                throw new SpiderException("小说章节目录无法访问");
            }
            var chapterNameMc = Regex.Matches(chapterListHtml, _rule.ChapterName.Pattern);
            var chapterUrlMc = Regex.Matches(chapterListHtml, _rule.ChapterUrl.Pattern);
            if (chapterNameMc.Count <= 0 || chapterUrlMc.Count <= 0 || chapterNameMc.Count != chapterUrlMc.Count)
            {
                throw new SpiderException("获取小说章节失败");
            }
            for (int i = 0; i < chapterNameMc.Count; i++)
            {
                var name = chapterNameMc[i].Groups[1].Value.Trim();
                name = ReplaceMatch(name, _rule.ChapterName.Filter.OuterXml);
                var url = chapterUrlMc[i].Groups[1].Value.Trim();
                Logger.ColorConsole(string.Format("{0}-{1}", name, url));
                result.Add(new KeyValuePair<string, string>(name, url));
            }
            return result;
        }

        /// <summary>
        /// 获取小说章节内容
        /// </summary>
        /// <param name="novelKey"></param>
        /// <param name="chapterIndex"></param>
        /// <param name="chapterKey"></param>
        /// <returns></returns>
        public async Task<string> GetContent(string novelKey, string chapterIndex, string chapterKey)
        {
            var content = string.Empty;
            //章节内容页url处理
            var contentUrl = _rule.ContentUrl.Replace("{NovelKey}", novelKey).Replace("{ChapterIndex}", chapterIndex).Replace("{ChapterKey}", chapterKey);
            if (!contentUrl.Contains(_rule.SiteUrl))
                contentUrl = UtilityHelper.Combine(_rule.SiteUrl, contentUrl);
            Logger.ColorConsole("抓取章节内容:" + contentUrl);
            var chapterHtml = await HtmlHelper.Get(contentUrl);
            if (Regex.IsMatch(chapterHtml, _rule.ContentErr.Pattern))
            {
                throw new SpiderException("匹配到章节错误标识，失败");
            }
            content = RegexMatch(_rule.ContentText, chapterHtml);
            return content;
        }

        private bool RegexIsMatch(PatternItem rule, string html)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(rule.Pattern) && Regex.IsMatch(html, rule.Pattern))
                {
                    result = Regex.IsMatch(html, rule.Pattern);
                }
            }
            catch { }
            return result;
        }

        private string RegexMatch(PatternItem rule, string html)
        {
            var result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(rule.Pattern) && Regex.IsMatch(html, rule.Pattern))
                {
                    //匹配
                    result = Regex.Match(html, rule.Pattern).Groups[1].Value.Trim();
                    //替换
                    result = ReplaceMatch(result, rule.Filter.OuterXml);
                }
            }
            catch { }
            return result;
        }

        private string ReplaceMatch(string str, string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter) && filter.Contains("&&"))
            {
                var lines = filter.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains("&&"))
                    {
                        var strs = line.Split(new string[] { "&&" }, StringSplitOptions.None);
                        var str1 = strs[0];
                        var str2 = strs[1];
                        if (!string.IsNullOrWhiteSpace(str1))
                        {
                            str.Replace(str1, str2);
                        }
                    }
                }
            }
            return str;
        }

        //入库才匹配分类
        public string MatchSort(string str)
        {
            var sort = _novelSorts.LastOrDefault()?.Name;

            if (string.IsNullOrWhiteSpace(str))
                return sort;

            foreach (var item in _novelSorts)
            {
                if (Regex.IsMatch(str.Trim(), item.Match))
                {
                    sort = item.Name;
                    break;
                }
            }

            return sort;
        }

        public byte[] DownLoadImage(string url)
        {
            return HtmlHelper.DownLoad(url);
        }

        public string DownLoadImageToBase64(string url)
        {
            var bytes = this.DownLoadImage(url);
            if (bytes != null)
                return Convert.ToBase64String(bytes);
            else
                return null;
        }
    }
}
