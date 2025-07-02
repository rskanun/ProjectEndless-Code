using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ContactApp : App
{
    private enum ContactState
    {
        Party,
        Weapon,
        Skill
    }

    [SerializeField] private ContactWindow mainWindow;
    [SerializeField] private ContactWindow weaponWindow;
    [SerializeField] private ContactWindow skillWindow;

    private ContactState state;
    private ContactWindow currentWindow;

#if UNITY_EDITOR
    private void OnValidate()
    {
        ui = GetComponent<ContactUI>();
    }
#endif

    protected override void OnOpened()
    {
        state = ContactState.Party;
        currentWindow = mainWindow;

        // 파티 맴버 화면부터 열기
        mainWindow.OpenWindow();
    }

    /// <summary>
    /// 현재 창에 띄워진 목록을 무기 목록으로 바꾸기
    /// </summary>
    public void ShowWeapons()
    {
        // 현재 활성화된 목록이 무기 목록일 경우 넘어가기
        if (state == ContactState.Weapon) return;

        // 이전 화면 비활성화
        currentWindow.CloseWindow();

        // 무기 목록 화면 활성화
        weaponWindow.gameObject.SetActive(true);
        weaponWindow.OpenWindow();
    }

}