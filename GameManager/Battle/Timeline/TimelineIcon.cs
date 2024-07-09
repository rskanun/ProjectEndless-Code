using UnityEngine;
using UnityEngine.UI;

public class TimelineIcon : MonoBehaviour
{
    public Image iconImage;
    public GameObject highlight;

    // 해당 타임라인에 지정된 액션
    private BattleAction _nextAction;
    public BattleAction NextAction
    {
        private set { _nextAction = value; }
        get { return _nextAction; }
    }

    public Vector2 Position
    {
        get { return transform.localPosition; }
    }

    public void SetTimeline(BattleAction nextAction)
    {
        NextAction = nextAction;

        // 해당 행동을 하는 행위자의 외형을 타임라인의 이미지로 사용
        GameObject actor = nextAction.actor.gameObject;
        SpriteRenderer actorImg = actor.GetComponent<SpriteRenderer>();

        // 타임라인 아이콘 이미지 지정
        SetImage(actorImg);
    }

    private void SetImage(SpriteRenderer sprite)
    {
        // 임시 색으로 지정
        iconImage.color = sprite.color;
    }

    public void SetMark(bool active)
    {
        highlight.SetActive(active);
    }
}