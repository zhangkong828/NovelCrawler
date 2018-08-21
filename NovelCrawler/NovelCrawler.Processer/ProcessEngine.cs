using NovelCrawler.Infrastructure;
using NovelCrawler.Models;
using NovelCrawler.Processer.Models;
using NovelCrawler.Repository.IRepository;
using NovelCrawler.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.Processer
{
    public class ProcessEngine
    {
        private static readonly object _obj = new object();
        private static ProcessEngine _instance;
        private ProcessEngineOptions _options;

        private INovelInfoRepository _novelInfoRepository;

        private ProcessEngine(ProcessEngineOptions options)
        {
            _options = options;

            _novelInfoRepository = new NovelInfoRepository();
        }

        public static ProcessEngine Create(ProcessEngineOptions options = null)
        {
            if (_instance == null)
            {
                lock (_obj)
                {
                    if (_instance == null)
                    {
                        _instance = new ProcessEngine(options ?? new ProcessEngineOptions());
                    }
                }
            }
            return _instance;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }


        private void Process()
        {
            var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>("testRule.xml", Encoding.UTF8);
            var spider = new Spider(null, rule);

            //获取更新列表
            var novelKeys = spider.GetUpdateList();

            //并行抓取
            Parallel.ForEach(novelKeys, (novelKey, loopState) =>
            {
                try
                {
                    //获取小说详情
                    var info = spider.GetNovelInfo(novelKey);
                    //判断是否已入库
                    if (_novelInfoRepository.Exists(x => x.Name == info.Name && x.Author == info.Author))
                    {
                        ProcessUpdate(spider, novelKey, info);//更新
                    }
                    else
                    {
                        ProcessAdd(spider, novelKey, info);//新增
                    }

                }
                catch (SpiderException ex)
                {
                    Logger.Error("{0}，{1} 抓取失败：{2}", rule.SiteUrl, novelKey, ex.Message);
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, "ProcessEngine.Process");
                }
            });
        }


        private void ProcessAdd(Spider spider, string novelKey, NovelDetails info)
        {
            //入库小说详情
            var novelInfo = new NovelInfo()
            {
                Id = ObjectId.NextId(),
                Name = info.Name,
                Author = info.Author,
                Sort = info.Sort,
                State = info.State,
                Des = info.Des,
                Cover = spider.DownLoadImageToBase64(info.ImageUrl),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                LatestChapter = "",
                LatestChapterId = ""
            };
            //_novelInfoRepository.Insert(novelInfo);
            //获取章节列表
            var chapterList = spider.GetNovelChapterList(novelKey, info.ChapterIndex);
            //写入目录索引
            //抓取章节  单个抓取，不然容易被封
            //章节内容 存储按路由规则分表
        }

        private void ProcessUpdate(Spider spider, string novelKey, NovelDetails info)
        {

        }




    }
}
