using UnityEditor;
using UnityEngine;

namespace Assets.Script.Object.Monster
{
    public class Monster : MonoBehaviour
    {
        [Header("오브젝트 데이터 값")]
        [SerializeField] private MonsterData data;

        public void OnTakeDamage(float damage, float targetMP)
        {
            float resultMP = data.MP - targetMP;
            float def = (resultMP < 0) ? 0 : resultMP;

            float totalDamage = damage - def;

            Debug.Log(totalDamage + " Damage!");
        }
    }
}