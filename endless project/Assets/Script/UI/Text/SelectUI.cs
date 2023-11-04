using Assets.Script.Text;
using Assets.Script.UI.Animation;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class SelectUI : MonoBehaviour
    {
        private SelectOptionSetting _optionSetting;

        [Header("Game Object")]
        [SerializeField] private GameObject darkPanel;
        [SerializeField] private GameObject selectionWindow;
        [SerializeField] private GameObject selectPrefab;
        [SerializeField] private VerticalLayoutGroup layoutGroup;

        private List<GameObject> options = new List<GameObject>();

        private void Start()
        {
            _optionSetting = SelectOptionSetting.Instance;

            SelectContainer windowSetting = _optionSetting.ContainerSetting;
            
            selectionWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(windowSetting.width, windowSetting.height);
            layoutGroup.spacing = _optionSetting.ContainerSetting.spacing;
        }

        public void setView(bool isView)
        {
            if (isView)
            {
                int index = options.Count;
                float height = _optionSetting.ContainerSetting.height
                        + index * _optionSetting.ButtonSetting.height
                        + (index - 1) * _optionSetting.ContainerSetting.spacing;

                SelectionAnimation.openSelectionAnimation(selectionWindow, options, height);
            }
            else
            {
                selectionWindow.SetActive(false);
            }

            darkPanel.SetActive(isView);
        }

        public void createButton(string option, Action<string> onClickAction)
        {
            // 버튼 오브젝트 추가
            GameObject obj = Instantiate(selectPrefab, selectionWindow.transform);

            // 텍스트 변경
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = option;

            // 호출함수 추가
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => onClickAction.Invoke(option));

            options.Add(obj);
        }

        public void destroySelect()
        {
            foreach (GameObject obj in options)
            {
                Destroy(obj);
            }

            options.Clear();
        }
    }
}