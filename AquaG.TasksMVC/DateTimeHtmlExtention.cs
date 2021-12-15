using System;

namespace AquaG.TasksMVC
{
    public static class DateTimeHtmlExtention
    {

        public const string HtmlDateTimeFormatString = $"{{0:dd.MM.yyyy HH:mm}}";


        static public string GetNamedWeekDay(DateTime date)
        {
            var names = new string[] { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
            return names[(int)date.DayOfWeek];
        }

        static public string GetNamedMonth(DateTime date)
        {
            var names = new string[] { "янв", "фев", "мар", "апр", "май", "июнь", "июль", "авг", "сент", "окт", "ноя", "дек" };
            return names[date.Month - 1];
        }


        public static string ToTextHtml(this DateTime? dateValue)
        {
            if (!dateValue.HasValue) return "";
            var value = (DateTime)dateValue;

            string timePart;
            if (value.Hour != 0 || value.Minute != 0)
                timePart = ' ' + string.Format("{0:HH:mm}", value);
            else
                timePart = "";

            value = value.Date;
            var dayNow = DateTime.Now.Date;

            string datePart;
            if (value == dayNow)
                datePart = "Cегодня";
            else if (value == dayNow.AddDays(1))
                datePart = "Завтра";
            else if (value > dayNow && value < dayNow.AddDays(7))
                datePart = GetNamedWeekDay(value);
            else
                datePart = value.Day.ToString("00") + ' ' + GetNamedMonth(value) + (value.Year != dayNow.Year ?  ' ' + value.Year.ToString(): "");

            return datePart + timePart;
        }



    }
}
