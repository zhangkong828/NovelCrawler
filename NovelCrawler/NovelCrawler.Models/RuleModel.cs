using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
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
        [RuleDescription("站点编码")]
        public string SiteCharset { get; set; }

        [XmlElement(Order = 3)]
        [RuleDescription("最新列表地址")]
        public string NovelUpdateListUrl { get; set; }

        [XmlElement(Order = 4)]
        [RuleDescription("更新列表", "从最新列表中获得小说编号\r\n获得结果存入{NovelKey}变量")]
        public PatternItem NovelUpdateList { get; set; }

        [XmlElement(Order = 5)]
        [RuleDescription("小说地址", "小说详情地址\r\n可调用{NovelKey}变量")]
        public string NovelUrl { get; set; }

        [XmlElement(Order = 6)]
        [RuleDescription("小说页面错误标识", "判断小说页面能否正常打开")]
        public PatternItem NovelErr { get; set; }

        [XmlElement(Order = 7)]
        [RuleDescription("小说名称", "获得小说名称正则，可使用替换标签&&")]
        public PatternItem NovelName { get; set; }

        [XmlElement(Order = 8)]
        [RuleDescription("小说缩略图", "获得小说封面，可以使用替换标签&&")]
        public PatternItem NovelImage { get; set; }

        [XmlElement(Order = 9)]
        [RuleDescription("小说分类", "获得小说分类，可使用替换标签&&")]
        public PatternItem NovelClassify { get; set; }

        [XmlElement(Order = 10)]
        [RuleDescription("小说作者", "获得小说作者，可使用替换标签&&")]
        public PatternItem NovelAuthor { get; set; }

        [XmlElement(Order = 11)]
        [RuleDescription("小说简介", "获得小说简介，可使用替换标签&&")]
        public PatternItem NovelDes { get; set; }

        [XmlElement(Order = 12)]
        [RuleDescription("小说完结状态", "获得小说写作进程，可使用替换标签&&")]
        public PatternItem NovelState { get; set; }


        [XmlElement(Order = 13)]
        [RuleDescription("章节目录", "获得小说的章节目录Key\r\n在小说详情页获取\r\n获得结果存入{ChapterIndex}变量，不用可以不写")]
        public PatternItem ChapterIndex { get; set; }

        [XmlElement(Order = 14)]
        [RuleDescription("章节列表地址", "获得小说的完整章节目录列表地址\r\n可调用{NovelKey}，{ChapterIndex}变量")]
        public string ChapterList { get; set; }

        [XmlElement(Order = 15)]
        [RuleDescription("章节名称", "获得章节名称，可使用替换标签&&")]
        public PatternItem ChapterName { get; set; }

        [XmlElement(Order = 16)]
        [RuleDescription("章节地址", "获得章节地址，所取数量必须和章节名称一致\r\n获得结果存入{ChapterKey}变量")]
        public PatternItem ChapterUrl { get; set; }

        [XmlElement(Order = 17)]
        [RuleDescription("章节内容地址", "章节内容页地址\r\n可调用{NovelKey}，{ChapterIndex}，{ChapterKey}变量")]
        public string ContentUrl { get; set; }

        [XmlElement(Order = 18)]
        [RuleDescription("章节内容", "章节正文，可以使用替换标签&&")]
        public PatternItem ContentText { get; set; }

        [XmlElement(Order = 19)]
        [RuleDescription("章节错误标识", "判断章节页面能否正常打开采集")]
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
        public CDATA Filter { get; set; }

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


    public class CDATA : IXmlSerializable
    {
        public CDATA()
        {
        }
        public CDATA(string xml)
        {
            this.OuterXml = xml;
        }
        public string OuterXml { get; private set; }
        public string InnerXml { get; private set; }

        private string _innerSourceXml;
        public string InnerSourceXml
        {
            get
            {
                return InnerXml;
            }
        }
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string s = reader.ReadInnerXml();
            string startTag = "<![CDATA[";
            string endTag = "]]>";
            char[] trims = new char[] { '\r', '\n', '\t', ' ' };
            s = s.Trim(trims);
            if (s.StartsWith(startTag) && s.EndsWith(endTag))
            {
                s = s.Substring(startTag.Length, s.LastIndexOf(endTag) - startTag.Length);
            }
            this._innerSourceXml = s;
            this.InnerXml = s.Trim(trims);
        }
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteCData(this.OuterXml);
        }
    }
}

