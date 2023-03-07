using UnityEditor;
using UnityEngine;

namespace Assets.Script.Item
{
    public class Armor : Item
    {
        [SerializeField]
        private int defensive;
        /***************************************************************
        * [ 방어력 (Defensive) ]
        * 
        * 오브젝트의 방어력 수치로 받는 데미지에 영향을 끼친다.
        * 방어력 1당 1의 데미지를 줄인다.
        ****************************************************************/
        public int DFS
        {
            get { return defensive; }
            set { defensive = value; }
        }


    }
}