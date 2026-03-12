using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamContactWindow : ContactWindow
{
    [SerializeField] private CanvasGroup darkPanel;
    [SerializeField] private NavigationGroup naviGroup;
    [SerializeField] private TeamContactController controller;

    [Header("연락처 오브젝트")]
    [SerializeField] private List<GameObject> fadeOutObjects;
    [SerializeField] private GameObject contactPrefab;

    [Header("참조 스크립트")]
    [SerializeField] private ContactApp app;
    [SerializeField] private TeamContact playerContact;

    private TeamContact lastSelected;

    // 애니메이션 정보
    private float hideShowMoveRange = 80.0f;
    private float hideShowDuration = 0.15f;

    private void Awake()
    {
        CharacterData playerData = PartyData.Instance.Player;

        // 플레이어 오브젝트의 핸들러 등록
        playerContact.SetSelectAction(() => OnSelectContact(playerContact, playerData));
        playerContact.SetClickHandler(() => OnClickContact());
        playerContact.SetSubmitHandler(() => ModifyCharacter(playerData));
    }

    private void OnDisable()
    {
        // 해당 창이 비활성화 될 때, 모든 오브젝트 목록을 지우기
        DestroyContactObjs();
    }

    /// <summary>
    /// 스킬 정보를 위해 잠시 숨겨놓았던 화면 불러오기
    /// </summary>
    public void ShowWindow()
    {
        // 본래 위치 저장
        float originPosX = content.localPosition.x;

        // 화면 위치 옮겨놓기
        content.transform.localPosition -= new Vector3(hideShowMoveRange, 0);

        // 화면 복구 애니메이션
        DOTween.Sequence()
            .Join(content.transform.DOLocalMoveX(originPosX, hideShowDuration));
    }

    /// <summary>
    /// 스킬 정보를 위해 화면 잠시 숨기기
    /// </summary>
    public void HideWindow()
    {
        float originPosX = content.localPosition.x;

        // 화면 숨김 애니메이션 실행
        DOTween.Sequence()
            .Join(content.transform.DOLocalMoveX(originPosX - hideShowMoveRange, hideShowDuration)) // 화면을 통째로 옆으로 이동
            .Join(darkPanel.DOFade(0.5f, hideShowDuration)) // 화면 점점 어둡게
            .OnKill(() =>
            {
                content.transform.localPosition = new Vector3(originPosX, content.transform.localPosition.y);
                darkPanel.alpha = 0.0f;
            });
    }

    /// <summary>
    /// 마지막으로 선택된 연락처 오브젝트 선택
    /// </summary>
    public void SelectLastSelectedContact()
    {
        EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
    }

    /// <summary>
    /// 현재 선택된 파티원의 파티 참여 상태를 스위칭
    /// </summary>
    public void SwitchPartyState()
    {
        if (lastSelected.Character.IsParty) KickParty(lastSelected);
        else JoinParty(lastSelected);
    }

    private void JoinParty(TeamContact joinMember)
    {
        PartyData partyData = PartyData.Instance;

        // 받을 수 있는 파티 인원수를 초과한 경우 또는 해당 파티원이 사망한 경우 무시
        bool isPartyFull = partyData.GetPartyMembers().Count >= partyData.MaxPartySize;
        bool isDead = joinMember.Character.IsSlain;
        if (isPartyFull || isDead)
        {
            return;
        }

        // 해당 캐릭터를 파티에 추가
        joinMember.SetPartyStatus(true);
        partyData.JoinParty(joinMember.Character.Name);
    }

    private void KickParty(TeamContact kickMember)
    {
        // 플레이어는 파티에서 추방 X
        if (kickMember.Character == PartyData.Instance.Player)
        {
            return;
        }

        // 해당 캐릭터를 파티에서 추방
        kickMember.SetPartyStatus(false);
        PartyData.Instance.KickParty(kickMember.Character.Name);
    }

    /// <summary>
    /// 초기 연락처 목록 생성
    /// </summary>
    protected override void InitContact()
    {
        lastSelected = playerContact;

        // 플레이어 데이터 설정
        playerContact.UpdateInfo(PartyData.Instance.Player);

        // 모든 캐릭터 중 파티 편입이 가능한 캐릭터만 뽑아내기
        List<CharacterData> unlockChrs = PartyData.Instance.Characters.Where(chr => chr.IsUnlocked).ToList();

        // 파티 편입이 가능한 캐릭터 수만큼 오브젝트 생성
        foreach (CharacterData character in unlockChrs)
        {
            // 플레이어는 미리 생성되어 있기에 패스
            if (character == PartyData.Instance.Player) continue;

            // 해당 캐릭터 정보를 토대로 한 연락처(=정보) 오브젝트 생성
            GameObject contactObj = Instantiate(contactPrefab, content);
            TeamContact contact = contactObj.GetComponent<TeamContact>();

            // 정보 및 핸들러 등록
            contact.UpdateInfo(character);
            contact.SetSelectAction(() => OnSelectContact(contact, character));
            contact.SetClickHandler(() => OnClickContact());
            contact.SetSubmitHandler(() => ModifyCharacter(character));

            // 후에 파괴를 위한 리스트에 추가
            contactList.Add(contactObj);

            // 마지막에 선택한 오브젝트인 경우 해당 오브젝트를 선택하기
            if (character == app.SelectCharacter) lastSelected = contact;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        // 버튼 네비게이션 설정
        naviGroup.SetupChildsNavigation();

        // 컨트롤러 설정
        controller.ControlConnect();

        // 메뉴에 포커싱 된 상태라면, 이전에 선택한 캐릭터, 혹은 플레이어 먼저 선택
        if (app.State == ContactState.Party)
            EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
    }

    private void OnSelectContact(TeamContact contact, CharacterData character)
    {
        lastSelected = contact;

        app.OnSelectCharacter(character);
        UpdateScrollPosition(contact.gameObject);
    }

    private void OnClickContact()
    {
        // 파티 목록 상태에서 클릭한 경우는 무시
        if (app.State == ContactState.Party) return;

        // 메뉴에 초점 두기
        app.FocusContactMenu();
    }

    private void ModifyCharacter(CharacterData character)
    {
        // 해당 캐릭터가 사망했다면 목록 불러오기 X
        if (character.IsSlain) return;

        // 다이어리로 넘어가기
        app.FocusDiary();
    }

    protected override IEnumerator OpenAnimation()
    {
        // 페이드 아웃된 목록들의 알파값 초기화
        foreach (GameObject obj in fadeOutObjects)
        {
            CanvasGroup cg = obj.GetComponent<CanvasGroup>();

            if (cg == null) continue;

            cg.alpha = 1.0f;
        }

        // 기존 애니메이션 진행
        return base.OpenAnimation();
    }

    protected override IEnumerator CloseAnimation()
    {
        _isTweening = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        yield return null;

        // 컨트롤러 설정
        controller.ControlDisconnect();

        // 레이아웃 그룹 잠시 끄기
        layoutGroup.enabled = false;

        // 화면에 표시되는 y 경계값 찾기
        float minY = -content.localPosition.y;
        float maxY = -content.localPosition.y - viewportRect.rect.height;

        int count = 0; // 애니메이션이 실행되는 오브젝트 개수
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            float contactMinY = item.localPosition.y;
            float contactMaxY = item.localPosition.y - item.rect.height;

            // 경계값 밖의 오브젝트는 애니메이션 적용 X
            if (minY <= contactMaxY || maxY >= contactMinY) continue;

            GameObject obj = item.gameObject;

            // 페이드 아웃 목록들은 따로 페이드 아웃으로 사라짐
            if (fadeOutObjects.Contains(obj))
            {
                CanvasGroup cg = obj.GetComponent<CanvasGroup>();

                // 캔버스 그룹 컴포넌트가 없다면 본래 애니메이션으로 처리
                if (cg != null)
                {
                    Tween tween = cg.DOFade(0.0f, duration)
                        .SetDelay(count++ * interval);
                    tween.OnKill(() => playAnimations.Remove(tween));

                    playAnimations.Add(tween);
                }
            }
            else
            {
                Vector2 targetPos = item.anchoredPosition - new Vector2(offsetX, 0);

                // 나머지 오브젝트는 왼쪽으로 빠지며 페이드 아웃되는 애니메이션 실행
                Tween tween = item.DOAnchorPos(targetPos, duration)
                    .SetDelay(count++ * interval)
                    .SetEase(Ease.OutCubic);
                tween.OnKill(() => playAnimations.Remove(tween));

                playAnimations.Add(tween);
            }
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(count * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;

        // 해당 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}