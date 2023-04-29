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

        private int fileCount = 0;

        private void OnDisable()
        {
            fileCount = 0;
        }

        public void initSaveFileObj(List<SaveFile> saveFiles)
        {
            foreach(SaveFile saveFile in saveFiles)
            {
                saveFileObjAdd(saveFile);
            }

            // 뷰어 높이 설정
            setViewrSize();
        }

        public void addSaveFileObj(SaveFile saveFile)
        {
            saveFileObjAdd(saveFile);

            // 뷰어 높이 설정
            setViewrSize();
        }

        private void saveFileObjAdd(SaveFile saveFile)
        {
            GameObject saveFileObj = Instantiate(saveFilePrifab, prifabParentTransform);

            // init save file
            saveFile.setObject(saveFileObj);
            saveFile.setCallBack(() => app.rewriteSave(saveFile.Id));

            fileCount++;
        }

        public void reloadSaveFileObj(int index, SaveData data)
        {
            
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
            float height = top + bottom + spacing * fileCount + objHeight * (fileCount + 1);

            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}