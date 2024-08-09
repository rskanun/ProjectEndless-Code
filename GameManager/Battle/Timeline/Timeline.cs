using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TimelineUI ui;

    // 시퀀스 데이터
    private BattleSequence battleSeq;

    // 타임라인 아이콘 관리
    private List<TimelineIcon> timelines = new List<TimelineIcon>();
    private int centerIndex;
    public int CenterIndex
    {
        get { return centerIndex; }
    }

    private bool isActiveInsert;

    public bool isMovable;

    /***************************************************************
    * [ 전투 타임라인 ]
    * 
    * 전투 진행에 따른 턴 순서를 나타내는 타임라인 처리
    ***************************************************************/

    public void SetupTimeline(BattleSequence battleSeq)
    {
        this.battleSeq = battleSeq;

        InitTimeLine();

        // 타임라인 위치 처음으로 이동
        MoveStart();
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
    * [ 타임라인 삽입 ]
    * 
    * 캐릭터의 다음 행동이 어느 턴에 진행할 지 보여줌
    ***************************************************************/

    public void SetActiveInsert(bool isActive)
    {
        isActiveInsert = isActive;

        // 삽입 아이콘 활성화 설정
        ui.SetActiveInsertIcon(isActive);

        // 삽입을 활성화 할 경우 삽입 아이콘 활성화
        if (isActive)
        {
            Entity curTurnChr = timelines[0].Action.actor;
            GameObject chrObj = curTurnChr.gameObject;

            // 삽입 아이콘 이미지는 현재 턴인 캐릭터의 타임라인 아이콘 이미지
            ui.SetInsertIconImage(chrObj);
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
        if (isMovable && centerIndex < timelines.Count - 1)
        {
            MoveIndex(centerIndex + 1);
        }
    }

    public void MovePrev()
    {
        if (isMovable && centerIndex > 0)
        {
            MoveIndex(centerIndex - 1);
        }
    }

    public void MoveIndex(int index)
    {
        // 이전 아이콘 마킹 해제
        SetMarkingIcon(centerIndex, false);

        // 새 중앙 아이콘 할당
        SetCenterIcon(index);

        // 현재 아이콘 마킹
        SetMarkingIcon(centerIndex, true);
    }

    private void SetCenterIcon(int index)
    {
        centerIndex = index;

        if (isActiveInsert == false) ui.CenterIconAtIndex(index);
        else
        {
            // 삽입 아이콘의 위치는 첫번째 아이콘의 무조건 뒤
            ui.CenterIconAtIndex(index + 1);
            ui.SetSiblingInsertIcon(index + 1);
        }
    }

    private void SetMarkingIcon(int index, bool isMarking)
    {
        // 삽입 아이콘이 활성화 된 상태라면 무조건 마킹 해제
        timelines[index].SetMark(isMarking && !isActiveInsert);
    }
}