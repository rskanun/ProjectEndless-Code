using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineIcon : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI turnTimer;
    public GameObject turnTimerIcon;
    public GameObject highlight;

    private Vector2 originSize;

    // 해당 타임라인에 지정된 액션
    private BattleAction _action;
    public BattleAction Action
    {
        private set { _action = value; }
        get { return _action; }
    }

    private void Awake()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        originSize = rect.sizeDelta;
    }

    public void SetTimeline(BattleAction action)
    {
        Action = action;

        // 해당 행동을 하는 행위자의 외형을 타임라인의 이미지로 사용
        GameObject actor = action.actor.gameObject;
        SpriteRenderer actorImg = actor.GetComponent<SpriteRenderer>();

        // 타임라인 아이콘 이미지 지정
        InitImage(actorImg);

        // 남은 턴 지정
        SetTurnTime(action.remainTurn);
    }

    private void InitImage(SpriteRenderer sprite)
    {
        // 임시 색으로 지정
        iconImage.color = sprite.color;
    }

    private void SetTurnTime(float time)
    {
        turnTimer.text = time.ToString("0.0");
    }

    public void SetMark(bool isActive)
    {
        // 마킹된 아이콘 사이즈 조정
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Vector2 sizeChange = new Vector2(10f, 10f);

        rect.sizeDelta = originSize + (isActive ? sizeChange : Vector2.zero);

        // 하이라이트 활성화
        highlight.SetActive(isActive);

        // 현재 타임라인의 경우 남은 턴 수 숨기기
        SetActiveTurnTimer(!isActive);
    }

    private void SetActiveTurnTimer(bool isActive)
    {
        turnTimer.gameObject.SetActive(isActive);
        turnTimerIcon.SetActive(isActive);
    }

    public void UpdateTurnTime()
    {
        SetTurnTime(Action.remainTurn);
    }
}