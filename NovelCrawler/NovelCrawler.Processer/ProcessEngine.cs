using NovelCrawler.Infrastructure;
using NovelCrawler.Infrastructure.Router;
using NovelCrawler.Models;
using NovelCrawler.Processer.Models;
using NovelCrawler.Repository.IRepository;
using NovelCrawler.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovelCrawler.Processer
{
    public class ProcessEngine
    {
        private static readonly object _obj = new object();
        private static ProcessEngine _instance;
        private ProcessEngineOptions _options;

        private INovelInfoRepository _novelInfoRepository;
        private INovelChapterRepository _novelChapterRepository;

        private ProcessEngine(ProcessEngineOptions options)
        {
            _options = options;

            _novelInfoRepository = new NovelInfoRepository();
            _novelChapterRepository = new NovelChapterRepository();
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
                    Logger.Error("{0}，{1} 小说详情抓取失败：{2}", rule.SiteUrl, novelKey, ex.Message);
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, "ProcessEngine.Process");
                }
            });
        }


        private void ProcessAdd(Spider spider, string novelKey, NovelDetails info)
        {
            var chapterIndex = info.ChapterIndex;

            var novelId = ObjectId.NextId();
            var novelCover = spider.DownLoadImageToBase64(info.ImageUrl);
            //入库小说详情
            var novelInfo = new NovelInfo()
            {
                Id = novelId,
                Name = info.Name,
                Author = info.Author,
                Sort = info.Sort,
                State = info.State,
                Des = info.Des,
                Cover = novelCover,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                LatestChapter = "",
                LatestChapterId = ""
            };
            _novelInfoRepository.Insert(novelInfo);
            //获取章节列表
            var chapterList = spider.GetNovelChapterList(novelKey, chapterIndex);
            //抓完在写 目录索引
            //获取分片id
            var shardingId = Route.GetShardingId(novelId);
            //抓取章节  单个抓取，不然容易被封
            var indexes = new List<Index>();
            for (int i = 0; i < chapterList.Count; i++)
            {
                var chapter = chapterList[i];
                try
                {
                    var content = spider.GetContent(novelKey, chapterIndex, chapter.Value);
                    var chapterId = ObjectId.NextId();
                    var chapterEntity = new NovelChapter()
                    {
                        Id = chapterId,
                        NovelId = novelId,
                        ChapterName = chapter.Key,
                        UpdateTime = DateTime.Now,
                        WordCount = Utils.GetWordCount(content),
                        Content = content
                    };
                    _novelChapterRepository.Insert(novelId, chapterEntity);
                    indexes.Add(new Index() { ChapterId = chapterId, ChapterName = chapter.Key });//索引目录
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    Logger.Error("{0}-{1} 小说章节抓取失败：{2}", chapter.Key, chapter.Value, ex.Message);
                    //单章节 抓取失败
                    //todo
                }
            }

            //章节内容 存储按路由规则分表
        }

        private void ProcessUpdate(Spider spider, string novelKey, NovelDetails info)
        {

        }




    }
}
