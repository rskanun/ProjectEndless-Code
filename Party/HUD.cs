using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("구성 오브젝트")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI spText;
    public Image hpAmount;
    public Image spAmount;

    public void InitData(CharacterData chrData)
    {
        // Set Name
        nameText.text = chrData.Name;

        // Init HP & SP Bar
        UpdateHP(chrData.Stats.HP, chrData.Stats.MaxHP);
        UpdateSP(chrData.Stats.SP, chrData.Stats.MaxSP);
    }

    public void UpdateHP(int hp, int maxHP)
    {
        hpText.text = hp.ToString();
        hpAmount.fillAmount = (float)hp / maxHP;
    }

    public void UpdateSP(int sp, int maxSP)
    {
        spText.text = sp.ToString();
        spAmount.fillAmount = (float)sp / maxSP;
    }
}