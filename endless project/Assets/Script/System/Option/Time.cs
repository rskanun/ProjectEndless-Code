using System.Threading;

namespace Assets.Script.System.Option
{
    public class Time
    {
        private int time;
        private const int MAX_TIME = 86400;

        private int hour;
        public int Hour { get { return hour; } }

        private int minute;
        public int Minute { get { return minute; } }

        private int second;
        public int Second { get { return second; } }

        public Time(int time) 
        {
            this.time = time;

            hour = time / 60 / 60;
            minute = time / 60 % 60;
            second = time % 60;
        }

        public static Time operator --(Time t)
        {
            if (t.time <= 0) return new Time(t.time = MAX_TIME);
            return new Time(t.time = t.time - 1);
        }
    }
}