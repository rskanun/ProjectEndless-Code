using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SequenceIcon : TimelineIcon
{
    public Image iconImage;
    public TextMeshProUGUI turnTimerText;
    public GameObject turnTimer;
    public GameObject border;

    private Vector2 originSize;

    private void Awake()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        originSize = rect.sizeDelta;
    }

    public void SetTimeline(BattleAction action)
    {
        Action = action;

        // 임시
        name = $"{action.actor.Name} Timeline";

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
        turnTimerText.text = time.ToString("0.0");
    }

    public override void SetMarking()
    {
        // 마킹된 아이콘 사이즈 조정
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        rect.sizeDelta = originSize + new Vector2(10f, 10f);

        // 테두리 활성화
        border.SetActive(true);

        // 현재 타임라인의 경우 남은 턴 수 숨기기
        turnTimer.SetActive(false);
    }

    public override void ClearMarking()
    {
        // 본래 사이즈로 조정
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        rect.sizeDelta = originSize;

        // 하이라이트 비활성화
        border.SetActive(false);

        // 현재 타임라인의 경우 남은 턴 수 나타내기
        turnTimer.SetActive(true);
    }

    public override void UpdateTurnTime()
    {
        SetTurnTime(Action.remainTurn);
    }
}