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

        // 세이브 파일 오브젝트 생성 변수
        private float interval = 7.4f;
        private float height;
        private Vector2 initPos;

        private void OnEnable()
        {
            height = saveAddObj.GetComponent<RectTransform>().rect.height;
            initPos = saveAddObj.transform.position;
        }

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

                GameObject SaveFile = createSaveFile(index, getPos(index), data.date, data.location, data.quest);
                saveFiles.Add(SaveFile);

                index++;
            }

            // 세이프 파일 추가 오브젝트 위치 설정 및 뷰어 높이 설정
            setSaveAddObj();
            setViewrSize();
        }

        public void addSaveFileObj(SaveData data)
        {
            int index = saveFileCount;
            GameObject saveFile = createSaveFile(index, getPos(index), data.date, data.location, data.quest);

            saveFiles.Add(saveFile);

            // 세이프 파일 추가 오브젝트 위치 설정 및 뷰어 높이 설정
            setSaveAddObj();
            setViewrSize();
        }

        private Vector2 getPos(int index)
        {
            // index번째의 세이브 파일 오브젝트 생성 좌표
            float x = initPos.x;
            float y = initPos.y - index * (interval + height);

            return new Vector2(x, y);
        }

        private GameObject createSaveFile(int index, Vector2 pos, int date, string location, string quest)
        {
            // 세이브 파일 오브젝트 추가
            GameObject saveFile = Instantiate(saveFilePrifab, prifabParentTransform);
            saveFile.transform.position = pos;

            // 세이브 파일 오브젝트 내용 추가
            saveFile.GetComponent<SaveFileUI>().setSaveFile(date, location, quest);

            // OnClick 추가
            Button button = saveFile.GetComponent<Button>();
            button.onClick.AddListener(() => app.rewriteSave(index));

            return saveFile;
        }

        private void setSaveAddObj()
        {
            saveAddObj.transform.position = getPos(saveFileCount);
        }

        private void setViewrSize()
        {
            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.rect.height * (saveFileCount + 1));
        }
    }
}