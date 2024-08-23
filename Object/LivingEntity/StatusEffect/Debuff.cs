using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Debuff", fileName = "Debuff")]
public class Debuff : StatusEffect
{
    public override bool IsBuff => false;
}