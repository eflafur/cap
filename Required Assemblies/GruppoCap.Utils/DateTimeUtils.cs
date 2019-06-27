using System;

namespace GruppoCap
{
    public static class DateTimeUtils
    {

        // CONSTs
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public const Int64 OneMinuteInSeconds = 60;
        public const Int64 OneHourInSeconds = 3600;
        public const Int64 OneDayInSeconds = 86400;
        public static readonly DateTime SqlServerDateTimeMinValue = new DateTime(1753, 1, 1);
        public static readonly DateTime SqlServerDateTimeMaxValue = new DateTime(9999, 12, 31, 11, 59, 00);
        public static readonly DateTime OracleDateTimeMinValue = new DateTime(1900, 1, 1);
        public static readonly DateTime OracleDateTimeMaxValue = new DateTime(9999, 12, 31, 11, 59, 00);

        // FROM UNIX EPOCH DATE
        public static DateTime FromUnixEpochDate(Double unixEpochDate)
        {
            return (UnixEpoch.AddSeconds(unixEpochDate));
        }

        // TO EUROPEAN STRING
        public static String ToEuropeanString(this DateTime date, Boolean includeTime = false, Boolean ensureUTC = false)
        {
            DateTime d;
            d = date;

            if (date == DateTime.MinValue)
                return String.Empty;

            if (ensureUTC)
                d = d.ToUniversalTime();

            if (includeTime)
                return d.ToString("dd/MM/yyyy HH:mm");
            else
                return d.ToString("dd/MM/yyyy");
        }

        // TO EUROPEAN STRING
        public static String ToEuropeanString(this DateTime? date, Boolean includeTime = false, Boolean ensureUTC = false)
        {
            if (date.HasValue == false)
                return String.Empty;

            return date.Value.ToEuropeanString(includeTime: includeTime, ensureUTC: ensureUTC);
        }

        // TO STRING RFC 822
        public static String ToStringRFC822(this DateTime date)
        {
            return date.ToString("r");
        }

        // TO RFC 3339 - UTC
        public static String ToStringRFC3339_UTC(this DateTime date, Boolean includeTime = true, Boolean ensureUTC = false)
        {
            DateTime d;

            d = date;

            if (ensureUTC)
                d = d.ToUniversalTime();

            if (includeTime)
                return d.ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                return d.ToString("yyyy-MM-dd");
        }

        // PARSE YYYYMMDDTHHMMSSZ
        public static DateTime ParseYYYYMMDDTHHMMSSZ(String s, DateTime defaultValue)
        {
            try
            {
                return DateTime.ParseExact(s, "yyyyMMddTHHmmssZ", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        // TO ISO DATE STRING
        public static String ToISODateString(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        // TO ISO DATE STRING
        public static String ToISODateTimeString(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmss");
        }

        // TO ISO DATE NUMERIC
        public static Int32 ToISODateNumeric(this DateTime date)
        {
            return Int32.Parse(date.ToISODateString());
        }

        // FROM EUROPEAN DATE STRING
        public static DateTime? FromEuropeanDateString(this String s)
        {
            if (s.IsNullOrWhiteSpace())
                return null;

            return s.CoerceToOrDefault<DateTime>();
        }

        // FROM ISO DATE STRING
        public static DateTime FromISODateString(String date)
        {
            return DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
        }

        // FROM ISO DATE NUMERIC
        public static DateTime FromISODateNumeric(Int32 date)
        {
            return FromISODateString(date.ToString());
        }

        // SECONDs
        public static TimeSpan Seconds(this Int32 v)
        {
            return TimeSpan.FromSeconds(v);
        }

        // MINUTEs
        public static TimeSpan Minutes(this Int32 v)
        {
            return TimeSpan.FromMinutes(v);
        }

        // HOURs
        public static TimeSpan Hours(this Int32 v)
        {
            return TimeSpan.FromHours(v);
        }

        // AGO
        public static DateTimeOffset Ago(this TimeSpan ts)
        {
            return DateTimeOffset.Now.Add(-ts);
        }

        // AGO UTC
        public static DateTimeOffset AgoUTC(this TimeSpan ts)
        {
            return DateTimeOffset.UtcNow.Add(-ts);
        }

        // FROM NOW
        public static DateTimeOffset FromNow(this TimeSpan ts)
        {
            return DateTimeOffset.Now.Add(ts);
        }

        // FROM NOW UTC
        public static DateTimeOffset FromNowUTC(this TimeSpan ts)
        {
            return DateTimeOffset.UtcNow.Add(ts);
        }

        // GET MILLISECONDS SINCE 1 JAN 1970 AT MIDNIGHT
        //public static Double GetMillisecondsSince1Jan1970AtMidnight(DateTime moment)
        //{
        //    return new TimeSpan(moment.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks).TotalMilliseconds;
        //}
    }
}
