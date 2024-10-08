public enum BuffType
{
    AttackBuff,
    DefenseBuff
}

[System.Serializable]
public abstract class Buff : StatusEffect
{
    public override bool IsBuff => true;
    public abstract BuffType Type { get; }
}