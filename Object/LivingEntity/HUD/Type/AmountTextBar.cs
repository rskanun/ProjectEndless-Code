using TMPro;
using UnityEngine.UI;

public class AmountTextBar : AmountHUD
{
    public Image amountBar;
    public TextMeshProUGUI amountText;
    public bool isViewMax;

    public override void UpdateAmount(int curAmount, int maxAmount)
    {
        UpdateBar(curAmount, maxAmount);
        UpdateText(curAmount, maxAmount);
    }

    protected virtual void UpdateBar(int amount, int maxAmount)
    {
        amountBar.fillAmount = (float)amount / maxAmount;
    }

    protected virtual void UpdateText(int amount, int maxAmount)
    {
        if (isViewMax) amountText.text = $"{amount} / {maxAmount}";
        else amountText.text = amount.ToString();
    }
}