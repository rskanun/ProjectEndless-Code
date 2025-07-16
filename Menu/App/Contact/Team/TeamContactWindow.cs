using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamContactWindow : ContactWindow
{
    [Header("연락처 오브젝트")]
    [SerializeField] private List<GameObject> fadeOutObjects;
    [SerializeField] private GameObject contactPrefab;

    [Header("참조 스크립트")]
    [SerializeField] private ContactApp app;
    [SerializeField] private TeamContact playerContact;

    private List<GameObject> contactList = new();

    private void Awake()
    {
        // 플레이어 오브젝트의 핸들러 등록
        playerContact.SetSelectAction(() => app.OnSelectCharacter(PartyData.Instance.Player));
        playerContact.SetSubmitHandler(() => app.ShowWeapons());
    }

    private void OnDisable()
    {
        // 해당 창이 비활성화 될 때, 모든 오브젝트 목록을 지우기
        foreach (GameObject obj in contactList)
        {
            Destroy(obj);
        }
    }

    protected override void InitContact()
    {
        // 플레이어 데이터 설정
        playerContact.UpdateInfo(PartyData.Instance.Player);

        // 모든 캐릭터 중 파티 편입이 가능한 캐릭터만 뽑아내기
        List<CharacterData> unlockChrs = PartyData.Instance.Characters.Where(chr => chr.IsUnlocked).ToList();

        // 파티 편입이 가능한 캐릭터 수만큼 오브젝트 생성
        foreach (CharacterData character in unlockChrs)
        {
            if (character is PlayerData) continue;

            // 해당 캐릭터 정보를 토대로 한 연락처(=정보) 오브젝트 생성
            GameObject contactObj = Instantiate(contactPrefab, content);
            TeamContact contact = contactObj.GetComponent<TeamContact>();

            // 정보 및 핸들러 등록
            contact.UpdateInfo(character);
            contact.SetSelectAction(() => app.OnSelectCharacter(character));
            contact.SetSubmitHandler(() => app.ShowWeapons());

            // 후에 파괴를 위한 리스트에 추가
            contactList.Add(contactObj);
        }

        // 플레이어의 연락처를 먼저 선택
        EventSystem.current.SetSelectedGameObject(playerContact.gameObject);
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

        // 레이아웃 그룹 잠시 끄기
        layoutGroup.enabled = false;

        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i) is not RectTransform item) continue;

            GameObject obj = item.gameObject;

            // 페이드 아웃 목록들은 따로 페이드 아웃으로 사라짐
            if (fadeOutObjects.Contains(obj))
            {
                CanvasGroup cg = obj.GetComponent<CanvasGroup>();

                // 캔버스 그룹 컴포넌트가 없다면 본래 애니메이션으로 처리
                if (cg != null)
                {
                    cg.DOFade(0.0f, duration)
                        .SetDelay(i * interval);
                }
            }
            else
            {
                Vector2 targetPos = item.anchoredPosition - new Vector2(offsetX, 0);

                // 나머지 오브젝트는 왼쪽으로 빠지며 페이드 아웃되는 애니메이션 실행
                item.DOAnchorPos(targetPos, duration)
                    .SetDelay(i * interval)
                    .SetEase(Ease.OutCubic);
            }
        }

        // 애니메이션 종료까지 대기
        yield return new WaitForSeconds(content.childCount * interval + duration);

        // 애니메이션 종료 후 레이아웃 그룹 다시 작동
        layoutGroup.enabled = true;

        // 코루틴 애니메이션 종료 선언
        _isTweening = false;

        // 해당 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}