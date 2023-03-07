using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Item
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "scriptable Object/Item/Weapon")]
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
            set { damagePercent = value; }
        }
    }
}