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
        [XmlElement]
        public string SiteName { get; set; }

        [XmlElement]
        public string SiteUrl { get; set; }

        [XmlElement]
        public string NovelUpdateListUrl { get; set; }

        [XmlElement]
        public RuleItem NovelUpdateList { get; set; }

        [XmlElement]
        public RuleItem NovelUrl { get; set; }

        [XmlElement]
        public RuleItem NovelErr { get; set; }

        [XmlElement]
        public RuleItem NovelName { get; set; }

        [XmlElement]
        public RuleItem NovelImage { get; set; }

        [XmlElement]
        public RuleItem NovelClassify { get; set; }

        [XmlElement]
        public RuleItem NovelAuthor { get; set; }

        [XmlElement]
        public RuleItem NovelDes { get; set; }

        [XmlElement]
        public RuleItem NovelState { get; set; }


        [XmlElement]
        public RuleItem ChapterList { get; set; }
        [XmlElement]
        public RuleItem ChapterName { get; set; }
        [XmlElement]
        public RuleItem ChapterUrl { get; set; }

        [XmlElement]
        public RuleItem ContentText { get; set; }
        [XmlElement]
        public RuleItem ContentErr { get; set; }
    }

    public class RuleItem
    {
        public string Key { get; set; }
        public string Pattern { get; set; }
        public string FilterPattern { get; set; }
    }



}
