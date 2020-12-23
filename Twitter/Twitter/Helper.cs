using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace Twitter
{
    public static class Helper
    {
        public static string CheckHeader(HttpRequest re)
        {
            if (re == null)
            {
                throw new ArgumentNullException(nameof(re));
            }
            var headers = re.Headers;
            string source = string.Empty;
            if (headers.ContainsKey("Source"))
            {
                if (headers.TryGetValue("Source", out StringValues token))
                {
                    source = token.ToString();
                }
            }
            return source;
        }
    }
}
