using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace NovelCrawler.Models
{
    [XmlRoot("Rule")]
    public class RuleModel
    {
        [XmlElement(Order = 0)]
        [RuleDescription("站点名称")]
        public string SiteName { get; set; }

        [XmlElement(Order = 1)]
        [RuleDescription("站点地址")]
        public string SiteUrl { get; set; }

        [XmlElement(Order = 2)]
        [RuleDescription("更新列表Url", "需要采集更新的列表地址")]
        public string NovelUpdateListUrl { get; set; }

        [XmlElement(Order = 3)]
        [RuleDescription("更新列表", "更新列表的正则表达式")]
        public PatternItem NovelUpdateList { get; set; }

        [XmlElement(Order = 4)]
        [RuleDescription("小说Url", "")]
        public PatternItem NovelUrl { get; set; }

        [XmlElement(Order = 5)]
        [RuleDescription("小说页面错误", "判断小说页面能否正常打开采集")]
        public PatternItem NovelErr { get; set; }

        [XmlElement(Order = 6)]
        [RuleDescription("小说名称", "")]
        public PatternItem NovelName { get; set; }

        [XmlElement(Order = 7)]
        [RuleDescription("小说缩略图", "")]
        public PatternItem NovelImage { get; set; }

        [XmlElement(Order = 8)]
        [RuleDescription("小说分类", "")]
        public PatternItem NovelClassify { get; set; }

        [XmlElement(Order = 9)]
        [RuleDescription("小说作者", "")]
        public PatternItem NovelAuthor { get; set; }

        [XmlElement(Order = 10)]
        [RuleDescription("小说简介", "")]
        public PatternItem NovelDes { get; set; }

        [XmlElement(Order = 11)]
        [RuleDescription("小说完结状态", "")]
        public PatternItem NovelState { get; set; }


        [XmlElement(Order = 12)]
        [RuleDescription("章节列表", "")]
        public PatternItem ChapterList { get; set; }

        [XmlElement(Order = 13)]
        [RuleDescription("章节名称", "")]
        public PatternItem ChapterName { get; set; }

        [XmlElement(Order = 14)]
        [RuleDescription("章节地址", "")]
        public PatternItem ChapterUrl { get; set; }

        [XmlElement(Order = 15)]
        [RuleDescription("章节内容", "章节正文，可以使用替换规则，去除相关文字水印和广告")]
        public PatternItem ContentText { get; set; }

        [XmlElement(Order = 16)]
        [RuleDescription("章节错误", "判断章节页面能否正常打开采集")]
        public PatternItem ContentErr { get; set; }
    }

    public class PatternItem
    {
        [XmlIgnore]
        public string Source { get; set; }
        [RuleDescription("Key")]
        public string Key { get; set; }
        [RuleDescription("采集规则", "正则表达式")]
        public string Pattern { get; set; }
        [RuleDescription("过滤规则", "正则表达式替换")]
        public string Filter { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class RuleDescriptionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public RuleDescriptionAttribute(string name, string description = null)
        {
            Name = name;
            Description = string.IsNullOrWhiteSpace(description) ? name : description;
        }
    }

}
