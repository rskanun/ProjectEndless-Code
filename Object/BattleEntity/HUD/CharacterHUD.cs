using UnityEngine;

public class CharacterHUD : BattleHUD
{
    [Header("사용 HUD")]
    [SerializeField] private AmountHUD hpBar;
    [SerializeField] private AmountHUD spBar;

    public override void UpdateHP(int hp, int maxHP)
    {
        hpBar.UpdateAmount(hp, maxHP);
    }

    public override void UpdateSP(int sp, int maxSP)
    {
        spBar.UpdateAmount(sp, maxSP);
    }

    public override void UpdateMP(int mp, int maxMP)
    {
        // 잠시 빈 칸
    }
}