using Assets.Script.System.Menu.Save;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu.Save
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
        [SerializeField] private SaveManager saveManager;

        private Dictionary<int, GameObject> saveFileObjDic = new Dictionary<int, GameObject>();
        private const int MAX_FILE = 10;

        private void OnDisable()
        {
            saveFileObjDic.Clear();
        }

        /************************************************************
        * [세이브 파일 추가]
        * 
        * 현재 날짜와 장소, 퀘스트가 담긴 세이브 파일 생성
        ************************************************************/

        public void initSaveFileObj(Dictionary<int, SaveData> saveFiles)
        {
            // id 값에 따른 오름차순 정렬
            List<int> keys = new List<int>(saveFiles.Keys);
            keys.Sort();

            foreach(int id in keys)
            {
                SaveData data = saveFiles[id];

                saveFileObjAdd(id, data);
            }

            // 세이브 추가 버튼 설정
            initSaveAddObj();

            // 뷰어 높이 설정
            setViewrSize();
        }

        public void addSaveFileObj(int id, SaveData saveData)
        {
            saveFileObjAdd(id, saveData);

            // 세이브 추가 버튼 설정
            initSaveAddObj();

            // 뷰어 높이 설정
            setViewrSize();
        }

        private void saveFileObjAdd(int id, SaveData saveData)
        {
            GameObject saveFileObj = Instantiate(saveFilePrifab, prifabParentTransform);
            SaveFileManager manager = saveFileObj.GetComponent<SaveFileManager>();

            manager.setData(saveData);
            manager.setCallBack(() => saveManager.rewriteSave(id));

            saveFileObjDic[id] = saveFileObj;
        }

        private void initSaveAddObj()
        {
            // 최대 개수 달성 시 세이브 파일 추가 버튼 삭제
            if (saveFileObjDic.Count >= MAX_FILE)
            {
                saveAddObj.SetActive(false);
            }
            // 최대 개수 이하로 내려오면 다시 세이브 파일 추가 버튼 생성
            else if (saveAddObj.activeSelf == false)
            {
                saveAddObj.SetActive(true);
            }
        }

        private void setViewrSize()
        {
            VerticalLayoutGroup component = viewer.GetComponent<VerticalLayoutGroup>();
            float top = component.padding.top;
            float bottom = component.padding.bottom;
            float spacing = component.spacing;

            // 오브젝트 높이
            float objHeight = saveFilePrifab.GetComponent<RectTransform>().rect.height;

            // 총 길이
            // (세이브 추가 오브젝트 높이 = 세이브 파일 오브젝트 높이 가정)
            int count = (saveAddObj.activeSelf) ? saveFileObjDic.Count + 1 : saveFileObjDic.Count;
            float height = top + bottom + spacing * count + objHeight * count;

            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        /************************************************************
        * [세이브 파일 재설정]
        * 
        * 저장된 세이브 파일의 데이터 갱신
        ************************************************************/

        public void reloadSaveFileObj(int id, SaveData saveData)
        {
            GameObject saveFileObj = saveFileObjDic[id];
            SaveFileManager manager = saveFileObj.GetComponent<SaveFileManager>();

            manager.setData(saveData);
        }
    }
}