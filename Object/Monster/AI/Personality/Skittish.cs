public class Skittish : Personality
{
    public Skittish(Monster monster) : base(monster) { }

    public override IMonsterState OnDetectedPlayer()
    {
        return new RunState(monster);
    }
}