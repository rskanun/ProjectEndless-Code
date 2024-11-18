using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleSequence
{
    // 전투 순서
    [SerializeReference]
    private List<BattleAction> _sequence;
    public List<BattleAction> Sequence
    {
        private set { _sequence = value; }
        get { return _sequence; }
    }

    // 시퀀스 업데이트
    private GameEvent SeqUpdateEvent
        => GameEventResource.Instance.SequenceUpdateEvent;

    public void SetSequence(List<Entity> entityList)
    {
        Sequence = new List<BattleAction>();

        // 엔티티의 민첩 수치로 내림차순 정렬
        List<Entity> sortedList = new List<Entity>(entityList);
        sortedList.Sort((x, y) => y.Stat.AGI.CompareTo(x.Stat.AGI));

        // 모든 엔티티들은 0턴 대기 행동 시전
        foreach (Entity entity in sortedList)
        {
            WaitAction turnData = new WaitAction();

            turnData.remainTurn = 0.0f;
            turnData.actor = entity;

            Sequence.Add(turnData);
        }
    }

    public void NextTurn()
    {
        Sequence.RemoveAt(0);

        // 다음 턴만큼 수치 앞당기기
        PassedTurn(Sequence[0].remainTurn);

        // 시퀀스 업데이트 알림
        SeqUpdateEvent.NotifyUpdate();
    }

    private void PassedTurn(float turn)
    {
        foreach (BattleAction turnData in Sequence)
        {
            turnData.remainTurn -= turn;
        }

        // 전투 데이터에 경과한 만큼의 턴 추가
        CurrentBattleData.Instance.OnPassedTurn(turn);
    }

    public BattleAction GetTurnAction(int index)
    {
        if (Sequence.Count <= index) return null;
        return Sequence[index];
    }

    public BattleAction GetEntityAction(Entity actor)
    {
        foreach (BattleAction action in Sequence)
        {
            if (action.actor == actor)
                return action;
        }

        return null;
    }

    public void AddTurn(BattleAction action)
    {
        int index = Sequence.BinarySearch(action);

        if (index < 0) Sequence.Insert(~index, action);
        else Sequence.Insert(index, action);

        // 시퀀스 업데이트 알림
        SeqUpdateEvent.NotifyUpdate();
    }

    public void AddTurn(BattleAction action, int index)
    {
        if (index >= Sequence.Count) Sequence.Add(action);
        else Sequence.Insert(index, action);

        // 시퀀스 업데이트 알림
        SeqUpdateEvent.NotifyUpdate();
    }

    public void RemoveTurns(Entity actor)
    {
        for (int i = Sequence.Count - 1; i >= 0; i--)
        {
            if (Sequence[i].actor == actor)
            {
                Sequence.RemoveAt(i);
            }
        }

        // 시퀀스 업데이트 알림
        SeqUpdateEvent.NotifyUpdate();
    }

    public int GetActionMinSeq(BattleAction action)
    {
        int index = Sequence.BinarySearch(action);
        if (index < 0) index = ~index;

        return index;
    }

    public int GetActionMinSeq(Entity actor)
    {
        BattleAction action = GetEntityAction(actor);

        return GetActionMinSeq(action);
    }

    public int GetActionMinSeq(Entity actor, float costTurn)
    {
        // 임시로 행동을 만들어 배치될 수 있는 최소 위치 리턴
        WaitAction action = new WaitAction();

        action.actor = actor;
        action.remainTurn = costTurn;

        return GetActionMinSeq(action);
    }

    public int GetSeqIndex(Entity actor)
    {
        return Sequence.FindIndex(seq => seq.actor == actor);
    }
}