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

        private float damage;
        public float Damage { get { return damage; } }

        public float MP { get { return player.MP; } }
        
        private void damageUpdate()
        {
            damage = player.STR;
            if (weapon != null) damage *= weapon.DamagePercent;
        }

        private void Start()
        {
            damageUpdate();
        }
    }
}