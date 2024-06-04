using UnityEngine;

public class HudManager : MonoBehaviour
{
    [Header("플레이어 스텟 데이터")]
    [SerializeField] private PlayerData stat;

    [Header("참조 스크립트")]
    [SerializeField] private HealthPointBarUI hpBarUI;
    [SerializeField] private AwakenPointBarUI apBarUI;

    // 수치 변환 전 스텟
    private int initHP;

    private void Start()
    {
        initHP = stat.HP;

        hpBarUI.SetHpBar(stat.HP, stat.MaxHP);
        apBarUI.SetApBar(stat.AP, stat.MaxAP);
    }

    public void HpUpdate()
    {
        int currentHP = stat.HP;

        hpBarUI.BarUpdate(initHP, currentHP);
    }

    public void ApUpdate()
    {
        int currentAP = stat.AP;

        apBarUI.BarUpdate(currentAP);
    }
}