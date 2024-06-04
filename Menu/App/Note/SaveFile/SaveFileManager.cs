using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SaveFileUI ui;

    public delegate void SaveFileCallBack();
    private event SaveFileCallBack callBack;

    public void SetData(SaveData data)
    {
        string date = data.storyData.date;
        string location = data.mapData.name;
        string quest = data.questData.title;

        ui.SetSaveFile(date, location, quest);
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