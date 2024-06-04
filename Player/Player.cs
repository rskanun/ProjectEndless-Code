using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이벤트")]
    [SerializeField] private GameEvent hpEvent;
    [SerializeField] private GameEvent apEvent;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData stat;
    [SerializeField] private PlayerEquipData equip;

    public void InitStat()
    {
        stat.Initialization();
    }

    public void OnDamage(int damage)
    {
        stat.HP -= Mathf.Abs(damage);

        hpEvent.NotifyUpdate();
    }

    public void ApproachAwaken(int point)
    {
        stat.AP += Mathf.Abs(point);
        
        apEvent.NotifyUpdate();
    }
}