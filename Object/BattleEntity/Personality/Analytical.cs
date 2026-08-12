using System.Collections.Generic;
using UnityEngine;

public class Analytical : Personality
{
    // 재사용 인스턴스
    private Dictionary<Entity, float> weightData = new(4);
    private List<Entity> sortedList = new(4);

    public Analytical() : base(PersonalityType.Analytical) { }

    protected override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        // 재사용 인스턴스 초기화
        weightData.Clear();
        sortedList.Clear();

        // 타겟이 없는 경우 빈 값 리턴
        if (targetList == null || targetList.Count == 0)
        {
            return weightData;
        }

        // 리스트 복사(오염 방지)
        sortedList.AddRange(targetList);

        // 체력이 낮은 순서대로 재정렬
        sortedList.Sort((x, y) => y.FinalStats.HP.CompareTo(x.FinalStats.HP));

        // 체력에 따른 가중치 값
        float hpWeight = 1.0f;

        // 체력이 낮은 순서부터 순회
        foreach (Entity target in sortedList)
        {
            // 가중치 초기값 설정
            weightData[target] = 0.0f;

            // 체력이 낮은 적부터 높은 가중치 부여
            weightData[target] += hpWeight;

            // 전방에 위치한 적인 경우
            if (target.Position == BattlePosition.Front)
            {
                // 가중치 부여
                weightData[target] += 1.0f;
            }

            // 체력 가중치 낮추기(최소 0)
            hpWeight = Mathf.Max(0.0f, hpWeight - 0.1f);
        }

        return weightData;
    }
}