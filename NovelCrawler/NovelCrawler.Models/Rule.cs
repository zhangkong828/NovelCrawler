using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NovelCrawler.Models
{
    public class Rule
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        [XmlElement(Order = 0)]
        public string SiteName { get; set; }
        /// <summary>
        /// 站点地址
        /// </summary>
        [XmlElement(Order = 1)]
        public string SiteUrl { get; set; }
        /// <summary>
        /// 更新列表
        /// </summary>
        [XmlElement(Order = 2)]
        public string NovelUpdateListUrl { get; set; }
        /// <summary>
        /// 更新列表
        /// </summary>
        [XmlElement(Order = 3)]
        public RuleItem NovelUpdateList { get; set; }
        /// <summary>
        /// 小说地址
        /// </summary>
        [XmlElement(Order = 4)]
        public RuleItem NovelUrl { get; set; }
        /// <summary>
        /// 找不到小说
        /// </summary>
        [XmlElement(Order = 5)]
        public RuleItem NovelErr { get; set; }
        /// <summary>
        /// 小说名称
        /// </summary>
        [XmlElement(Order = 6)]
        public RuleItem NovelName { get; set; }
        /// <summary>
        /// 小说缩略图
        /// </summary>
        [XmlElement(Order = 7)]
        public RuleItem NovelImage { get; set; }
        /// <summary>
        /// 小说分类
        /// </summary>
        [XmlElement(Order = 8)]
        public RuleItem NovelClassify { get; set; }
        /// <summary>
        /// 小说作者
        /// </summary>
        [XmlElement(Order = 9)]
        public RuleItem NovelAuthor { get; set; }
        /// <summary>
        /// 小说描述详情
        /// </summary>
        [XmlElement(Order = 10)]
        public RuleItem NovelDes { get; set; }
        /// <summary>
        /// 小说状态
        /// </summary>
        [XmlElement(Order = 11)]
        public RuleItem NovelState { get; set; }

        /// <summary>
        /// 章节列表
        /// </summary>
        [XmlElement(Order = 12)]
        public RuleItem ChapterList { get; set; }
        /// <summary>
        /// 章节名称
        /// </summary>
        [XmlElement(Order = 13)]
        public RuleItem ChapterName { get; set; }
        /// <summary>
        /// 章节地址
        /// </summary>
        [XmlElement(Order = 14)]
        public RuleItem ChapterUrl { get; set; }
        /// <summary>
        /// 章节内容
        /// </summary>
        [XmlElement(Order = 15)]
        public RuleItem ContentText { get; set; }
        /// <summary>
        /// 错误章节
        /// </summary>
        [XmlElement(Order = 16)]
        public RuleItem ContentErr { get; set; }
    }

    public class RuleItem
    {
        [XmlIgnore]
        public string Source { get; set; }
        public string Key { get; set; }
        public string Pattern { get; set; }
        public string FilterPattern { get; set; }
    }



}
