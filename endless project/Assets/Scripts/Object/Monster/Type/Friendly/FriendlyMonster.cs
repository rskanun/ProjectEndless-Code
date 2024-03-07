public abstract class FriendlyMonster : Monster
{
    protected override Propensity CreatePropensity()
    {
        return new Friendly(this);
    }

    public abstract void ProvideEffect();
}