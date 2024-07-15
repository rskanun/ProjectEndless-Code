using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public HorizontalLayoutGroup groupComponent;
    public GameObject timelineIcon;
    public RectTransform container;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 위치 데이터
    private float iconWidth;
    private float spacing;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> timelines = new List<TimelineIcon>();
    private int index;

    public void InitTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        InitTimeLine();
        InitPosData();

        // 첫번째 아이콘을 가운데에 위치
        ResetPosition();
    }

    private void InitTimeLine()
    {
        foreach (BattleAction action in battleSeq.Sequence)
        {
            GameObject iconObj = Instantiate(timelineIcon, container);
            TimelineIcon icon = iconObj.GetComponent<TimelineIcon>();

            // 타임라인 지정
            icon.SetTimeline(action);

            // 아이콘 목록에 추가
            timelines.Add(icon);
        }
    }

    private void InitPosData()
    {
        iconWidth = timelineIcon.GetComponent<RectTransform>().rect.width;
        spacing = groupComponent.spacing;
    }

    public void UpdateTimeline()
    {
        // 현재 맨 앞에 있는 타임라인 삭제
        RemoveTimeline();

        // 새 타임라인 추가
        AddTimeline();

        // 맨 처음 타임라인이 가운데로 오도록 이동
        ResetPosition();

        // 타임라인 턴타이머 업데이트
        foreach (TimelineIcon icon in timelines)
        {
            icon.UpdateTurnTime();
        }
    }

    private void RemoveTimeline()
    {
        List<BattleAction> seq = battleSeq.Sequence;

        // 타임라인을 역순으로 순회하여 조건에 맞지 않는 타임라인을 제거
        for (int i = timelines.Count - 1; i >= 0; i--)
        {
            if (!seq.Contains(timelines[i].NextAction))
            {
                Destroy(timelines[i].gameObject);
                timelines.RemoveAt(i);
            }
        }
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
                GameObject iconObj = Instantiate(timelineIcon, container);
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

    private void ResetPosition()
    {
        float startPosX = (iconWidth + spacing) / 2 * (timelines.Count - 1);
        Vector2 startPos = new Vector2(startPosX, container.localPosition.y);

        container.localPosition = startPos;
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