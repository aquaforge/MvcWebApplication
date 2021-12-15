using System;

namespace AquaG.TasksMVC
{
    public static class DateTimeHtmlExtention
    {

        private const string _htmlMonthFormat = "dd.MM";
        private const string _htmlDateFormat = $"{_htmlMonthFormat}.yy";
        private const string _htmlTimeFormat = "HH:mm";

        public const string HtmlDateTimeFormatString = $"{{0:{_htmlDateFormat} {_htmlTimeFormat}}}";
        public const string HtmlDateFormatString = $"{{0:{_htmlDateFormat}}}";
        public const string HtmlTimeFormatString = $"{{0:{_htmlTimeFormat}}}";
        public const string HtmlMonthFormatString = $"{{0:{_htmlMonthFormat}}}";

        public static string ToTextHtml(this DateTime? dateValue)
        {
            if (!dateValue.HasValue) return "";
            var value = (DateTime)dateValue;

            string datePart = "";
            string timePart = "";

            if (value.Hour != 0 || value.Minute != 0) timePart = string.Format(HtmlTimeFormatString, value);

            value = value.Date;
            var dayNow = DateTime.Now.Date;

            if (value == dayNow)
                datePart = "сегодня";
            else if (value == dayNow.AddDays(1))
                datePart = "завтра";
            else if (value == dayNow.AddDays(2))
                datePart = "послезавтра";
            else if (value == dayNow.AddDays(-1))
                datePart = "вчера";
            else
                datePart = string.Format(HtmlDateFormatString, value);

            return datePart + ' ' + timePart;
        }

    }
}
