using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Models
{
    public class NovelIndex
    {
        public string Id { get; set; }

        /// <summary>
        /// 小说id
        /// </summary>
        public string NovelId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 索引目录
        /// </summary>
        public List<Index> Indexex { get; set; }
    }

    public class Index
    {
        /// <summary>
        /// 章节id
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>
        public string ChapterName { get; set; }


    }
}
