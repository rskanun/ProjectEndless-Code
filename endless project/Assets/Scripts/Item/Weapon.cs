using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    [SerializeField]
    private float damagePercent;
    /***************************************************************
     * [ 데미지 증가율 (Damage Percent) ]
     * 
     * 장착한 캐릭터의 데미지를 일정 퍼센트 증가시켜준다.
     ****************************************************************/
    public float DamagePercent
    {
        get { return damagePercent; }
    }
}