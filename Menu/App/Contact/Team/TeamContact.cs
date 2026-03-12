using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamContact : Contact, ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject partyMark;
    [SerializeField] private AmountHUD hpHud;
    [SerializeField] private AmountHUD spHud;
    [SerializeField] private Image selectMark;

    private Action submitHandler;
    private CharacterData _character;
    public CharacterData Character => _character;

    private void OnDisable()
    {
        // 선택이 해제되지 않고 비활성화 될 경우 대비
        DeselectHandler();
    }

    public void UpdateInfo(CharacterData character)
    {
        _character = character;

        nameField.text = character.Name;
        partyMark.SetActive(character.IsParty);
        hpHud.UpdateAmount(character.Stats.HP, character.Stats.MaxHP);
        spHud.UpdateAmount(character.Stats.SP, character.Stats.MaxSP);
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

    protected override void SelectHandler()
    {
        base.SelectHandler();

        // 선택 애니메이션
        selectMark.gameObject.SetActive(true);
        selectMark.DOFade(0.25f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        DeselectHandler();
    }

    private void DeselectHandler()
    {
        // 비활성화 전 애니메이션 삭제
        selectMark.DOKill();

        selectMark.color = new Color(selectMark.color.r, selectMark.color.g, selectMark.color.b, 0f);
        selectMark.gameObject.SetActive(false);
    }
}