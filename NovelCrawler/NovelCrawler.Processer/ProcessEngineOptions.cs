using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Processer
{
    public class ProcessEngineOptions
    {
        public ProcessEngineOptions()
        {
            SpiderIntervalTime = new TimeSpan(0, 5, 0);
            SpiderOptions = new SpiderOptions();
        }

        public TimeSpan SpiderIntervalTime { get; set; }

        public SpiderOptions SpiderOptions { get; set; }
    }

    public class SpiderOptions
    {
        public SpiderOptions()
        {
            添加新书 = true;
            自动更新分类 = true;
            错误章节处理 = 错误章节处理.停止本书_继续采集下一本;
        }

        public bool 添加新书 { get; set; }

        public bool 不处理已完成小说 { get; set; }

        public bool 强制清空重采 { get; set; }

        public bool 自动更新封面 { get; set; }

        public bool 自动更新分类 { get; set; }

        public bool 自动更新简介 { get; set; }

        public bool 入库章节时是否添加文字广告 { get; set; }

        public List<string> 文字广告集合 { get; set; }

        public 错误章节处理 错误章节处理 { get; set; }
    }


    public enum 错误章节处理
    {
        停止本书_继续采集下一本 = 0,
        入库章节名_继续采集下一章 = 1
    }
}
