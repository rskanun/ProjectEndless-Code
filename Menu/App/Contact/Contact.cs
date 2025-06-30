using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Contact : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject partyMark;
    [SerializeField] private AmountHUD hpHud;
    [SerializeField] private AmountHUD spHud;
    [SerializeField] private GameObject selectMark;

    private Action selectHandler;

    private void OnEnable()
    {
        // 해당 오브젝트가 선택된 경우
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // 핸들러 작동
            SelectHandler();
        }
    }

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

    public void SetSelectAction(Action handler)
    {
        selectHandler = handler;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectHandler();
    }

    private void SelectHandler()
    {
        selectHandler?.Invoke();
        selectMark.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectMark.SetActive(false);
    }
}