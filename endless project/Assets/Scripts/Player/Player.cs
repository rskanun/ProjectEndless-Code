using UnityEngine;

public class Player : MonoBehaviour
{
    // 이벤트 목록
    private HpEvent hpEvent;
    private ApEvent apEvent;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData stat;
    [SerializeField] private PlayerEquipData equip;

    private void Start()
    {
        hpEvent = HpEvent.Instance;
        apEvent = ApEvent.Instance;
    }

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