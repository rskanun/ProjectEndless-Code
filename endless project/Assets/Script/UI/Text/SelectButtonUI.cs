using System.Collections;
using UnityEngine;

namespace Assets.Script.UI.Text
{
    public class SelectButtonUI : MonoBehaviour
    {
        void Start()
        {
            SelectButton setting = SelectOptionSetting.Instance.ButtonSetting;

            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (setting.width, setting.height);
        }
    }
}