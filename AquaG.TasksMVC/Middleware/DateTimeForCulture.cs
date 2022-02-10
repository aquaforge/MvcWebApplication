using System;
using System.Globalization;

namespace AquaG.TasksMVC.Middleware
{
    public static class DateTimeForCulture
    {
        public static string HtmlFormDateTimeFormatString => $"{CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern} HH:mm";


        public static string ToHtmlForm(this DateTime? dateValue)
        {
            if (!dateValue.HasValue) return "";
            return string.Format("{0:g}", (DateTime)dateValue);
        }


        public static string ToHtmlText(this DateTime? dateValue)
        {
            if (!dateValue.HasValue) return "";
            var value = (DateTime)dateValue;

            string timePart = "";
            if (value.Hour != 0 || value.Minute != 0) timePart = ' ' + string.Format("{0:HH:mm}", value);

            value = value.Date;
            var dayNow = DateTime.Now.Date;


            string cultureUI = CultureInfo.CurrentUICulture.Name[..2].ToLower();

            string datePart;
            if (value == dayNow)
                datePart = cultureUI == "ru" ? "Cегодня" : "Today";
            else if (value == dayNow.AddDays(1))
                datePart = cultureUI == "ru" ? "Завтра" : "Tomorrow";
            else if (value > dayNow && value < dayNow.AddDays(7))
                datePart = string.Format("{0:dddd}", value);
            else if (value.Year == dayNow.Year)
                datePart = string.Format("{0:dddd} {0:M}", value);
            else
                datePart = string.Format("{0:dddd} {0:d}", value);

            return datePart + timePart;
        }
    }
}
