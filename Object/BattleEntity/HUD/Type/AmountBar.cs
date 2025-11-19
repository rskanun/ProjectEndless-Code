using UnityEngine.UI;

public class AmountBar : AmountHUD
{
    public Image bar;

    public override void UpdateAmount(int curAmount, int maxAmount)
    {
        bar.fillAmount = (float)curAmount / maxAmount;
    }
}