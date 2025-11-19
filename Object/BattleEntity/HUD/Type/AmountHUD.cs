using UnityEngine;

public abstract class AmountHUD : MonoBehaviour
{
    public abstract void UpdateAmount(int curAmount, int maxAmount);
}