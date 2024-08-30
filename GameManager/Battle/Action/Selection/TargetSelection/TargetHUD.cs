using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetHUD : MonoBehaviour
{
    public GameObject bar;
    public GameObject reduceBar;

    private Image amount;
    private Image reduceAmount;
    private RectTransform reduceAmountRec;
    private float barWidth;

    private void Awake()
    {
        amount = bar.GetComponent<Image>();
        reduceAmount = reduceBar.GetComponent<Image>();
        reduceAmountRec = reduceBar.GetComponent<RectTransform>();

        barWidth = bar.GetComponent<RectTransform>().rect.width;
    }

    private void Start()
    {
        // 깜빡이는 주기
        float duration = 1.0f;

        DOTween.Sequence()
            .Append(reduceAmount.DOFade(0f, duration))
            .Append(reduceAmount.DOFade(1f, duration))
            .SetLoops(-1);
    }

    public void SetActiveReduceAmount(bool isActive)
    {
        reduceBar.SetActive(isActive);
    }

    public void UpdateAmount(int value, int subValue, int maxValue)
    {
        // 현재 양 설정
        amount.fillAmount = (float)value / maxValue;

        // 깎일 양 설정
        float right = barWidth / amount.fillAmount; // 현재 깎여진 부분
        float left = barWidth / ((float)(value - subValue) / maxValue); // 깎여 나갈 부분

        SetAmountRecRight(right);
        SetAmountRecLeft(left);
    }

    private void SetAmountRecLeft(float left)
    {
        reduceAmountRec.offsetMin = new Vector2(left, reduceAmountRec.offsetMin.y);
    }

    private void SetAmountRecRight(float right)
    {
        reduceAmountRec.offsetMax = new Vector2(right, reduceAmountRec.offsetMax.y);
    }
}