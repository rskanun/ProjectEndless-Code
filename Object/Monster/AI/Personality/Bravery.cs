public class Bravery : Personality
{
    public Bravery(Monster monster) : base(monster) { }

    public override IMonsterState OnDetectedPlayer()
    {
        return new ChaseState(monster);
    }
}