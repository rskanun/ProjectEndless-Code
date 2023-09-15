using UnityEditor;
using UnityEngine;

namespace Assets.Script.Object.Monster
{
    public class Monster : MonoBehaviour
    {
        [Header("오브젝트 데이터 값")]
        [SerializeField] private MonsterData data;

        public void OnTakeDamage(int damage, int targetMP)
        {
            float originHP = data.HP;
            float originRP = data.RP;

            data.HP -= damage;

            Debug.Log(damage + " Damage! " + originHP + " -> " + data.HP);
        }
    }
}