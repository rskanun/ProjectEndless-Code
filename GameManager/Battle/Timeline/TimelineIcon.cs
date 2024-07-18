using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineIcon : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI turnTimer;
    public GameObject highlight;

    // 해당 타임라인에 지정된 액션
    [SerializeField]
    private BattleAction _action;
    public BattleAction Action
    {
        private set { _action = value; }
        get { return _action; }
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

    public void SetMark(bool active)
    {
        highlight.SetActive(active);
    }

    public void UpdateTurnTime()
    {
        SetTurnTime(Action.remainTurn);
    }
}