public class Player : Character
{
    public override void OnAttackTargeted(Entity attacker, bool isUsedParry, bool isUsedDodge)
    {
        battleData.IsUsedParry = isUsedParry;
        battleData.IsUsedDodge = isUsedDodge;
    }
}