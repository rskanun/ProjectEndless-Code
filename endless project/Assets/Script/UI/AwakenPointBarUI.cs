using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class AwakenPointBarUI : MonoBehaviour
    {
        private const int AP_COUNT = 5; // AP 갯수
        private const int AP_MAX_STEP = 5; // AP 변화 단계

        [SerializeField]
        private GameObject[] APBar = new GameObject[AP_COUNT];

        protected internal void barUpdate(Player player)
        {
            int apPerValue = player.maxAp / AP_COUNT / (AP_MAX_STEP - 1); // AP의 이미지가 변하는 최소 단위
            int ap = player.ap / apPerValue;

            for(int i = 0; i < ap / AP_MAX_STEP; i++)
            {
                
            }
        }
    }
}