using Assets.Script.System.Interface.Menu.App;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu
{
    public class SaveUI : MonoBehaviour
    {
        [Header("프리팹")]
        [SerializeField] private GameObject saveFilePrifab;
        [SerializeField] private Transform prifabParentTransform;
        [Header("오브젝트")]
        [SerializeField] private GameObject saveAddObj;
        [Header("참조 스크립트")]
        [SerializeField] private SaveApp app;


        private List<GameObject> saveFiles = new List<GameObject>();

        private void OnDisable()
        {
        }

        public void setSaveFileObj(Dictionary<string, SaveData> datas)
        {
            float interval = 7.4f;
            float height = saveAddObj.GetComponent<RectTransform>().rect.height;
            Vector2 initPos = saveAddObj.transform.localPosition;

            // 파일 수만큼 오브젝트 추가
            int index = 0;
            foreach(string key in datas.Keys.ToList())
            {
                SaveData data = datas[key];
                float posY = initPos.y + index * (interval + height);


            }
        }

        private void createSaveFile(int index, Vector2 pos, int date, string location, string quest)
        {
            // 세이브 파일 오브젝트 추가
            GameObject saveFile = Instantiate(saveFilePrifab, prifabParentTransform);
            saveFile.transform.localPosition = pos;

            // 세이브 파일 오브젝트 내용 추가
            saveFile.GetComponent<SaveFileUI>().setSaveFile(date, location, quest);

            Button button = saveFile.GetComponent<Button>();
        }

        public void setSaveAddObj()
        {
            
        }
    }
}