using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TimelineUI ui;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> timelines = new List<TimelineIcon>();

    public void SetupTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        InitTimeLine();

        // 타임라인 위치 처음으로 이동
        ui.SetPos(0);
    }

    private void InitTimeLine()
    {
        foreach (BattleAction action in battleSeq.Sequence)
        {
            TimelineIcon icon = ui.CreateTimelineIcon(action);

            // 아이콘 목록에 추가
            timelines.Add(icon);
        }
    }

    public void ResetTimeline()
    {
        // 타임라인 위치 초기화
        ui.SetPos(0);

        // 첫번째 아이콘 마킹
        timelines[0].SetMark(true);
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

        // 타임라인을 역순으로 순회하여 조건에 맞지 않는 타임라인을 제거
        for (int i = timelines.Count - 1; i >= 0; i--)
        {
            if (!seq.Contains(timelines[i].Action))
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
            if (timelines.Count <= i || seq[i] != timelines[i].Action)
            {
                // 타임라인 추가
                TimelineIcon icon = ui.CreateTimelineIcon(seq[i], i);

                // 아이콘 목록에 추가
                timelines.Insert(i, icon);
            }
        }
    }
}