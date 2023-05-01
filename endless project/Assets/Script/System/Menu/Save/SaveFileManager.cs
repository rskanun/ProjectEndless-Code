using Assets.Script.UI.Menu.Save;
using UnityEngine;
using static Assets.Script.UI.Menu.Save.SaveFileUI;

namespace Assets.Script.System.Menu.Save
{
    public class SaveFileManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject saveFile;

        [SerializeField]
        private SaveFileUI ui;

        // 데이터 파일
        private SaveFileData data;

        private void OnDisable()
        {
            Destroy(saveFile);
        }

        public void setData(SaveFileData data)
        {
            this.data = data;

            // 데이터 등록 시, 해당 데이터를 바탕으로 오브젝트 내용 수정
            initObject();
        }

        public void setCallBack(SaveFileCallBack Listener)
        {
            ui.setCallBack(Listener);
        }

        private void initObject()
        {
            SaveData saveData = data.Data;

            ui.setSaveFile(saveData.date, saveData.location, saveData.quest);
        }
    }
}