using Assets.Script.Control.Text.Object;
using Assets.Script.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Text
{
    public class SelectManager : MonoBehaviour
    {
        public bool isActive { get { return ui.IsActive; } }

        // 참조 스크립트
        public SelectUI ui;

        public void openSelect(Select select)
        {
            List<string> list = select.Options;
            ui.createSelection(list);
            ui.panelView(true);
        }

        public void closeSelect()
        {
            ui.destroySelect();
            ui.panelView(false);
        }
    }
}