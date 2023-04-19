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
        [SerializeField] private GameObject viewer;
        [Header("참조 스크립트")]
        [SerializeField] private SaveApp app;

        private List<GameObject> saveFiles = new List<GameObject>();
        public int saveFileCount { get { return saveFiles.Count; } }

        private void OnDisable()
        {
            // 세이브 파일 추가 오브젝트 위치 초기로 설정
            if(saveFiles.Count > 0)
                saveAddObj.transform.position = saveFiles[0].transform.position;

            // 프리팹 파괴
            foreach(GameObject saveFile in saveFiles)
            {
                Destroy(saveFile);
            }

            saveFiles.Clear();
        }

        public void initSaveFileObj(Dictionary<string, SaveData> datas)
        {
            // 파일 수만큼 오브젝트 추가
            int index = 0;
            foreach(string key in datas.Keys.ToList())
            {
                SaveData data = datas[key];

                GameObject SaveFile = createSaveFile(index, data.date, data.location, data.quest);
                saveFiles.Add(SaveFile);

                index++;
            }

            // 뷰어 높이 설정
            setViewrSize();
        }

        public void addSaveFileObj(SaveData data)
        {
            int index = saveFileCount;
            GameObject saveFile = createSaveFile(index, data.date, data.location, data.quest);

            saveFiles.Add(saveFile);

            // 뷰어 높이 설정
            setViewrSize();
        }

        private GameObject createSaveFile(int index, int date, string location, string quest)
        {
            // 세이브 파일 오브젝트 추가
            GameObject saveFile = Instantiate(saveFilePrifab, prifabParentTransform);
            saveFile.GetComponent<SaveFileUI>().setSaveFile(date, location, quest);

            // OnClick 추가
            Button button = saveFile.GetComponent<Button>();
            button.onClick.AddListener(() => app.rewriteSave(index));

            return saveFile;
        }

        private void setViewrSize()
        {
            VerticalLayoutGroup component = viewer.GetComponent<VerticalLayoutGroup>();
            float top = component.padding.top;
            float bottom = component.padding.bottom;
            float spacing = component.spacing;

            // 오브젝트 높이
            float objHeight = saveAddObj.GetComponent<RectTransform>().rect.height;

            // 총 길이
            // (세이브 추가 오브젝트 높이 = 세이브 파일 오브젝트 높이 가정)
            float height = top + bottom + spacing * saveFileCount + objHeight * (saveFileCount + 1);

            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}