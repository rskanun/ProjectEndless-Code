using Assets.Script.Control.Text.Object;
using Assets.Script.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Text
{
    public class SelectManager : MonoBehaviour
    {
        // 참조 스크립트
        [SerializeField] private SelectUI ui;
        [SerializeField] private TalkManager talkManager;

        public void openSelect(Select select)
        {
            List<string> optionList = select.Options;
            
            foreach(string option in  optionList)
            {
                ui.createButton(option, onButtonClick);
            }

            ui.setView(true);
        }

        private void onButtonClick(string option)
        {
            talkManager.optionSelect(option);
            closeSelect();
        }

        public void closeSelect()
        {
            ui.destroySelect();
            ui.setView(false);
        }
    }
}