/***************************************************************
* [ 분석적인 성격 (Analytical) ]
* 
* 체력이 적은 적과 전방에 있는 적을 우선시 한다.
* 
* <가중치>
* 체력이 없는 적부터 +1, +0.9,…
* 전방에 있는 적 +1
****************************************************************/
using System.Collections.Generic;

public class Analytical : IPersonality
{
    public List<Entity> GetPriorityTargetList()
    {
        throw new System.NotImplementedException();
    }
}