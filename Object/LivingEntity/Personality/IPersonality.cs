using System;
using System.Collections.Generic;

public enum PersonalityType
{
    Belligerent, // 호전적인
    Cautious, // 신중한
    Crusty, // 신경질적인
    Brave, // 용감한
    Analytical, // 분석적인
}

public interface IPersonality
{
    public List<Entity> GetPriorityTargetList();

    public static IPersonality OfType(PersonalityType type)
    {
        switch (type)
        {
            case PersonalityType.Belligerent: return new Belligerent();
            case PersonalityType.Cautious: return new Cautious();
            case PersonalityType.Crusty: return new Crusty();
            case PersonalityType.Brave: return new Brave();
            case PersonalityType.Analytical: return new Analytical();
        }

        return null;
    }
}