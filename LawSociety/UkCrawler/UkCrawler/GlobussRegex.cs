using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Globussoft.File;
using Globussoft.Utils;
using System.Threading;


namespace Globussoft.RegexExp
{
    class GlobussRegex
    {

        public string GetAnchorTag(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();
            string CatlogUrl = null;
            
            Regex anchorTextExtractor = new Regex(@"<a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                //if (!url.Value.Contains("img"))
                //{
                    //lstAnchorUrl.Add(url.Value);
                CatlogUrl = url.Value;
                //}
            }
            return CatlogUrl;
        }

        public string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public string GetUrlFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            string strUrl = string.Empty;
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                //lstUrl.Add(url.Value);
                strUrl = url.Value;
            }

            return strUrl;
        }

        public List<string> GetUrlsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
               
            }

            return lstUrl;
        }

        public List<string> GetEmailsFromString(string HtmlData)
        {
            List<string> lstEmail = new List<string>();
            var regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var PageData = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                lstEmail.Add(email.Value);
            }
            return lstEmail.Distinct().ToList();
        }
    }
}
