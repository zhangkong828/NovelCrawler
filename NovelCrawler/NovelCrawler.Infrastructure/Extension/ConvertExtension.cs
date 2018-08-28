using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Extension
{
    public static class ConvertExtension
    {
        public static Dictionary<int, string> numberTable = new Dictionary<int, string>();
        public static Dictionary<int, string> digitTable = new Dictionary<int, string>();

        static ConvertExtension()
        {
            numberTable.Add(0, "零");
            numberTable.Add(1, "一");
            numberTable.Add(2, "二");
            numberTable.Add(3, "三");
            numberTable.Add(4, "四");
            numberTable.Add(5, "五");
            numberTable.Add(6, "六");
            numberTable.Add(7, "七");
            numberTable.Add(8, "八");
            numberTable.Add(9, "九");
            digitTable.Add(1, "零");
            digitTable.Add(2, "十");
            digitTable.Add(3, "百");
            digitTable.Add(4, "千");
            digitTable.Add(5, "万");
            digitTable.Add(6, "十");
            digitTable.Add(7, "百");
            digitTable.Add(8, "千");
            digitTable.Add(9, "亿");
        }

        public static string ConvertNumberToChinese(int number)
        {
            bool flag = false; //是否有连续个零
            var list = new List<string>();

            string s = number.ToString();
            int len = s.Length;
            //零做单独的处理
            if (len == 1 && number == 0)
            {
                list.Add("零");
            }
            for (int i = 1; i <= len; i++)
            {
                int digit = number % 10;
                number = number / 10;
                if (i == 1)
                {
                    if (digit != 0)
                    {
                        list.Add(numberTable[digit]);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                {
                    if (digit != 0)
                    {
                        list.Add(digitTable[i]);
                        list.Add(numberTable[digit]);
                        flag = false;
                    }
                    else
                    {
                        if (flag == false)
                        {
                            list.Add(numberTable[digit]);
                            flag = true;
                        }
                    }
                }
            }

            list.Reverse();
            return string.Join("", list);
        }

    }
}
