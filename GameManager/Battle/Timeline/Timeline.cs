using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<TimelineIcon> icons = new List<TimelineIcon>();
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
            icons.Add(icon);
        }
    }

    public void UpdateTimeline()
    {
        // 현재 맨 앞에 있는 타임라인 삭제
        RemoveCurTimeline();

        // 새 타임라인 추가
        UpdateNewTimeline();

        // 타임라인 턴타이머 업데이트
        foreach (TimelineIcon icon in icons)
        {
            icon.UpdateTurnTime();
        }
    }

    private void RemoveCurTimeline()
    {
        // 현재 차례인 타임라인 삭제
        Destroy(icons[0].gameObject);
        icons.RemoveAt(0);
    }

    private void UpdateNewTimeline()
    {
        List<BattleAction> seq = battleSeq.Sequence; 

        for (int i = 0; i < icons.Count; i++)
        {
            // 새로 추가된 타임라인 부분 찾기
            if (seq[i] != icons[i].NextAction)
            {
                // 타임라인 추가
                AddTimeline(i, seq[i]);

                // 찾기 종료
                return;
            }
        }

        // 자리를 찾지 못한 경우 마지막 자리에 추가
        if (seq.Count > icons.Count)
        {
            int index = icons.Count;
            AddTimeline(index, seq[index]);
        }
    }

    public void AddTimeline(int index, BattleAction nextAction)
    {
        GameObject iconObj = Instantiate(timeLineIcon, container);
        TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

        // 타임라인 지정
        icon.SetTimeline(nextAction);

        // 위치 지정
        iconObj.transform.SetSiblingIndex(index + 1);

        // 아이콘 목록에 추가
        icons.Insert(index, icon);
    }

    private void MoveToStart()
    {
        // 아이콘 정렬
        Canvas.ForceUpdateCanvases();

        // 아이콘 Container 이동
        index = battleSeq.Sequence.Count - 1;

        Vector2 startPos = new Vector2(icons[index].Position.x, container.transform.localPosition.y);
        container.transform.localPosition = startPos;
    }

    public void MoveToNext()
    {
        if (index <= 0)
        {
            // 맨 첫번째라면 넘기기 X
            return;
        }

        container.transform.localPosition = icons[--index].Position;
    }

    public void MoveToPrev()
    {
        if (index >= icons.Count - 1)
        {
            // 맨 마지막이라면 넘기기 X
            return;
        }

        container.transform.localPosition = icons[++index].Position;
    }

    public void MarkCurIcon()
    {
        icons[0].SetMark(true);
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