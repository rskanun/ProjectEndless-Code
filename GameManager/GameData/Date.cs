using System;
using UnityEngine;

namespace Endless.GameData
{
    [Serializable]
    public class Date
    {
        [SerializeField] private int year = 2048;
        [SerializeField] private int month;
        [SerializeField] private int day;

        public DateTime date
        {
            get { return new DateTime(year, month, day); }
            set
            {
                month = value.Month;
                day = value.Day;
            }
        }

        public Date(int month, int day)
        {
            SetDate(month, day);
        }

        public Date Clone()
        {
            return new Date(month, day);
        }

        public void SetDate(int month, int day)
        {
            this.month = month;
            this.day = day;
        }

        public bool IsPastDate(DateTime date)
        {
            return date < this.date;
        }

        public override string ToString()
        {
            return date.ToString("O");
        }

        public override bool Equals(object obj)
        {
            if (obj is Date)
            {
                Date otherDate = (Date)obj;

                return date.Equals(otherDate.date);
            }

            return false;
        }

        public static Date StrToDate(string date)
        {
            DateTime dateTime = DateTime.Parse(date);

            return new Date(dateTime.Month, dateTime.Day);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Date d1, Date d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Date d1, Date d2)
        {
            return !(d1 == d2);
        }

        public static bool operator <(Date d1, Date d2)
        {
            return d1.date < d2.date;
        }

        public static bool operator >(Date d1, Date d2)
        {
            return d1.date > d2.date;
        }

        public static bool operator <=(Date d1, Date d2)
        {
            return d1.date <= d2.date;
        }

        public static bool operator >=(Date d1, Date d2)
        {
            return d1.date >= d2.date;
        }
    }
}
