using System.Collections;
using UnityEngine;

namespace Assets.Script.Control
{
    public class InterfaceControl : MonoBehaviour
    {
        // 현재 선택한 것에 대한 좌표
        private Vector2 select_point = Vector2.zero;

        // 현재 인터페이스 내에서 컨트롤을 하는 중인가
        private bool isInterface = false;


        /************************************************************
        * [Key Value]
        * 
        * 각종 키들의 string을 모아둔 변수
        ************************************************************/

        // 선택키
        private static string select = Option.getKey(Key.select);

        // 취소키
        private static string cancel = Option.getKey(Key.cancel);

        private void initInterface()
        {

        }

        void Update()
        {

        }
    }
}