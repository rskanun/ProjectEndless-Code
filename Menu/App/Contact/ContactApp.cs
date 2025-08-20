using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ContactState
{
    Party,
    Diary,
    Weapon,
    OffWeapon,
    Accessory1,
    Accessory2,
    Skill
}

public class ContactApp : App
{
    [SerializeField] private Diary diary;
    [SerializeField] private TeamContactWindow mainWindow;
    [SerializeField] private WeaponContactWindow weaponWindow;
    [SerializeField] private OffWeaponContactWindow offWeaponWindow;
    [SerializeField] private AccessoryContactWindow accessoryWindow;
    [SerializeField] private SkillInformationWindow skillWindow;

    private Dictionary<ContactState, ContactWindow> windows;

    private ContactWindow currentWindow;
    private ContactState _state;
    public ContactState State => _state;
    public CharacterData SelectCharacter { get; private set; }

    private void OnValidate()
    {
        ui = GetComponent<ContactUI>();
    }

    private void Awake()
    {
        windows = new Dictionary<ContactState, ContactWindow>()
        {
            { ContactState.Party, mainWindow },
            { ContactState.Weapon, weaponWindow },
            { ContactState.OffWeapon, offWeaponWindow },
            { ContactState.Accessory1, accessoryWindow },
            { ContactState.Accessory2, accessoryWindow },
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
        ShowContact(ContactState.Weapon);
    }

    /// <summary>
    /// 현재 창에 띄워진 목록을 보조 무기 목록으로 바꾸기
    /// </summary>
    public void ShowOffWeapons()
    {
        ShowContact(ContactState.OffWeapon);
    }

    /// <summary>
    /// 현재 창에 띄워진 목록을 1번 슬롯의 악세사리 목록으로 바꾸기
    /// </summary>
    public void ShowSlot1Accessory()
    {
        ShowContact(ContactState.Accessory1);
    }

    /// <summary>
    /// 현재 창에 띄워진 목록을 2번 슬롯의 악세사리 목록으로 바꾸기
    /// </summary>
    public void ShowSlot2Accessory()
    {
        ShowContact(ContactState.Accessory2);
    }

    /// <summary>
    /// 현재 창에서 스킬 정보를 보여주는 창으로 바꾸기
    /// </summary>
    public void ShowSkillInformation(Skill skill)
    {
        _state = ContactState.Skill;

        // 맴버 정보 화면 숨기기
        mainWindow.HideWindow();

        // 스킬 정보 불러오기
        skillWindow.OpenWindow(skill);
    }

    /// <summary>
    /// 스킬 정보가 띄워진 상태에서 다른 스킬 정보로 넘어가기
    /// </summary>
    /// <param name="skill"></param>
    public void SwapSkillInformation(Skill skill, bool isReverseMove)
    {
        skillWindow.SwapInfo(skill, isReverseMove);
    }

    public void HideSkillInformation()
    {
        diary.IsFocusToSkill = false;

        // 스킬 정보 숨기기
        skillWindow.CloseWindow();

        // 맴버 정보 화면 되돌리기
        mainWindow.ShowWindow();
    }

    private void ShowContact(ContactState state)
    {
        // 애니메이션이 실행 중이라면 창 변경 중지
        if (currentWindow?.IsTweening == true) return;

        // 이전 화면이 비활성화 되고 난 후에 바꿀 화면 활성화
        StartCoroutine(SwapWindow(state));
    }

    private IEnumerator SwapWindow(ContactState state)
    {
        // 현재 활성화된 목록과 동일한 경우 넘어가기
        if (currentWindow != null && _state == state) yield break;

        _state = state;

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

    public void OnSelectCharacter(CharacterData character)
    {
        // 현재 선택된 캐릭터 정보 업데이트
        SelectCharacter = character;

        // 다이어리 정보 업데이트
        diary.UpdateDiary(character);
    }

    public override void Close()
    {
        // 현재 열린 창에서 애니메이션이 실행 중이면 무시
        if (currentWindow?.IsTweening == true) return;

        // 첫 화면이 아닌 경우
        if (_state != ContactState.Party)
        {
            switch (_state)
            {
                // 다이어리라면 파티 메뉴 내의 버튼 선택으로 넘어가기
                case ContactState.Diary:
                    FocusContactMenu();
                    break;

                // 스킬은 예외적으로 처리
                case ContactState.Skill:
                    HideSkillInformation();
                    FocusContactMenu();
                    break;

                // 그 외엔 메인 파티 메뉴로 돌아가서 다이어리 내 버튼 선택으로 넘어가기
                default:
                    ShowContact(ContactState.Party);
                    FocusDiary();
                    break;
            }
        }
        else
        {
            // 현재 창 정보 초기화
            currentWindow = null;

            // 선택 캐릭터 정보 초기화
            SelectCharacter = null;

            // 처음 창인 경우 앱 종료
            base.Close();
        }
    }

    public override void Shutdown()
    {
        currentWindow.KillAnimations();

        // 현재 창 정보 초기화
        currentWindow = null;

        // 선택 캐릭터 정보 초기화
        SelectCharacter = null;

        // 열려있는 창 모두 닫기
        CloseAllWindows();

        // 앱 셧다운
        base.Shutdown();
    }

    private void CloseAllWindows()
    {
        // 모든 창 닫기
        mainWindow.gameObject.SetActive(false);
        weaponWindow.gameObject.SetActive(false);
        offWeaponWindow.gameObject.SetActive(false);
        accessoryWindow.gameObject.SetActive(false);
        skillWindow.gameObject.SetActive(false);
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
        // 현재 상태 변경
        _state = ContactState.Diary;

        // 다이어리 내 버튼 선택
        diary.SelectLastButton();
    }

    /// <summary>
    /// 메뉴 내의 버튼 선택으로 넘아가기
    /// </summary>
    public void FocusContactMenu()
    {
        // 현재 상태 변경
        _state = ContactState.Party;

        // 다이어리 내 버튼 선택 마크 제거
        diary.LastSelectedInfo?.OnDeselect(null);

        // 선택된 캐릭터 선택으로 넘어가기
        mainWindow.SelectLastSelectedContact();
    }

    /// <summary>
    /// 현재 선택된 캐릭터의 다이어리 정보 업데이트
    /// </summary>
    public void UpdateDiaryInfo()
    {
        diary.UpdateDiary(SelectCharacter);
    }
}