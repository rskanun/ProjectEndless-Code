using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class SaveFileUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date;
        [SerializeField] private TextMeshProUGUI contents;

        public void setSaveFile(int date, string location, string quest)
        {
            this.date.text = $"20XX년 {date / 100:D2}월 {date % 100:D2}일";

            contents.text = "- 장소 : " + location
                + "\r\n" + "- 메인 퀘스트 : " + quest;
        }
    }
}