using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class SaveUI : MonoBehaviour
    {
        [Header("프리팹")]
        [SerializeField] private GameObject saveFilePrifab;
        [Header("오브젝트 구성요소")]
        [SerializeField] private GameObject saveAddObj;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI contents;


        private void OnEnable()
        {
            initSaveFile();
        }

        public void initSaveFile()
        {
            float initialY = saveAddObj.transform.position.y;
            float height = saveAddObj.GetComponent<RectTransform>().rect.height;
            float interval = 7.4f;


        }
    }
}