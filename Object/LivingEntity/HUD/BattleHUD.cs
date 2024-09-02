using UnityEngine;

public abstract class BattleHUD : MonoBehaviour
{
    public abstract void UpdateHP(int currentHP, int maxHP);

    public abstract void UpdateSP(int currentSP, int maxSP);

    public abstract void UpdateMP(int currentMP, int maxMP);
}