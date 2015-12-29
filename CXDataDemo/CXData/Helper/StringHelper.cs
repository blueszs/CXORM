using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CXData.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// 获取正则表达式的值
        /// </summary>
        /// <param name="sInput">输入字段</param>
        /// <param name="spattern">正则表达</param>
        /// <returns></returns>
        public static string GetValue(this string sInput, string spattern)
        {
            Match titleMatch = Regex.Match(sInput, spattern, RegexOptions.IgnoreCase);
            string result = titleMatch.Value;
            return result;
        }

        /// <summary>
        /// 验证字符串是否正则表达式
        /// </summary>
        /// <param name="sInput">输入字段</param>
        /// <param name="spattern">正则表达</param>
        /// <returns></returns>
        public static bool IsMatch(this string sInput, string spattern)
        {
            return Regex.IsMatch(sInput, spattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取正则表达式的值
        /// </summary>
        /// <param name="sInput">输入字段</param>
        /// <param name="spattern">正则表达</param>
        /// <param name="groupsName">分组名</param>
        /// <returns></returns>
        public static string GetValueByGroup(this string sInput, string spattern, string groupsName)
        {
            Match titleMatch = Regex.Match(sInput, spattern, RegexOptions.IgnoreCase);
            string result = titleMatch.Groups[groupsName].Value;
            return result;
        }

        /// <summary>
        /// 获取所有匹配项的Group值集合
        /// </summary>
        /// <param name="sInput">输入字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="groupNames">分组数组</param>
        /// <returns></returns>
        public static ArrayList GetArrayAttr(this string sInput, string spattern, string[] groupNames)
        {
            MatchCollection matches = Regex.Matches(sInput, spattern, RegexOptions.IgnoreCase);
            ArrayList itemArrayList = new ArrayList();
            foreach (Match itemmatch in matches)
            {
                itemArrayList.Add(groupNames.Select(groupName => itemmatch.Groups[groupName].Value).ToArray());
            }
            return itemArrayList;
        }

        /// <summary>
        /// 获取所有匹配项的Group值集合
        /// </summary>
        /// <param name="sInput">输入字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="groupNames">分组数组</param>
        /// <returns></returns>
        public static List<string[]> GetArrayByGroupName(this string sInput, string spattern, string[] groupNames)
        {
            MatchCollection matches = Regex.Matches(sInput, spattern, RegexOptions.IgnoreCase);
            return (from Match itemmatch in matches select groupNames.Select(groupName => itemmatch.Groups[groupName].Value).ToArray()).ToList();
        }

        /// <summary>
        /// 获取正则表达式的值数组
        /// </summary>
        /// <param name="sInput">输入字段</param>
        /// <param name="spattern">正则表达</param>
        /// <param name="groupsName">分组</param>
        /// <returns></returns>
        public static string[] GetArrayByGroupName(this string sInput, string spattern, string groupsName)
        {
            MatchCollection matches = Regex.Matches(sInput, spattern, RegexOptions.IgnoreCase);
            List<string> resultList = new List<string>();
            if (matches.Count > 0)
            {
                var query = (from Match item in matches select item.Groups[groupsName]);
                IEnumerable<Group> enumerable = query as Group[] ?? query.ToArray();
                if (enumerable.Any())
                {
                    resultList = enumerable.Select(x => x.Value).ToList();
                }
            }
            return resultList.ToArray();
        }

        /// <summary>
        /// 按正则表达式替换内容后返回新的字符串
        /// </summary>
        /// <param name="strInput">原文本字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="strReplace">替换的文本</param>
        /// <returns>替换后的文本</returns>
        public static string ReplaceStr(this string strInput, string spattern, string strReplace)
        {
            Regex regex = new Regex(spattern, RegexOptions.IgnoreCase);
            return regex.Replace(strInput, strReplace);
        }

        /// <summary>
        /// 按正则表达式替换内容后返回新的字符串
        /// </summary>
        /// <param name="strInput">原文本字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="replaceitem">替换的分组名称和值</param>
        /// <returns>替换后的文本</returns>
        public static string ReplaceByGroup(this string strInput, string spattern, KeyValuePair<string, string> replaceitem)
        {
            string reval = strInput;
            MatchCollection matches = Regex.Matches(strInput, spattern, RegexOptions.IgnoreCase);
            foreach (Match itemmatch in matches)
            {
                string newreplace = itemmatch.Value;
                newreplace = newreplace.Replace(itemmatch.Groups[replaceitem.Key].Value, replaceitem.Value);
                reval = reval.Replace(itemmatch.Value, newreplace);
            }
            return reval;
        }

        /// <summary>
        /// 按正则表达式替换内容后返回新的字符串
        /// </summary>
        /// <param name="strInput">原文本字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="replaceitems">替换的分组名称和值</param>
        /// <returns>替换后的文本</returns>
        public static string ReplaceByGroup(this string strInput, string spattern, List<KeyValuePair<string, string>> replaceitems)
        {
            string reval = strInput;
            MatchCollection matches = Regex.Matches(strInput, spattern, RegexOptions.IgnoreCase);
            foreach (Match itemmatch in matches)
            {
                string newreplace = replaceitems.Aggregate(itemmatch.Value, (current, item) => current.Replace(itemmatch.Groups[item.Key].Value, item.Value));
                reval = reval.Replace(itemmatch.Value, newreplace);
            }
            return reval;
        }

        /// <summary>
        /// 根据正则表达式匹配符合的数据
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="parentPattern">上级正则表达式</param>
        /// <param name="childPattern">匹配数据的表达式</param>
        /// <param name="sgroup">返回数据的分组</param>
        /// <returns></returns>
        public static string GetValueByRecursiveGroup(this string strInput, string parentPattern, string childPattern, string sgroup)
        {
            Match match = Regex.Match(strInput, parentPattern, RegexOptions.IgnoreCase);
            string reval = string.Empty;
            if (match.Success)
            {
                Match childmatch = Regex.Match(match.Value, childPattern, RegexOptions.IgnoreCase);
                if (childmatch.Success)
                {
                    reval = childmatch.Groups[sgroup].Value;
                }
            }
            return reval;
        }

        /// <summary>
        /// 根据正则表达式匹配符合的数据数组
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="parentPattern">上级正则表达式</param>
        /// <param name="childPattern">匹配数据的表达式</param>
        /// <param name="sgroup">返回数据的分组</param>
        /// <returns></returns>
        public static string[] GetArrayByRecursiveGroup(this string strInput, string parentPattern, string childPattern, string sgroup)
        {
            Match match = Regex.Match(strInput, parentPattern, RegexOptions.IgnoreCase);
            List<string> reval = new List<string>();
            if (match.Success)
            {
                MatchCollection childmatchs = Regex.Matches(match.Value, childPattern, RegexOptions.IgnoreCase);
                if (childmatchs.Count > 0)
                {
                    var query = (from Match item in childmatchs select item.Groups[sgroup]);
                    IEnumerable<Group> enumerable = query as Group[] ?? query.ToArray();
                    if (enumerable.Any())
                    {
                        reval = enumerable.Select(x => x.Value).ToList();
                    }
                }
            }
            return reval.ToArray();
        }

        /// <summary>
        /// 根据正则表达式匹配符合的多个分组数据数组
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="parentPattern">上级正则表达式</param>
        /// <param name="childPattern">匹配数据的表达式</param>
        /// <param name="sgroup">返回数据的分组</param>
        /// <returns></returns>
        public static List<string[]> GetArrayByRecursiveGroup(this string strInput, string parentPattern, string childPattern, string[] sgroup)
        {
            MatchCollection parentmatchs = Regex.Matches(strInput, parentPattern, RegexOptions.IgnoreCase);
            List<string[]> reval = new List<string[]>();
            if (sgroup != null && sgroup.Length > 0)
            {
                if (parentmatchs.Count > 0)
                {
                    foreach (Match parentitem in parentmatchs)
                    {
                        MatchCollection childmatchs = Regex.Matches(parentitem.Value, childPattern, RegexOptions.IgnoreCase);
                        if (childmatchs.Count > 0)
                        {
                            reval.AddRange(from Match childitem in childmatchs select sgroup.Select(groupname => childitem.Groups[groupname].Success ? childitem.Groups[groupname].Value : "").ToArray());
                        }
                    }
                }
            }
            return reval;
        }


        /// <summary>
        /// 根据正则表达式匹配符合的分组最大值
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="sgroup">获取最大值的分组</param>
        /// <returns></returns>
        public static string GetMaxByRegexGroup(this string strInput, string spattern, string sgroup)
        {
            MatchCollection matches = Regex.Matches(strInput, spattern, RegexOptions.IgnoreCase);
            string maxval = string.Empty;
            if (matches.Count > 0)
            {
                var query = (from Match item in matches select item.Groups[sgroup]);
                IEnumerable<Group> enumerable = query as Group[] ?? query.ToArray();
                if (enumerable.Any())
                {
                    maxval = enumerable.Select(x => x.Value).Max();
                }
            }
            return maxval;
        }

        /// <summary>
        /// 根据正则表达式匹配符合的分组最大值
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="sgroup">获取最大值的分组</param>
        /// <returns></returns>
        public static string GetMinByRegexGroup(this string strInput, string spattern, string sgroup)
        {
            MatchCollection matches = Regex.Matches(strInput, spattern, RegexOptions.IgnoreCase);
            string minval = string.Empty;
            if (matches.Count > 0)
            {
                var query = (from Match item in matches select item.Groups[sgroup]);
                IEnumerable<Group> enumerable = query as Group[] ?? query.ToArray();
                if (enumerable.Any())
                {
                    minval = enumerable.Select(x => x.Value).Min();
                }
            }
            return minval;
        }

        /// <summary>
        /// 根据正则表达式匹配符合的个数
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="spattern">正则表达式</param>
        /// <param name="sgroup">获取最大值的分组</param>
        /// <returns></returns>
        public static int GetCountByRegexGroup(this string strInput, string spattern, string sgroup)
        {
            MatchCollection matches = Regex.Matches(strInput, spattern, RegexOptions.IgnoreCase);
            int countval = 0;
            if (matches.Count > 0)
            {
                var query = (from Match item in matches select item.Groups[sgroup]);
                IEnumerable<Group> enumerable = query as Group[] ?? query.ToArray();
                if (enumerable.Any())
                {
                    countval = enumerable.Select(x => x.Value).Count();
                }
            }
            return countval;
        }
    }
}
