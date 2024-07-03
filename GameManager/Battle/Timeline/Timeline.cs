using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public HorizontalLayoutGroup groupComponent;
    public GameObject timeLineIcon;
    public Transform container;

    // 움직임 범위
    private Vector2 startPos;
    private Vector2 endPos;
    private float moveDistance;

    // 현재 커서 위치
    private int index;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    private void Start()
    {
        battleSeq = BattleData.Instance.Sequence;

        InitMovePos();
    }

    private void InitMovePos()
    {
        int seqCount = battleSeq.Sequence.Count;
        float pacing = groupComponent.spacing;
        float iconWidth = timeLineIcon.GetComponent<RectTransform>().rect.width;
        float width = seqCount * iconWidth + (seqCount - 1) * pacing;

        startPos = new Vector2(width / 2, container.localPosition.y);
        endPos = new Vector2(-width / 2, container.localPosition.y);
        moveDistance = iconWidth + pacing / 2;
    }

    private void InitTimeLine()
    {
        int seqCount = battleSeq.Sequence.Count;
        

    }

    public void Print()
    {
        StringBuilder timeline = new StringBuilder();
        foreach (BattleAction action in battleSeq.Sequence)
        {
            timeline.AppendFormat("{0} ({1}) -> ", action.actor.Name, action.remainTurn);
        }

        Debug.Log(timeline);
    }
}