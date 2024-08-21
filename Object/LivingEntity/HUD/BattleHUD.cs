using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [Header("체력바 요소")]
    public Image hpBar;
    public TextMeshProUGUI hpAmount;

    [Header("마력바 요소")]
    public Image mpBar;

    public void UpdateHP(int currentHP, int maxHP)
    {
        // 바 업데이트
        hpBar.fillAmount = (float)currentHP / maxHP;

        // 텍스트 업데이트
        hpAmount.text = $"{currentHP} / {maxHP}";
    }

    public void UpdateMP(int currentMP, int maxMP)
    {
        // 바 업데이트
        mpBar.fillAmount = (float)currentMP / maxMP;
    }
}