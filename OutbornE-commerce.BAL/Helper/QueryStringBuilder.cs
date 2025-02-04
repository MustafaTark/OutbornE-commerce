using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Helper
{
    public class QueryStringBuilder
    {
        private readonly List<KeyValuePair<string, string>> _params = new();

        public QueryStringBuilder Add(string key, string value)
        {
            _params.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public override string ToString()
        {
            return string.Join("&", _params.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
        }
    }
}
