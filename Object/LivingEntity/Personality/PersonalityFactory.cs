public class PersonalityFactory
{
    private static PersonalityFactory _instance;
    public static PersonalityFactory Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PersonalityFactory();

            return _instance;
        }
    }

    public Personality CreatePersonality(PersonalityType type)
    {
        switch (type)
        {
            case PersonalityType.Belligerent: return new Belligerent();
            case PersonalityType.Cautious: return new Cautious();
            case PersonalityType.Nervous: return new Crusty();
            case PersonalityType.Brave: return new Brave();
            case PersonalityType.Analytical: return new Analytical();
        }

        return null;
    }
}