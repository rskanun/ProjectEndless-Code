public enum PersonalityType
{
    Belligerent, // 호전적인
    Cautious, // 신중한
    Nervous, // 신경질적인
    Brave, // 용감한
    Analytical, // 분석적인
}

public abstract class Personality
{
    public string NameTitle { protected set; get; }
    public abstract Entity SelectTarget();
}