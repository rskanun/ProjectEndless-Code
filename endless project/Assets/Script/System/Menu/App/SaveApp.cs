using Assets.Script.UI.Menu.Popup;
using UnityEngine;

namespace Assets.Script.System.Interface.Menu.App
{
    public class SaveApp : App
    {
        [SerializeField] private ConfirmUI confirm;
        [SerializeField] private SaveManager saveManager;

        public void addSave(int index)
        {
            saveManager.saveData(index);
            saveManager.initSaveFileObj();
        }

        public void rewriteSave(int index)
        {
            confirm.setConfirm("이미 저장된 내용이 있는 파일입니다. 그래도 덮어 씌우시겠습니까?", "계속", "취소");
            confirm.setYesCallBack(() =>
            {
                saveManager.saveData(index);

                confirm.setActive(false);
            });
            confirm.setNoCallBack(() => confirm.setActive(false));

            confirm.setActive(true);
        }
    }
}