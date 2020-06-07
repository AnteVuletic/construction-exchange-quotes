using System;

namespace ConstructionExchangeQuotes.Shared
{
    public static class Date
    {
        public static string ToIsoFormat(DateTime date)
        {
            return date.Date.ToString("yyyy-MM-dd");
        }
    }
}
