using Assets.Script.Item;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Script.Object.Player
{
    // 물리 데미지 = 근력 * 무기 공격력 증가율
    // 마력 데미지 = 각 스킬에 대한 데미지
    // 피해량 = (데미지 - (상대 방어력 - 플레이어의 방어력 관통력)[최소 0]) / (자신의 마력 - 상대방의 마력)[상대의 마력이 더 강한 경우 곱연산]

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData player;

        [SerializeField]
        private Weapon weapon;

        private float damage;
        public float Damage { get { return damage; } }
        
        private void damageUpdate()
        {
            damage = player.STR;
            if (weapon != null) damage *= weapon.DamagePercent;
        }

        private void Start()
        {
            damageUpdate();
        }

        private void OnStrengthChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "STR")
            {
                damageUpdate();
            }
        }

        public void Attack()
        {
            Debug.Log(damage);
        }
    }
}