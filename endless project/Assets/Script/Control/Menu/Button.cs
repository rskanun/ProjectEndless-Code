using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    abstract public class Button : MonoBehaviour
    {
        [SerializeField]
        private GameObject selectIcon;

        [SerializeField]
        private GameObject buttonIcon;

        // 마우스 해당 버튼을 클릭했을 경우 작동되는 이벤트
        public abstract void onButton();

        private void OnMouseEnter()
        {
            selectIcon.transform.localPosition = buttonIcon.transform.localPosition;
        }
    }
}