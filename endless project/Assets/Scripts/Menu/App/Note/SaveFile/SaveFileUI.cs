using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SaveFileUI : MonoBehaviour
{
    [Header("연관 오브젝트")]
    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI contents;

    public delegate void SaveFileCallBack();
    private event SaveFileCallBack callBack;

    public void SetSaveFile(string date, string location, string quest)
    {
        this.date.text = DateTime.ParseExact(date, "O", CultureInfo.InvariantCulture).ToString("20XX년 MM월 dd일");

        contents.text = "- 장소 : " + location
            + "\r\n" + "- 메인 퀘스트 : " + quest;
    }

    public void SetCallBack(SaveFileCallBack listener)
    {
        callBack = listener;
    }

    public void OnClick()
    {
        callBack?.Invoke();
    }
}