using Assets.Script.Control.Text;
using Assets.Script.UI.Animation;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class SelectUI : MonoBehaviour
    {
        private bool isActive = false;
        public bool IsActive { get { return isActive; } }

        [Header("Game Object")]
        public GameObject darkPanel;
        public GameObject selectionWindow;
        public GameObject selectPrefab;
        [Space]
        [Header("참조 스크립트")]
        public LineManager lineManager;

        private List<GameObject> options = new List<GameObject>();

        public void panelView(bool isView)
        {
            darkPanel.SetActive(isView);
        }

        public void createSelection(List<string> options)
        {
            isActive = true;

            float size = 127f;
            float distance = 10f + selectPrefab.GetComponent<RectTransform>().rect.height;
            float posY = selectPrefab.transform.position.y;

            int count = options.Count;
            if (count > 0)
            {
                posY += (count % 2 == 1) ? (count / 2) * distance : (count / 2) * distance - distance / 2;
                for (int i = 0; i < count; i++)
                {
                    // 선택지 생성 및 텍스트 수정
                    createSelectButton(options[i], new Vector2(0, posY));
                    posY -= distance;
                }

                float height = size + (count - 1) * distance;
                SelectionAnimation.openSelectionAnimation(selectionWindow, this.options, height);
            }
        }

        private void createSelectButton(string name, Vector2 pos)
        {
            // 버튼 오브젝트 추가
            GameObject obj = Instantiate(selectPrefab, selectionWindow.transform);
            obj.transform.localPosition = pos;

            // 텍스트 변경
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = name;

            // 호출함수 추가
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => lineManager.selectCase(name));

            options.Add(obj);
        }

        public void destroySelect()
        {
            isActive = false;
            selectionWindow.SetActive(false);

            foreach (GameObject obj in options)
            {
                Destroy(obj);
            }

            options.Clear();
        }
    }
}