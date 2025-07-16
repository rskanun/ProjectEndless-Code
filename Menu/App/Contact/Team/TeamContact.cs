using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamContact : Contact, ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject partyMark;
    [SerializeField] private AmountHUD hpHud;
    [SerializeField] private AmountHUD spHud;

    private Action submitHandler;

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

    public void SetSubmitHandler(Action handler)
    {
        submitHandler = handler;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        submitHandler?.Invoke();
    }
}