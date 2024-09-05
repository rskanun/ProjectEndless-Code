using UnityEngine;

public abstract class BattleHUD : MonoBehaviour
{
    [Header("예상 HP")]
    [SerializeField] private RectTransform hpBarFrame;
    [SerializeField] private RectTransform forecastHP;

    public abstract void UpdateHP(int currentHP, int maxHP);

    public abstract void UpdateSP(int currentSP, int maxSP);

    public abstract void UpdateMP(int currentMP, int maxMP);

    public void SetForecastHP(int hp, int maxHP, int change)
    {
        float width = hpBarFrame.rect.width;
        float left = width / 2;
        float right = width / 2;

        if (change < 0) // 딜에 대한 변화량일 경우
        {
            left = hpBarFrame.rect.width * ((float)(hp + change) / maxHP);
            right = hpBarFrame.rect.width * ((float)(maxHP - hp) / maxHP);
        }
        else if (change > 0) // 힐에 대한 변화량일 경우
        {
            left = hpBarFrame.rect.width * ((float)hp / maxHP);
            right = hpBarFrame.rect.width * ((float)(maxHP - (hp + change)) / maxHP);
        }

        forecastHP.offsetMin = new Vector2(Mathf.Max(left, 0.0f), forecastHP.offsetMin.y);
        forecastHP.offsetMax = new Vector2(-Mathf.Max(right, 0.0f), forecastHP.offsetMax.y);
    }
}