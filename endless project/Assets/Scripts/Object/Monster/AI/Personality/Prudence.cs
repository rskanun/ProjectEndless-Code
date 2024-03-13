public class Prudence : Personality
{
    public Prudence(Monster monster) : base(monster) { }

    public override IMonsterState OnDetectedPlayer()
    {
        throw new System.NotImplementedException();
    }
}