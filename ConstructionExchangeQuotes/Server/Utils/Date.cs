using System;

namespace ConstructionExchangeQuotes.Server.Utils
{
    public static class Date
    {
        public static string ToIsoFormat(DateTime date)
        {
            return date.Date.ToString("yyyy-MM-dd");
        }
    }
}
