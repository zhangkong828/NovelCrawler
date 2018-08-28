using NovelCrawler.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelCrawler.Infrastructure
{
    public class Utils
    {
        /// <summary>
        /// 对象的属性是否都不为null或空,仅限string类型，可设置忽略项
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignores">忽略的属性名</param>
        /// <returns></returns>
        public static bool ObjectIsNotNull<T>(T o, params string[] ignores) where T : class
        {
            var t = typeof(T);
            foreach (var pi in t.GetProperties())
            {
                if (pi.PropertyType.Name == "String")
                {
                    if (ignores.Contains(pi.Name))
                        continue;
                    var v = pi.GetValue(o);
                    if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 统计字数。目前只统计汉字和英文单词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetWordCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            //清理html标签
            text = Regex.Replace(text, "<\\/?.+?\\/?>", " ");

            //英文单词  连词不算1个，算2个单词
            var reg1 = new Regex(@"[A-Za-z0-9][A-Za-z0-9\-.]*");
            int count = reg1.Matches(text).Count;

            var chats = text.ToCharArray();
            int count1 = 0;//汉字
            int count2 = 0;//字母(单个字母)
            int count3 = 0;//数字(单个数字)

            for (int i = 0; i < chats.Length; i++)
            {
                var c = (int)chats[i];
                if (c >= 0x4E00 && c <= 0x9FA5)//汉字
                {
                    count1++;
                }
                else if ((c >= 65 && c <= 90) || c >= 97 && c <= 122)//字母
                {
                    count2++;
                }
                else if (c >= 48 && c <= 57)//数字
                {
                    count3++;
                }
            }
            return count + count1;
        }


        public static bool CompareChapter(string text1, string text2, out double similarity)
        {
            similarity = 0;
            if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
                return false;
            //字符串特殊处理
            text1 = ReplaceNumberToChinese(ReplaceSpechars(text1));
            text2 = ReplaceNumberToChinese(ReplaceSpechars(text2));

            LevenshteinDistance(text1, text2, out similarity);

            return similarity > 0.8;//相似度大于80% 则认为相同
        }

        /// <summary>  
        /// 字符串相似度 编辑距离（Levenshtein Distance）  
        /// </summary>  
        /// <param name="source">源串</param>  
        /// <param name="target">目标串</param>  
        /// <param name="similarity">输出：相似度，值在0～１</param>  
        /// <param name="isCaseSensitive">是否大小写敏感</param>  
        /// <returns>源串和目标串之间的编辑距离</returns>  
        private static int LevenshteinDistance(string source, string target, out double similarity, bool isCaseSensitive = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target))
                {
                    similarity = 1;
                    return 0;
                }
                else
                {
                    similarity = 0;
                    return target.Length;
                }
            }
            else if (string.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            string From, To;
            if (isCaseSensitive)
            {   // 大小写敏感  
                From = source;
                To = target;
            }
            else
            {   // 大小写无关  
                From = source.ToLower();
                To = target.ToLower();
            }

            // 初始化  
            int m = From.Length;
            int n = To.Length;
            int[,] H = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++) H[i, 0] = i;  // 注意：初始化[0,0]  
            for (int j = 1; j <= n; j++) H[0, j] = j;

            // 迭代  
            for (int i = 1; i <= m; i++)
            {
                char SI = From[i - 1];
                for (int j = 1; j <= n; j++)
                {   // 删除（deletion） 插入（insertion） 替换（substitution）  
                    if (SI == To[j - 1])
                        H[i, j] = H[i - 1, j - 1];
                    else
                        H[i, j] = Math.Min(H[i - 1, j - 1], Math.Min(H[i - 1, j], H[i, j - 1])) + 1;
                }
            }

            // 计算相似度  
            int MaxLength = Math.Max(m, n);   // 两字符串的最大长度  
            similarity = ((double)(MaxLength - H[m, n])) / MaxLength;

            return H[m, n];    // 编辑距离  
        }

        /// <summary>
        /// 替换特殊字符
        /// </summary>
        /// <returns></returns>
        private static string ReplaceSpechars(string str)
        {
            return Regex.Replace(str.Replace(" ", ""), "[`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）|{}【】；‘’，。！/*-+]+", "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 将'章'前面的数字替换为汉字数字，没有匹配到就原样返回
        /// </summary>
        private static string ReplaceNumberToChinese(string str)
        {
            var reg = new Regex("(\\d+?)章");
            if (reg.IsMatch(str))
            {
                var s = reg.Match(str).Groups[1].Value;
                if (int.TryParse(s, out int num))
                {
                    var numStr = ConvertExtension.ConvertNumberToChinese(num);
                    return str.Replace(s, numStr);
                }
            }
            return str;
        }

    }
}
