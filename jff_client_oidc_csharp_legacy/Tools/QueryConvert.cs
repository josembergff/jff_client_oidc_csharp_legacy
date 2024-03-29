﻿using System;
using System.Linq;
using System.Web;

namespace jff_client_oidc_csharp_legacy.Tools
{
    public static class QueryConvert
    {
        public static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
