using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    private Vector2 originSize;

    private List<GameObject> optionList;

    private RectTransform selectionWindowRect;
    private RectTransform selectPrefabRect;

    [Header("Game Object")]
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private GameObject selectionWindow;
    [SerializeField] private GameObject selectPrefab;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    private void Start()
    {
        optionList = new List<GameObject>();

        selectionWindowRect = selectionWindow.GetComponent<RectTransform>();
        selectPrefabRect = selectPrefab.GetComponent<RectTransform>();

        originSize = new Vector2(selectionWindowRect.rect.width, selectionWindowRect.rect.height);
    }

    public void SetView(bool isView)
    {
        if (isView)
        {
            float containerHeight = originSize.y;
            float buttonHeight = selectPrefabRect.rect.height;
            float spacing = layoutGroup.spacing;
            float height = containerHeight + optionList.Count * (buttonHeight + spacing);

            SelectionOpenAnimation(height);
        }

        selectionWindow.SetActive(isView);
        darkPanel.SetActive(isView);
    }

    private void SelectionOpenAnimation(float height)
    {
        float minH = 104;
        float sec = 0.25f;

        RectTransform rect = selectionWindowRect;

        DOTween.Sequence()
            .OnStart(() =>
            {
                rect.sizeDelta = new Vector2(rect.rect.width, minH);
            })
            .Append(rect.DOSizeDelta(new Vector2(rect.rect.width, height), sec).SetEase(Ease.OutCubic))
            .OnComplete(() =>
            {
                foreach (GameObject option in optionList)
                {
                    option.SetActive(true);
                }
            });
    }

    public void CreateButtons(string[] options, Action<string> onClickAction)
    {
        foreach (string option in options)
        {
            // 버튼 오브젝트 추가
            GameObject obj = Instantiate(selectPrefab, selectionWindow.transform);

            // 텍스트 변경
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = option;

            // 호출함수 추가
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => onClickAction.Invoke(option));

            optionList.Add(obj);
        }
    }

    public void DestroySelect()
    {
        foreach (GameObject obj in optionList)
        {
            Destroy(obj);
        }

        optionList.Clear();
    }
}