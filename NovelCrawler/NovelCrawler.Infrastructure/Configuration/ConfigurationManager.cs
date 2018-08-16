using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NovelCrawler.Infrastructure.Configuration
{
    public class ConfigurationManager
    {
        static IConfiguration Configuration { get; set; }
        static ConfigurationManager()
        {
            var provider = new EnvironmentVariablesConfigurationProvider();
            provider.Load();
            provider.TryGet("ASPNETCORE_ENVIRONMENT", out string environmentName);

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// 获取配置节点，使用规则详情：https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1&tabs=basicconfiguration
        /// </summary>
        /// <param name="key">不区分大小写</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Configuration[key];
        }

        public static T GetValue<T>(string key)
        {
            return Configuration.GetValue<T>(key);
        }
    }
}
