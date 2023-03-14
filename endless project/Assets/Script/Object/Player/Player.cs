using Assets.Script.Item;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Script.Object.Player
{
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