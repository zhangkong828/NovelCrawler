using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Models
{
    public class NovelChapter
    {
        public string Id { get; set; }

        /// <summary>
        /// 小说Id
        /// </summary>
        public string NovelId { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>
        public string ChapterName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 字数
        /// </summary>
        public string WordCount { get; set; }

        /// <summary>
        /// 章节内容
        /// </summary>
        public string Content { get; set; }


    }
}
