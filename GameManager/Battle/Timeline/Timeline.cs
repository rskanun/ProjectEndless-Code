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

    // 움직임 범위
    private Vector2 startPos;
    private Vector2 endPos;
    private float moveDistance;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> icons = new List<TimelineIcon>();
    private int index;

    public void InitTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        InitTimeLine();

        // 아이콘 정렬
        Canvas.ForceUpdateCanvases();

        // 첫번째 아이콘을 가운데에 위치
        MoveToStart();
    }

    private void InitTimeLine()
    {
        int seqCount = battleSeq.Sequence.Count;

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

    public void AddTimeline(BattleAction nextAction)
    {
        GameObject iconObj = Instantiate(timeLineIcon, container);
        TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

        // 타임라인 지정
        icon.SetTimeline(nextAction);

        // 아이콘 목록에 추가
        InsertSorted(icon);
    }

    private void InsertSorted(TimelineIcon newIcon)
    {
        // 새 아이콘의 삽입 위치를 결정
        int newIndex = icons.Count;

        // 삽입 위치를 찾기 위해 remainTurn 값을 비교하여 정렬
        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i].NextAction.remainTurn > newIcon.NextAction.remainTurn)
            {
                newIndex = i;
                break;
            }
        }

        // 리스트에 새 아이콘을 삽입
        icons.Insert(newIndex, newIcon);

        // UI에서의 순서도 업데이트
        newIcon.transform.SetSiblingIndex(newIndex);
    }

    public void UpdateTimeline()
    {
        // 현재 맨 앞에 있는 타임라인 삭제
        RemoveCurTimeline();

        // 새 타임라인 추가
    }

    private void RemoveCurTimeline()
    {
        // 현재 차례인 타임라인 삭제
        Destroy(icons[0].gameObject);
        icons.RemoveAt(0);

        // 위치 초기화
        MoveToStart();
    }

    private void MoveToStart()
    {
        index = battleSeq.Sequence.Count - 1;

        container.transform.localPosition = icons[index].Position;
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

    public void MoveToEnd()
    {
        index = 0;

        container.transform.localPosition = icons[index].Position;
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