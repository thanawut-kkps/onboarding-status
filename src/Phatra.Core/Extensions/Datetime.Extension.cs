using System;
using System.Globalization;

namespace Phatra.Core.Extensions
{
    /// <summary>
    /// enum สำหรับระบุว่าการที่จะใช้ method ใน utility นี้มีวันที่แบบไหน วันที่ไทย หรือ วันที่อังกฤษ
    /// </summary>
    public enum YearType
    {
        Thai = 0, English = 1
    }

    /// <summary>
    /// enum สำหรับระบุว่าวันที่ที่อยู่ในรูปแบบ string แบบไหนที่ต้องการจัดการ วันที่จะถูกคั่นด้วย สัญลักษณ์ /
    /// </summary>
    public enum DateFormat
    {
        ddmmyy = 0,
        ddmmmyy = 1,
        ddmmyyyy = 3,
        yyyymmdd = 4,
        ddmmmyyyy = 5,
        ddmmmmyyyy = 6
    }

    public static class DatetimeExtension
    {
        /// <summary>
        /// วันที่ในรูปแบบของ ไทย (พ.ศ)
        /// </summary>
        private static CultureInfo _cultureThai = new CultureInfo("th-th");

        /// <summary>
        /// วันที่ในรูปแบบของ อังกฤษ (ค.ศ)
        /// </summary>
        private static CultureInfo _cultureEnglish = new CultureInfo("en-us");

        /// <summary>
        /// แปลงจาก string ที่อยู่ในรูปแบบ dd/mm/yy ให้เป็น DateTime
        /// </summary>
        /// <param name="strDate">string ของวันที่ที่อยู่ในรูปแบบ dd/mm/yy และปีเป็นปี ค.ศ</param>
        /// <returns>DateTime</returns>
        public static DateTime? ToDate(this string source)
        {
            return ToDate(source, DateFormat.ddmmyy, YearType.English);
        }

        /// <summary>
        /// แปลง string ตาม format ที่ระบุ ให้เป็น DateTime
        /// </summary>
        /// <param name="strDate">string ของวันที่ที่ต้องการแปลง</param>
        /// <param name="format">format วันที่ที่ส่งมาว่าอยู่ในรูปแบบไหน</param>
        /// <param name="year">ปี พ.ศ หรือ ปี ค.ศ</param>
        /// <returns>DateTime</returns>
        public static DateTime? ToDate(this string source, DateFormat format, YearType year)
        {
            if (source == string.Empty) return null;
            return DateTime.ParseExact(source, GetFormatDate(format), GetCulture(year));
        }

        public static DateTime? DataDateToDate(this string source)
        {
            if (source == string.Empty) return null;
            return DateTime.ParseExact(source, GetFormatDate(DateFormat.yyyymmdd, string.Empty), GetCulture(YearType.English));
        }

        /// <summary>
        /// แปลงวันที่ DateTime เป็น string display date ให้อยู่ในรูแปแบบ dd/mm/yy และแสดงเป็นปี ค.ศ
        /// </summary>
        /// <param name="d">DateTime ที่ต้องการแปลง</param>
        /// <returns>string display date ให้อยู่ในรูแปแบบ dd/mm/yy และแสดงเป็นปี ค.ศ</returns>
        public static string ToDisplayDate(this DateTime? d)
        {
            if (d == null) return string.Empty;
            return ToDisplayDate(d.Value, DateFormat.ddmmmyy, YearType.English);
        }

        /// <summary>
        /// แปลงวันที่ DateTime เป็น string สำหรับ display date ตามรูปแบบที่ต้องการ
        /// </summary>
        /// <param name="d">DateTime ที่ต้องการแปลง</param>
        /// <param name="format">format ที่ต้องการแปลง</param>
        /// <param name="year">ปีที่ต้องการแปลง</param>
        /// <returns>string สำหรับ display date ตามรูปแบบที่ต้องการ</returns>
        public static string ToDisplayDate(this DateTime? d, DateFormat format, YearType year)
        {
            if (!d.HasValue) return string.Empty;
            return d.Value.ToString(GetFormatDate(format), GetCulture(year));
        }

        public static string ToDisplayDate(this DateTime? d, DateFormat format, string separator, YearType year)
        {
            if (!d.HasValue) return string.Empty;
            return d.Value.ToString(GetFormatDate(format, separator), GetCulture(year));
        }

        public static string ToDataDate(this DateTime? d)
        {
            if (!d.HasValue) return string.Empty;
            return d.Value.ToString(GetFormatDate(DateFormat.yyyymmdd, string.Empty), GetCulture(YearType.English));
        }

        public static string ToDisplayTime(this DateTime date)
        {
            return ToDisplayTime(date, "");
        }

        public static string ToDisplayTime(this DateTime date, string separetor)
        {
            return date.ToString("HH" + separetor + "mm" + separetor + "ss");
        }

        /// <summary>
        /// method สำหรับตรวจสอบว่าวันที่ที่ส่งมาถูกต้องตามรูปแบบ dd/mm/yy และเป็นปี ค.ศ หรือไม่
        /// </summary>
        /// <param name="strDate">string ที่อยู่ในรูปแบบ dd/mm/yy </param>
        /// <returns>true ผ่านการ validate , false ไม่ผ่านการ validate</returns>
        public static bool IsValid(this string strDate)
        {
            return IsValid(strDate, DateFormat.ddmmyy, YearType.English);
        }

        /// <summary>
        /// method สำหรับตรวจสอบว่าวันที่ที่ส่งมาถูกต้องตามรูปแบบที่กำหนดหรือไม่
        /// </summary>
        /// <param name="strDate">string ของวันที่</param>
        /// <param name="format">รูปแบบที่ต้องการตรวจสอบ</param>
        /// <param name="yearType">ปี ค.ศ หรือ ปี พ.ศ</param>
        /// <returns>true ผ่านการ validate , false ไม่ผ่านการ validate</returns>
        public static bool IsValid(this string strDate, DateFormat format, YearType yearType)
        {
            bool isValid = false;
            DateTime validDate;

            isValid = DateTime.TryParseExact(strDate, GetFormatDate(format), GetCulture(yearType), DateTimeStyles.None, out validDate);
            return isValid;
        }

        /// <summary>
        /// method สำหรับทำการตรวจสอบว่าวันที่ที่ส่งมาถูกต้องตามรูปแบบที่กำหนดหรือไม่
        /// </summary>
        /// <param name="strDate">string ของวันที่</param>
        /// <param name="format">string ที่เป็นรูปแบบที่ต้องการตรวจสอบ</param>
        /// <param name="yearType">ปี ค.ศ หรือ ปี พ.ศ</param>
        /// <returns>true ผ่านการ validate , false ไม่ผ่านการ validate</returns>
        public static bool IsValid(this string strDate, string format, YearType yearType)
        {
            bool isValid = false;
            DateTime validDate;

            isValid = DateTime.TryParseExact(strDate, format, GetCulture(yearType), DateTimeStyles.None, out validDate);
            return isValid;
        }

        private static string GetFormatDate(this DateFormat format)
        {
            return GetFormatDate(format, "/");
        }

        /// <summary>
        /// method สำหรับทำการแปลง format date ที่เป็น enum ให้เป็น string
        /// </summary>
        /// <param name="format">enum format date</param>
        /// <returns>string format date</returns>
        private static string GetFormatDate(DateFormat format, string separator)
        {
            //if (string.IsNullOrEmpty(separator)) separator = "/";
            if (format == DateFormat.ddmmmyy)
                return "dd" + separator + "MMM" + separator + "yy";
            if (format == DateFormat.ddmmyy)
                return "dd" + separator + "MM" + separator + "yy";
            if (format == DateFormat.ddmmyyyy)
                return "dd" + separator + "MM" + separator + "yyyy";
            if (format == DateFormat.yyyymmdd)
                return "yyyy" + separator + "MM" + separator + "dd";
            if (format == DateFormat.ddmmmyyyy)
                return "dd" + separator + "MMM" + separator + "yyyy";
            if (format == DateFormat.ddmmmmyyyy)
                return "dd" + separator + "MMMM" + separator + "yyyy";
            return "";
        }

        /// <summary>
        /// method สำหรับหาค่า culture info จากประเภทของ ปี ที่ส่งมา
        /// </summary>
        /// <param name="yearType">ประเภทของปี</param>
        /// <returns>culture info ที่ตรงกับประเภทของ ปี</returns>
        private static CultureInfo GetCulture(YearType yearType)
        {
            if (yearType == YearType.English)
                return _cultureEnglish;
            return _cultureThai;
        }
    }
}
