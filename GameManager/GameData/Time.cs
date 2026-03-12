using Sirenix.OdinInspector;

namespace Endless.GameData
{
    [System.Serializable]
    public class Time
    {
        [ShowInInspector]
        public int Hour
        {
            get => _hour;
            set => _hour = ((value % 24) + 24) % 24;
        }
        private int _hour;

        [ShowInInspector]
        public int Min
        {
            get => _minute;
            set => _minute = ((value % 60) + 60) % 60;
        }
        private int _minute;

        [ShowInInspector]
        public int Sec
        {
            get => _seconds;
            set => _seconds = ((value % 60) + 60) % 60;
        }
        private int _seconds;

        public int TotalSeconds
            => Hour * 3600 + Min * 60 + Sec;

        public Time(int hour, int min, int sec)
        {
            Hour = hour;
            Min = min;
            Sec = sec;
        }

        public Time(int seconds)
        {
            Hour = seconds / 3600;
            Min = seconds / 60 % 60;
            Sec = seconds % 60;
        }

        public void Subtract(int seconds)
        {
            int secondsInDay = 24 * 3600;
            int total = TotalSeconds;

            total -= seconds;

            // 하루 단위로 시간 래핑
            total = ((total % secondsInDay) + secondsInDay) % secondsInDay;

            // 다시 시, 분, 초로 환산
            Hour = total / 3600;
            Min = total / 60 % 60;
            Sec = total % 60;
        }

        public override string ToString()
        {
            return $"{Hour:d2}:{Min:d2}:{Sec:d2}";
        }

        public static Time operator +(Time a, Time b)
        {
            return new Time(a.TotalSeconds + b.TotalSeconds);
        }

        public static Time operator -(Time a, Time b)
        {
            return new Time(a.TotalSeconds - b.TotalSeconds);
        }

        public static Time operator /(Time a, Time b)
        {
            return new Time(a.TotalSeconds / b.TotalSeconds);
        }
    }
}