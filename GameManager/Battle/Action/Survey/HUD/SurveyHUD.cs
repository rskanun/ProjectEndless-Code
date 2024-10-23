using UnityEngine;

public class SurveyHUD : MonoBehaviour
{
    [Header("예상 HP")]
    [SerializeField] private RectTransform hpBarFrame;
    [SerializeField] private RectTransform forecastHP;

    public void SetHpBarActive(bool isActive)
    {
        forecastHP.gameObject.SetActive(isActive);
    }

    public bool IsHpBarActive()
    {
        return forecastHP.gameObject.activeSelf;
    }

    public void SetForecastHP(int hp, int maxHP, int change)
    {
        if (IsHpBarActive() == false)
        {
            // 예상 HP 바가 비활성화 상태일 경우 활성화 상태로 바꾸기
            SetHpBarActive(true);
        }

        if (change < 0) SetReducedBar(hp, maxHP, change); // HP가 증가될 경우
        else if (change > 0) SetIncreasedBar(hp, maxHP, change); // HP가 감소될 경우
    }

    private void SetReducedBar(int hp, int maxHP, int change)
    {
        float left = hpBarFrame.rect.width * ((float)(hp + change) / maxHP);
        float right = hpBarFrame.rect.width * ((float)(maxHP - hp) / maxHP);

        // 예상 HP 바 설정
        SetForecastHpBar(left, right);
    }

    private void SetIncreasedBar(int hp, int maxHP, int change)
    {
        float left = hpBarFrame.rect.width * ((float)hp / maxHP);
        float right = hpBarFrame.rect.width * ((float)(maxHP - (hp + change)) / maxHP);

        // 예상 HP 바 설정
        SetForecastHpBar(left, right);
    }

    private void SetForecastHpBar(float left, float right)
    {
        forecastHP.offsetMin = new Vector2(Mathf.Max(left, 0.0f), forecastHP.offsetMin.y);
        forecastHP.offsetMax = new Vector2(-Mathf.Max(right, 0.0f), forecastHP.offsetMax.y);
    }
}