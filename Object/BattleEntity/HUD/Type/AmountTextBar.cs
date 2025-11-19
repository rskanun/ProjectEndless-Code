using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmountTextBar : AmountHUD
{
    private enum BarType
    {
        Amount,
        AmountMax,
        Percentage
    }

    public Image amountBar;
    public TextMeshProUGUI amountText;
    [SerializeField] private BarType type;

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
        string text = "";

        if (type == BarType.AmountMax) text = $"{amount} / {maxAmount}";
        else if (type == BarType.Amount) text = amount.ToString();
        else if (type == BarType.Percentage) text = $"{(int)(amount / (float)maxAmount * 100)}%";

        amountText.text = text;
    }
}