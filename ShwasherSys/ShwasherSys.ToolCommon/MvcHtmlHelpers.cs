using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ShwasherSys
{

    public static class HtmlHelpers
    {
        private class ScriptBlock : IDisposable
        {
            private const string ScriptsKey = "PartialViewScripts";
            public static List<string> PartialViewScripts
            {
                get
                {
                    if (HttpContext.Current.Items[ScriptsKey] == null)
                        HttpContext.Current.Items[ScriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[ScriptsKey];
                }
            }

            readonly WebViewPage _webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                _webPageBase = webPageBase;
                _webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                PartialViewScripts.Add(((StringWriter)_webPageBase.OutputStack.Pop()).ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PartialViewScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PartialViewScripts.Select(s => s.ToString())));
        }
        public static List<SelectListItem> TranSelectItems<T>(List<T> tList, string showColumn, string valueColumn) where T : new()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var li in tList)
            {
                var type = typeof(T);
                PropertyInfo showPro = type.GetProperty(showColumn);
                var show = showPro?.GetValue(li);
                PropertyInfo valuePro = type.GetProperty(valueColumn);
                var value = valuePro?.GetValue(li);
                list.Add(new SelectListItem()
                {
                    Text = show?.ToString(),
                    Value = value?.ToString()
                });
            }
            return list;
        }
    }
}
