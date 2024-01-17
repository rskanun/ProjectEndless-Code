using TMPro;
using UnityEngine;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI manaText;

    private int maxHP;
    private int maxMana;

    public void InitHp(int maxHP, int currentHP)
    {
        this.maxHP = maxHP;

        UpdateHp(currentHP);
    }

    public void UpdateHp(int currentHP)
    {
        hpText.text = "체력: " + maxHP + " / " + currentHP;
    }

    public void InitMana(int maxMana, int currentMana)
    {
        this.maxMana = maxMana;

        UpdateMana(currentMana);
    }

    public void UpdateMana(int currentMana)
    {
        manaText.text = "마나: " + maxMana + " / " + currentMana;
    }
}