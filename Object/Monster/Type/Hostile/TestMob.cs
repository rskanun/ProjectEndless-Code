public class TestMob : HostileMonster
{
    public override void OnAttack()
    {
        
    }

    protected override Personality CreatePersonality()
    {
        return new Prudence(this);
    }
}