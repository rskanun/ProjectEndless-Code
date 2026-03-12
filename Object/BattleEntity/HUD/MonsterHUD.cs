using UnityEngine;

public class MonsterHUD : BattleHUD
{
    [Header("사용 HUD")]
    [SerializeField] private AmountHUD hpBar;
    [SerializeField] private AmountHUD mpBar;

    public override void UpdateHP(int hp, int maxHP)
    {
        hpBar.UpdateAmount(hp, maxHP);
    }

    public override void UpdateMP(int mp, int maxMP)
    {
        mpBar.UpdateAmount(mp, maxMP);
    }

    public override void UpdateSP(int sp, int maxSP)
    {
        // SP 업데이트 X
    }
}