using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CMICT.CSP.Web
{
    public class BaseWebPart : WebPart
    {
        /// <summary>
        /// Getts the current user.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserLoginId(int type=0)
        {
            SPUser currentSpUser = SPContext.Current.Web.CurrentUser;
            if (currentSpUser == null)
                return "";
            string currentSpUserLoginId = currentSpUser.LoginName;
            if (currentSpUserLoginId.ToLower() == "sharepoint\\system")
            {
                currentSpUserLoginId = HttpContext.Current.User.Identity.Name;
            }
            if (currentSpUserLoginId.Contains("|"))
            {
                currentSpUserLoginId = currentSpUserLoginId.Split('|')[1];
            }
            if (type == 0)
            {
                return currentSpUserLoginId + ";" + CMICT.CSP.BLL.Components.BaseComponent.GetDisplayName(currentSpUserLoginId);
            }
            else
            {
                if (currentSpUserLoginId.Contains('\\'))
                {
                    return currentSpUserLoginId.Split('\\')[1];
                }
                else
                {
                    return currentSpUserLoginId;
                }
            }
        }
        //sql过滤关键字   
        public static bool CheckKeyWord(string sWord)
        {
            //过滤关键字
            string StrKeyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
            //过滤关键字符
            string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            if (Regex.IsMatch(sWord, StrKeyWord, RegexOptions.IgnoreCase) || Regex.IsMatch(sWord, StrRegex))
                return true;
            return false;
        }
    }
}
