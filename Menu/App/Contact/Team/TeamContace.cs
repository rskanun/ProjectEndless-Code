using TMPro;
using UnityEngine;

public class TeamContact : Contact
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject partyMark;
    [SerializeField] private AmountHUD hpHud;
    [SerializeField] private AmountHUD spHud;

    public void UpdateInfo(CharacterData character)
    {
        nameField.text = character.Name;
        partyMark.SetActive(character.IsParty);
        hpHud.UpdateAmount(character.Stat.HP, character.Stat.MaxHP);
        spHud.UpdateAmount(character.Stat.SP, character.Stat.MaxSP);
    }

    public void SetPartyStatus(bool isInParty)
    {
        partyMark.SetActive(isInParty);
    }
}