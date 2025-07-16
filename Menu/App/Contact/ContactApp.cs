using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContactState
{
    Party,
    Diary,
    Weapon,
    OffWeapon,
    Accessory,
    Skill
}

public class ContactApp : App
{
    [SerializeField] private Diary diary;
    [SerializeField] private TeamContactWindow mainWindow;
    [SerializeField] private WeaponContactWindow weaponWindow;
    [SerializeField] private ContactWindow accessoryWindow;
    [SerializeField] private ContactWindow skillWindow;

    private Dictionary<ContactState, ContactWindow> windows;

    private ContactWindow currentWindow;
    private ContactState _state;
    public ContactState State => _state;
    public CharacterData SelectCharacter { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ui = GetComponent<ContactUI>();
    }
#endif

    private void Awake()
    {
        windows = new Dictionary<ContactState, ContactWindow>()
        {
            { ContactState.Party, mainWindow },
            { ContactState.Weapon, weaponWindow },
            { ContactState.Accessory, accessoryWindow },
            { ContactState.Skill, skillWindow }
        };
    }

    protected override void OnOpen()
    {
        // 파티 맴버 화면부터 열기
        ShowContact(ContactState.Party);
    }

    /// <summary>
    /// 현재 창에 띄워진 목록을 무기 목록으로 바꾸기
    /// </summary>
    public void ShowWeapons()
    {
        // 애니메이션이 실행 중이라면 창 변경 중지
        if (currentWindow?.IsTweening == true) return;

        ShowContact(ContactState.Weapon);
    }

    private void ShowContact(ContactState state)
    {
        // 이전 화면이 비활성화 되고 난 후에 바꿀 화면 활성화
        StartCoroutine(SwapWindow(state));
    }

    private IEnumerator SwapWindow(ContactState state)
    {
        // 현재 활성화된 목록과 동일한 경우 넘어가기
        if (currentWindow != null && this._state == state) yield break;

        this._state = state;

        // 이전 화면 비활성화
        currentWindow?.CloseWindow();

        // 이전 화면이 비활성화 될 때까지 대기
        if (currentWindow != null) yield return new WaitUntil(() => !currentWindow.IsTweening);

        // 새 목록 화면 활성화
        windows[state].gameObject.SetActive(true);
        windows[state].OpenWindow();

        // 현재 상태 업데이트
        currentWindow = windows[state];
    }

    public override void Close(bool isPlayAnimation)
    {
        // 현재 열린 창에서 애니메이션이 실행 중이면 무시
        if (currentWindow?.IsTweening == true) return;

        // 첫 화면이 아닌 경우
        if (_state != ContactState.Party)
        {
            // 다이어리라면 파티 메뉴 내의 버튼 선택으로 넘어가기
            if (_state == ContactState.Diary) FocusContactMenu();
            else
            {
                // 그 외엔 메인 파티 메뉴로 돌아가서 다이어리 내 버튼 선택으로 넘어가기
                ShowContact(ContactState.Party);
                FocusDiary();
            }
        }
        else
        {
            // 현재 창 정보 초기화
            currentWindow = null;

            // 선택 캐릭터 정보 초기화
            SelectCharacter = null;

            // 처음 창인 경우 앱 종료
            base.Close(isPlayAnimation);
        }
    }

    public void OnSelectCharacter(CharacterData character)
    {
        // 현재 선택된 캐릭터 정보 업데이트
        SelectCharacter = character;

        // 다이어리 정보 업데이트
        diary.UpdateDiary(character);
    }

    /************************************************************
    * [다이어리]
    * 
    * 캐릭터의 정보를 보이는 다이어리 관리
    ************************************************************/

    /// <summary>
    /// 다이어리 내의 버튼 선택으로 넘어가기
    /// </summary>
    public void FocusDiary()
    {
        // 다이어리 내 버튼 선택
        diary.SelectButton(_state);

        // 현재 상태 변경
        _state = ContactState.Diary;
    }

    /// <summary>
    /// 메뉴 내의 버튼 선택으로 넘아가기
    /// </summary>
    public void FocusContactMenu()
    {
        // 현재 상태 변경
        _state = ContactState.Party;

        // 선택된 캐릭터 선택으로 넘어가기
        mainWindow.SelectLastSelectedContact();
    }
}