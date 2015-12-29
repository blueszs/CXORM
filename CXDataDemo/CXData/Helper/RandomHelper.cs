using System;
using System.Collections.Generic;
using System.Text;

namespace CXData.Helper
{
    /// <summary>
    /// 随机字符串类
    /// 20150625-周盛-添加
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 获取7位随机码
        /// </summary>
        /// <returns></returns>
        public static string GetSevenRandomCode()
        {
            Random rd = new Random();
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";
            for (int i = 0; i < 7; i++)
            {
                result += str[rd.Next(str.Length)];
            }
            return result;
        }

        /// <summary>
        /// 字母和数字
        /// </summary>
        public const string NumberAndAlphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        /// <summary>
        /// 大写字母和数字
        /// </summary>
        public const string NumberAndUpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        /// <summary>
        /// 小写字母和数字
        /// </summary>
        public const string NumberAndLowerAlphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        /// <summary>
        /// 字母
        /// </summary>
        public const string Alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
        /// <summary>
        /// 数字
        /// </summary>
        public const string Number = "0123456789";
        /// <summary>
        /// 大写字母
        /// </summary>
        public const string UpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// 小写字母
        /// </summary>
        public const string LowerAlphabet = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="randomStrLen">生成的随机字符串长度</param>
        /// <param name="seedstr">随机字符提供种子字符串</param>
        /// <returns></returns>
        public static string GetRandStr(int randomStrLen, string seedstr = NumberAndAlphabet)
        {
            string randomStr = string.Empty;
            try
            {
                System.Security.Cryptography.RNGCryptoServiceProvider rng =
                    new System.Security.Cryptography.RNGCryptoServiceProvider();
                byte[] bytes = new byte[32];
                for (int i = 0; i < randomStrLen; i++)
                {
                    Array.Clear(bytes, 0, bytes.Length);
                    rng.GetBytes(bytes);
                    int num = Math.Abs(BitConverter.ToInt32(bytes, 0)%seedstr.Length);
                    randomStr += seedstr[num];
                }
            }
            catch
            {
                // ignored
            }
            return randomStr;
        }

        /// <summary>  
        /// 随机产生常用汉字  
        /// </summary>  
        /// <param name="count">要产生汉字的个数</param>  
        /// <returns>常用汉字</returns>  
        public static string RandChineseWords(int count)
        {
            try
            {
                List<string> chineseWords = new List<string>();
                Random rm = new Random();
                Encoding gb = Encoding.GetEncoding("gb2312");
                for (int i = 0; i < count; i++)
                {
                    // 获取区码(常用汉字的区码范围为16-55)  
                    int regionCode = rm.Next(16, 56);
                    // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)  
                    int positionCode = rm.Next(1, regionCode == 55 ? 90 : 95);
                    int regionCodeMachine = regionCode + 160; // 160即为十六进制的20H+80H=A0H  
                    int positionCodeMachine = positionCode + 160; // 160即为十六进制的20H+80H=A0H  

                    // 转换为汉字  
                    byte[] bytes = new [] {(byte) regionCodeMachine, (byte) positionCodeMachine};
                    chineseWords.Add(gb.GetString(bytes));
                }
                return string.Join("", chineseWords);
            }
            catch
            {
                // ignored
            }
            return "";
        }
    }
}
