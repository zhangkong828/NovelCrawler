using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Processer
{
    public class ProcessEngineOptions
    {
        public TimeSpan SpiderIntervalTime { get; set; }

        public SpiderOptions SpiderOptions { get; set; }
    }

    public class SpiderOptions
    {
        public bool 添加新书 { get; set; }

        public bool 不处理已完成小说 { get; set; }

        public bool 强制清空重采 { get; set; }

        public bool 自动更新封面 { get; set; }

        public bool 自动更新连载状态 { get; set; }

        public bool 自动更新分类 { get; set; }

        public bool 自动更新简介 { get; set; }

        public bool 入库章节时是否添加文字广告 { get; set; }

        public List<string> 文字广告集合 { get; set; }

    }


    public enum 错误章节处理方式
    {
        入库章节名_继续采集下一章 = 0,
        跳过本章_继续采集下一章 = 1,
        停止本书_继续采集下一本 = 2
    }
}
