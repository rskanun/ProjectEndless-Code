using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TimelineUI ui;

    [Header("참조 컴포넌트")]
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> timelines;
    private int centerIndex;
    public int CenterIndex
    {
        get { return centerIndex; }
    }
    public TimelineIcon CenterIcon
    {
        get { return timelines[CenterIndex]; }
    }

    /***************************************************************
    * [ 전투 타임라인 ]
    * 
    * 전투 진행에 따른 턴 순서를 나타내는 타임라인 처리
    ***************************************************************/

    public void SetupTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        // 전투 시퀀스에 따른 타임라인 목록 생성
        InitTimeLine();

        // 타임라인 위치 처음으로 이동
        MoveStart();
    }

    private void InitTimeLine()
    {
        timelines = new List<TimelineIcon>();

        foreach (BattleAction action in battleSeq.Sequence)
        {
            TimelineIcon icon = ui.CreateTimelineIcon(action);

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

        // 타임라인 위치 초기화
        MoveStart();

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

    /***************************************************************
    * [ 타임라인 이동 ]
    * 
    * 규격에 맞춘 타임라인 이동 처리
    ***************************************************************/

    public void MoveStart()
    {
        // 처음 아이콘의 위치로 이동
        MoveIndex(0);
    }

    public void MoveNext()
    {
        if (centerIndex < timelines.Count - 1)
        {
            MoveIndex(centerIndex + 1);
        }
    }

    public void MovePrev()
    {
        if (centerIndex > 0)
        {
            MoveIndex(centerIndex - 1);
        }
    }

    public void MoveIndex(int index)
    {
        // 타임라인 이동
        MoveTimelineAtIndex(index);

        // 마킹 변경
        SetCenterIcon(index);
    }

    public void MoveTimelineAtIndex(int index)
    {
        ui.CenterIconAtIndex(index);
    }

    private void SetCenterIcon(int index)
    {
        TimelineIcon centerIcon = timelines[centerIndex];
        TimelineIcon nextIcon = timelines[index];

        // 이전 아이콘의 마킹 해제 및 현재 아이콘 마킹
        centerIcon.ClearMarking();
        nextIcon.SetMarking();

        // 센터 아이콘 변경
        centerIndex = index;

        // 레이아웃 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
}