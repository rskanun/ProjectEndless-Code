public abstract class HostileMonster : Monster
{
    protected override Propensity CreatePropensity()
    {
        return new Hostile();
    }
}