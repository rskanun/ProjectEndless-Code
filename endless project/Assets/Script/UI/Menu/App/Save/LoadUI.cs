using Assets.Script.System.Menu;
using Assets.Script.System.Menu.Save;
using Assets.Script.UI.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu.App.Save
{
    public class LoadUI : MonoBehaviour
    {
        [Header("프리팹")]
        [SerializeField] private GameObject saveFilePrifab;
        [SerializeField] private Transform prifabParentTransform;
        [Header("오브젝트")]
        [SerializeField] private GameObject viewer;
        [SerializeField] private GameObject notice;
        [Header("참조 스크립트")]
        [SerializeField] private SaveManager saveManager;

        private int objCount = 0;

        /************************************************************
        * [세이브 파일 추가]
        * 
        * 현재 날짜와 장소, 퀘스트가 담긴 세이브 파일 생성
        ************************************************************/

        public void initSaveFileObj(Dictionary<int, SaveData> saveFiles)
        {
            // 오브젝트 개수 리셋
            objCount = 0;

            // id 값에 따른 오름차순 정렬
            List<int> keys = new List<int>(saveFiles.Keys);
            keys.Sort();

            foreach (int id in keys)
            {
                SaveData data = saveFiles[id];

                saveFileObjAdd(id, data);
            }

            // 비어있음 알림창 설정
            initNotice();

            // 뷰어 높이 설정
            setViewrSize();
        }

        private void saveFileObjAdd(int id, SaveData saveData)
        {
            GameObject saveFileObj = Instantiate(saveFilePrifab, prifabParentTransform);
            SaveFileManager manager = saveFileObj.GetComponent<SaveFileManager>();

            manager.setData(saveData);
            manager.setCallBack(() =>
            {
                MenuManager.Instance.menuClose();

                // 되돌아가는 시간이 과거일 경우 ■■ 발동
                if (OptionSetting.Instance.Date > DateTime.Parse(saveData.date))
                {
                    SecretEffects.Instance.play();
                }

                saveManager.loadData(id);
            });

            objCount++;
        }

        private void initNotice()
        {
            // 세이브 파일이 존재하지 않을 경우 알림창 띄우기
            if(objCount <= 0) notice.SetActive(true);
            else notice.SetActive(false);
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
            float height = top + bottom + spacing * objCount + objHeight * objCount;

            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}