using Assets.Script.System.Interface.Menu.App;
using Assets.Script.System.Menu.Save;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private SaveApp app;

        private Dictionary<int, GameObject> saveFileObjDic = new Dictionary<int, GameObject>();

        private void OnDisable()
        {
            saveFileObjDic.Clear();
        }

        public void initSaveFileObj(List<SaveFileData> saveFiles)
        {
            // id 값에 따른 오름차순 정렬
            saveFiles.Sort((a, b) => a.Id.CompareTo(b.Id));

            foreach(SaveFileData saveFile in saveFiles)
            {
                saveFileObjAdd(saveFile);
            }

            // 뷰어 높이 설정
            setViewrSize();
        }

        public void addSaveFileObj(SaveFileData saveFile)
        {
            saveFileObjAdd(saveFile);

            // 뷰어 높이 설정
            setViewrSize();
        }

        private void saveFileObjAdd(SaveFileData saveFile)
        {
            GameObject saveFileObj = Instantiate(saveFilePrifab, prifabParentTransform);
            SaveFileManager manager = saveFileObj.GetComponent<SaveFileManager>();

            manager.setData(saveFile);
            manager.setCallBack(() => app.rewriteSave(saveFile.Id));

            saveFileObjDic[saveFile.Id] = saveFileObj;
        }

        public void reloadSaveFileObj(SaveFileData saveFile)
        {
            GameObject saveFileObj = saveFileObjDic[saveFile.Id];
            SaveFileManager manager = saveFileObj.GetComponent<SaveFileManager>();

            manager.setData(saveFile);
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
            int count = saveFileObjDic.Count;
            float height = top + bottom + spacing * count + objHeight * (count + 1);

            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}