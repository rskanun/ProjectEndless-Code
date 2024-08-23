using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Buff", fileName = "Buff")]
public class Buff : StatusEffect
{
    public override bool IsBuff => true;
}