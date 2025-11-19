using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PersonalityType
{
    Belligerent, // 호전적인
    Cautious, // 신중한
    Crusty, // 신경질적인
    Brave, // 용감한
    Analytical, // 분석적인
}

public abstract class Personality
{
    public PersonalityType type { get; private set; }

    public Personality(PersonalityType type)
    {
        this.type = type;
    }

    public static Personality OfType(PersonalityType type)
    {
        return type switch
        {
            PersonalityType.Belligerent => new Belligerent(),
            PersonalityType.Cautious => new Cautious(),
            PersonalityType.Crusty => new Crusty(),
            PersonalityType.Brave => new Brave(),
            PersonalityType.Analytical => new Analytical(),

            _ => null
        };
    }

    public List<Entity> GetPriorityTargetList(List<Entity> targetList)
    {
        Dictionary<Entity, float> weightData = GetWeightData(targetList);

        // 가중치가 높은 순서대로 Entity 개체만 따로 빼내어 리스트로 만들어 반환
        return weightData
            .OrderByDescending(tw => tw.Value)
            .Select(tw => tw.Key)
            .ToList();
    }

    public void OnTurnStart()
    {
        // 다음 턴이 시작될 때, 해당 턴 정보 가져오기
        BattleAction curAction = BattleData.Instance.Sequence.GetTurnAction(0);

        // 해당 턴 안의 필요한 정보 저장
        GatherCurTurnAction(curAction);
    }

    protected abstract Dictionary<Entity, float> GetWeightData(List<Entity> targetList);

    protected virtual void GatherCurTurnAction(BattleAction action)
    {
        // 다음 턴이 진행될 경우 해당 턴의 정보를 수집하는 함수
    }
}