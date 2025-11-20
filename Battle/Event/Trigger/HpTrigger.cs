public class HpTrigger : EventTrigger
{
    public Entity target;
    public float percentage;

    public HpTrigger(Entity target)
    {
        this.target = target;
    }

    public override bool IsTrigger()
    {
        if (target == null)
        {
            // 정해진 타겟이 없다면 true 반환
            return true;
        }

        return target.FinalStats.HP <= target.FinalStats.MaxHP * percentage;
    }
}