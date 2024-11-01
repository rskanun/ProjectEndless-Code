using System.Collections.Generic;
using System.Linq;

public class Analytical : Personality
{
    public Analytical() : base(PersonalityType.Analytical) { }

    public override Dictionary<Entity, float> GetWeightData(List<Entity> targetList)
    {
        Dictionary<Entity, float> weightData = new Dictionary<Entity, float>();

        // 체력에 따른 가중치 값
        float hpWeight = 1.0f;

        // 체력이 낮은 순서부터 순회
        foreach (Entity target in targetList.OrderBy(entity => entity.Stat.HP))
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

            // 체력 가중치 낮추기
            hpWeight -= 0.1f;
        }

        return weightData;
    }
}