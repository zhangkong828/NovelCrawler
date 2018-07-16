using NovelCrawler.Common;
using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelCrawler.Rule
{
    public partial class TestForm : Form
    {
        public TestForm(RuleModel rule)
        {
            InitializeComponent();

            Task.Run(() =>
            {
                ExcuteRecord("开始测试");
                RunTest(rule);
                ExcuteRecord("测试结束");
            });

        }

        private void ExcuteRecord(string msg)
        {
            rtb_record.Invoke(new Action(() =>
            {
                rtb_record.AppendText(msg + "\r\n");
                rtb_record.ScrollToCaret();
            }));

        }

        private void RunTest(RuleModel rule)
        {

            //获取列表
            var novelList = new List<string>();
            ExcuteRecord("---------------------------------------");
            ExcuteRecord("获取列表");
            var updateListHtml = HtmlHelper.Get(rule.NovelUpdateListUrl);
            if (string.IsNullOrEmpty(updateListHtml))
            {
                ExcuteRecord("更新列表无法访问");
                return;
            }
            //匹配
            var mc = Regex.Matches(updateListHtml, rule.NovelUpdateList.Pattern);
            if (mc.Count <= 0)
            {
                ExcuteRecord("更新列表规则失败");
                return;
            }
            foreach (Match item in mc)
            {
                var url = UtilityHelper.Combine(rule.SiteUrl, item.Groups[1].Value);
                ExcuteRecord(url);
                novelList.Add(url);
            }
            if (novelList.Count <= 0)
            {
                ExcuteRecord("获取更新列表失败");
                return;
            }

            //随机获取小说
            ExcuteRecord("---------------------------------------");
            rule.NovelUrl.Pattern = novelList[UtilityHelper.Random(0, novelList.Count)];
            ExcuteRecord($"随机获取小说：{rule.NovelUrl.Pattern}");

            //获取小说详情页
            ExcuteRecord("---------------------------------------");
            ExcuteRecord("获取小说详情页");
            var novelInfoHtml = HtmlHelper.Get(rule.NovelUrl.Pattern);
            if (string.IsNullOrEmpty(novelInfoHtml))
            {
                ExcuteRecord("小说详情页无法访问");
                return;
            }
            //匹配
            if (Regex.IsMatch(novelInfoHtml, rule.NovelErr.Pattern))
            {
                ExcuteRecord(rule.NovelErr.Pattern);
                return;
            }
            ExcuteRecord($"Name:{RegexMatch(rule.NovelName, novelInfoHtml)}");
            ExcuteRecord($"Image:{RegexMatch(rule.NovelImage, novelInfoHtml)}");
            ExcuteRecord($"Classify:{ RegexMatch(rule.NovelClassify, novelInfoHtml)}");
            ExcuteRecord($"Author:{RegexMatch(rule.NovelAuthor, novelInfoHtml)}");
            ExcuteRecord($"State:{ RegexMatch(rule.NovelState, novelInfoHtml)}");
            ExcuteRecord($"Des:{ RegexMatch(rule.NovelDes, novelInfoHtml)}");

            //章节列表
            rule.ChapterList.Pattern = rule.NovelUrl.Pattern;

            //获取小说章节页
            ExcuteRecord("---------------------------------------");
            ExcuteRecord("获取小说章节页");
            var chapterList = new List<KeyValuePair<string, string>>();
            var chapterListHtml = HtmlHelper.Get(rule.ChapterList.Pattern);
            if (string.IsNullOrEmpty(chapterListHtml))
            {
                ExcuteRecord("小说章节页无法访问");
                return;
            }
            var chapterNameMc = Regex.Matches(chapterListHtml, rule.ChapterName.Pattern);
            var chapterUrlMc = Regex.Matches(chapterListHtml, rule.ChapterUrl.Pattern);
            if (chapterNameMc.Count <= 0 || chapterUrlMc.Count <= 0 || chapterNameMc.Count != chapterUrlMc.Count)
            {
                ExcuteRecord("获取小说章节页失败");
                return;
            }
            for (int i = 0; i < chapterNameMc.Count; i++)
            {
                var name = chapterNameMc[i].Groups[1].Value.Trim();
                var url = chapterUrlMc[i].Groups[1].Value.Trim();
                ExcuteRecord($"{name} — {url}");
                chapterList.Add(new KeyValuePair<string, string>(name, url));
            }

            //随机获取章节
            var randomChapter = chapterList[UtilityHelper.Random(0, chapterList.Count)];
            ExcuteRecord("---------------------------------------");
            ExcuteRecord("随机获取章节：");
            ExcuteRecord(randomChapter.Key);
            ExcuteRecord(randomChapter.Value);
            var chapterUrl = UtilityHelper.Combine(rule.SiteUrl, randomChapter.Value);
            var chapterHtml = HtmlHelper.Get(chapterUrl);
            if (Regex.IsMatch(chapterHtml, rule.ContentErr.Pattern))
            {
                ExcuteRecord(rule.ContentErr.Pattern);
                return;
            }
            ExcuteRecord(RegexMatch(rule.ContentText, chapterHtml));
        }

        private string RegexMatch(PatternItem rule, string html)
        {
            var result = string.Empty;
            try
            {
                result = Regex.Match(html, rule.Pattern).Groups[1].Value;
            }
            catch { }
            return result;
        }

    }
}
