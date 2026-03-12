using System;
using UnityEngine;

[Serializable]
public class RemainTime
{
    [ReadOnly]
    [SerializeField]
    private int remainCount;
    public int RemainCount
    {
        get { return remainCount; }
    }
    [ReadOnly]
    [SerializeField]
    private int hour;
    public int Hour { get { return hour; } }
    [ReadOnly]
    [SerializeField]
    private int minute;
    public int Minute { get { return minute; } }
    [ReadOnly]
    [SerializeField]
    private int second;
    public int Second { get { return second; } }

    public bool IsNull
    {
        get { return remainCount <= 0; }
    }

    public RemainTime(int hour, int min, int sec)
    {
        SetTime(hour, min, sec);
    }

    public RemainTime(int seconds)
    {
        SetRemainTime(seconds);
    }

    private void SetTime(int hour, int minute, int second)
    {
        this.hour = hour;
        this.minute = minute;
        this.second = second;

        remainCount = hour * 3600 + minute * 60 + second;
    }

    private void SetRemainTime(int time)
    {
        remainCount = time;

        hour = remainCount / 60 / 60;
        minute = remainCount / 60 % 60;
        second = remainCount % 60;
    }

    public void ConsumeTime()
    {
        if (remainCount <= 0) SetTime(23, 59, 59);
        else SetRemainTime(--remainCount);

        PlayerPrefs.SetInt("remainTime", remainCount);
    }
}
