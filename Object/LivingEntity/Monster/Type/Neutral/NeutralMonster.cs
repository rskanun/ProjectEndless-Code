public abstract class NeutralMonster : Monster
{
    protected override Propensity CreatePropensity()
    {
        return new Neutral(this);
    }
}