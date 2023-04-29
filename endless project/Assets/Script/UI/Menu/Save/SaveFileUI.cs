using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu.Save
{
    public class SaveFileUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date;
        [SerializeField] private TextMeshProUGUI contents;

        public delegate void SaveFileCallBack();
        private event SaveFileCallBack callBack;

        public void setSaveFile(string date, string location, string quest)
        {
            this.date.text = DateTime.ParseExact(date, "MMdd", CultureInfo.InvariantCulture).ToString("20XX년 MM월 dd일");

            contents.text = "- 장소 : " + location
                + "\r\n" + "- 메인 퀘스트 : " + quest;
        }

        public void setCallBack(SaveFileCallBack listener)
        {
            callBack = listener;
        }

        public void onClick()
        {
            callBack?.Invoke();
        }
    }
}