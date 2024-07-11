using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public HorizontalLayoutGroup groupComponent;
    public GameObject timeLineIcon;
    public Transform container;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> timelines = new List<TimelineIcon>();
    private int index;

    public void InitTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        InitTimeLine();

        // 첫번째 아이콘을 가운데에 위치
        MoveToStart();
    }

    private void InitTimeLine()
    {
        foreach (BattleAction action in battleSeq.Sequence)
        {
            GameObject iconObj = Instantiate(timeLineIcon, container);
            TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

            // 타임라인 지정
            icon.SetTimeline(action);

            // 아이콘 목록에 추가
            timelines.Add(icon);
        }
    }

    public void UpdateTimeline()
    {
        // 현재 맨 앞에 있는 타임라인 삭제
        RemoveTimeline();

        // 새 타임라인 추가
        AddTimeline();

        // 타임라인 턴타이머 업데이트
        foreach (TimelineIcon icon in timelines)
        {
            icon.UpdateTurnTime();
        }
    }

    private void RemoveTimeline()
    {
        List<BattleAction> seq = battleSeq.Sequence;

        timelines.RemoveAll(timeline => !seq.Contains(timeline.NextAction));
    }

    private void AddTimeline()
    {
        List<BattleAction> seq = battleSeq.Sequence; 

        for (int i = 0; i < seq.Count; i++)
        {
            // 현재 타임라인에 없는 시퀀스가 있을 경우 타임라인에 추가
            if (timelines.Count <= i || seq[i] != timelines[i].NextAction)
            {
                // 타임라인 추가
                GameObject iconObj = Instantiate(timeLineIcon, container);
                TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

                // 타임라인 지정
                icon.SetTimeline(seq[i]);

                // 위치 지정
                iconObj.transform.SetSiblingIndex(i + 1);

                // 아이콘 목록에 추가
                timelines.Insert(i, icon);
            }
        }
    }

    private void MoveToStart()
    {
        // 아이콘 정렬
        Canvas.ForceUpdateCanvases();

        // 아이콘 Container 이동
        index = battleSeq.Sequence.Count - 1;

        Vector2 startPos = new Vector2(timelines[index].Position.x, container.transform.localPosition.y);
        container.transform.localPosition = startPos;
    }

    public void MoveToNext()
    {
        if (index <= 0)
        {
            // 맨 첫번째라면 넘기기 X
            return;
        }

        container.transform.localPosition = timelines[--index].Position;
    }

    public void MoveToPrev()
    {
        if (index >= timelines.Count - 1)
        {
            // 맨 마지막이라면 넘기기 X
            return;
        }

        container.transform.localPosition = timelines[++index].Position;
    }

    public void MarkCurIcon()
    {
        timelines[0].SetMark(true);
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