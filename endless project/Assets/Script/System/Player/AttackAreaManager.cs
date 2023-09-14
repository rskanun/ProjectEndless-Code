using Assets.Script.Object.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Player
{
    public class AttackAreaManager : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(Tag.Monster))
            {
                gameObject.GetComponentInParent<AttackManager>().OnNormalDamage(collision);
            }
        }
    }
}