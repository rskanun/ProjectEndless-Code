using Assets.Script.Item;
using UnityEngine;

namespace Assets.Script.Object.Player
{
    // 물리 데미지 = 근력 * 무기 공격력 증가율
    // 마력 데미지 = 각 스킬에 대한 데미지
    // 피해량 = (데미지 - 마력에 의한 방어(자신의 마력 - 상대방의 마력)[최소 0])

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData player;

        [SerializeField]
        private Weapon weapon;

        private int _atkDamage;
        public int AttackDamage { get { return _atkDamage; } }

        public int MP { get { return player.MP; } }
        
        private void damageUpdate()
        {
            _atkDamage = player.STR;
            if (weapon != null) _atkDamage = Mathf.RoundToInt(_atkDamage * weapon.DamagePercent);
        }

        private void Start()
        {
            damageUpdate();
        }
    }
}