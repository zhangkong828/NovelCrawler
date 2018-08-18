using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Models
{
    public class NovelInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 小说名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 小说作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 小说分类
        /// </summary>
        public string Classify { get; set; }

        /// <summary>
        /// 小说状态
        /// 0：未完结
        /// 1：已完本
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 小说描述
        /// </summary>
        public string Des { get; set; }

        /// <summary>
        /// 小说封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 最新章节名称
        /// </summary>
        public string LatestChapter { get; set; }

        /// <summary>
        /// 最新章节地址
        /// </summary>
        public string LatestChapterId { get; set; }
    }
}
