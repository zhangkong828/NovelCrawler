using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace NovelCrawler.Infrastructure
{
    public class HtmlHelper
    {
        public static byte[] DownLoad(string url)
        {
            int tryCount = 3;
            GetImage:
            try
            {
                WebClient client = new WebClient();
                var result = client.DownloadData(url);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, url);
                if (tryCount > 0)
                {
                    tryCount--;
                    goto GetImage;
                }
                return null;
            }
        }

        public static bool DownLoad(string url, string savePath)
        {
            int tryCount = 3;
            GetImage:
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(url, savePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, url);
                if (tryCount > 0)
                {
                    tryCount--;
                    goto GetImage;
                }
                return false;
            }
        }

        public static string Get(string url, string cookie = null, string encodingStr = "UTF8")
        {
            var html = "";
            var encoding = Encoding.UTF8;
            try
            {
                encoding = Encoding.GetEncoding(encodingStr);
            }
            catch { }
            int tryCount = 3;
            GetHtml:
            bool isError = false;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                if (cookie != null)
                    request.Headers[HttpRequestHeader.Cookie] = cookie;
                request.Timeout = 1000 * 10;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    html = GetResponseBody(response, encoding);
                }
                isError = string.IsNullOrWhiteSpace(html);
            }
            catch (Exception ex)
            {
                isError = true;
                Logger.Error(ex, url);
            }
            if (isError)
            {
                if (tryCount > 0)
                {
                    tryCount--;
                    goto GetHtml;
                }
            }
            return html;
        }


        private static string GetResponseBody(HttpWebResponse response, Encoding encoding)
        {
            string responseBody = string.Empty;
            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(
                    response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader =
                        new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }
    }
}
