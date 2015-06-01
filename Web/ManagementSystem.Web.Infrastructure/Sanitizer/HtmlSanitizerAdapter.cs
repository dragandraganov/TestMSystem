using Ganss.XSS;
using System;
using System.Linq;

namespace ManagementSystem.Web.Infrastructure.Sanitizer
{
    public class HtmlSanitizerAdapter : ISanitizer
    {
        public string Sanitize(string html)
        {
            var sanitizer = new HtmlSanitizer();
            var result = sanitizer.Sanitize(html);
            return result;
        }
    }
}
