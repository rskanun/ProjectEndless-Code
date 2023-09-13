using Assets.Script.Object.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Player
{
    public class AttackAreaManager : MonoBehaviour
    {
        private List<Collider2D> _attackableMobs = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(Tag.Monster))
            {
                _attackableMobs.Add(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Tag.Monster))
            {
                _attackableMobs.Remove(collision);
            }
        }

        public void OnAttack(float damage, float mp)
        {
            foreach(Collider2D collision in _attackableMobs)
            {
                collision.GetComponent<Monster>().OnTakeDamage(damage, mp);
            }
        }
    }
}