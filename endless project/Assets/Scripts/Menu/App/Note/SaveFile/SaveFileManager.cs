using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject saveFile;

    [SerializeField]
    private SaveFileUI ui;

    private void OnDisable()
    {
        Destroy(saveFile);
    }

    public void SetData(SaveData data)
    {
        string date = data.storyData.date;
        string location = data.mapData.name;
        string quest = data.questData.title;

        ui.SetSaveFile(date, location, quest);
    }

    public void SetCallBack(SaveFileUI.SaveFileCallBack Listener)
    {
        ui.SetCallBack(Listener);
    }
}