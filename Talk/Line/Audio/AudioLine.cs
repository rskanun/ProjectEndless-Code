using System;

public enum AudioState
{
    Play,
    Stop,
}

[Serializable]
public abstract class AudioLine : Line
{
    public string FileName { get; private set; }
    public AudioState State { get; private set; }
    public AudioLine(LineType type, AudioState state, string fileName) : base(type)
    {
        // BGM, SE만 받도록 유효성 검사
        if (type != LineType.BGM && type != LineType.SE)
        {
            // 허용되지 않은 타입의 경우, ArgumentException 발생
            throw new ArgumentException($"AudioLine에 적합한 타입이 아닙니다! \r\n(입력된 타입: {type})", nameof(type));
        }

        FileName = fileName;
        State = state;
    }
}

[Serializable]
public class BgmLine : AudioLine
{
    public BgmLine(AudioState state, string fileName) : base(LineType.BGM, state, fileName)
    {

    }
}

[Serializable]
public class SeLine : AudioLine
{
    public SeLine(string fileName) : base(LineType.SE, AudioState.Play, fileName)
    {

    }
}