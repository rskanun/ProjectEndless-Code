using System.Threading;

namespace Assets.Script.System.Option
{
    public class Time
    {
        private int time;

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
            return new Time(t.time - 1);
        }
    }
}