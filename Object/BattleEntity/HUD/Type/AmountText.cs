using TMPro;

public class AmountText : AmountHUD
{
    public TextMeshProUGUI amountText;

    public override void UpdateAmount(int curAmount, int maxAmount)
    {
        amountText.text = curAmount.ToString();
    }
}