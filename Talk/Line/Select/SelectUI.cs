using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    private Vector2 originSize;

    private List<GameObject> optionList;

    private RectTransform selectionWindowRect;
    private RectTransform selectPrefabRect;

    [Header("구성 요소")]
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private GameObject selectionWindow;
    [SerializeField] private GameObject selectPrefab;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    [Header("설정")]
    [SerializeField] private float minHeight = 104;

    private void Awake()
    {
        optionList = new List<GameObject>();

        selectionWindowRect = selectionWindow.GetComponent<RectTransform>();
        selectPrefabRect = selectPrefab.GetComponent<RectTransform>();

        originSize = new Vector2(selectionWindowRect.rect.width, selectionWindowRect.rect.height);
    }

    public void OpenSelection(List<string> options, Action<string> onClickAction)
    {
        // 옵션 개수에 따른 선택창 크기 설정
        float height = GetSelectionHeight(options.Count);
        selectionWindowRect.sizeDelta = new Vector2(selectionWindowRect.rect.width, height);

        // 옵션 만들기
        CreateButtons(options, onClickAction);

        // 선택창 UI 활성화
        selectionWindow.SetActive(true);
        darkPanel.SetActive(true);
    }

    public void CloseSelection()
    {
        // 선택창 UI 비활성화
        selectionWindow.SetActive(false);
        darkPanel.SetActive(false);
    }

    private float GetSelectionHeight(int optionCount)
    {
        float containerHeight = originSize.y;
        float buttonHeight = selectPrefabRect.rect.height;
        float spacing = layoutGroup.spacing;
        float height = containerHeight + optionCount * (buttonHeight + spacing);

        // 만들어질 창의 높이가 최소값을 만족하지 못하면 최소값 리턴
        return Mathf.Max(minHeight, height);
    }

    private void CreateButtons(List<string> options, Action<string> onClickAction)
    {
        bool isSelected = false;
        foreach (string option in options)
        {
            // 버튼 오브젝트 추가
            GameObject obj = Instantiate(selectPrefab, selectionWindow.transform);
            SelectOption selectOption = obj.GetComponent<SelectOption>();

            // 텍스트 변경
            selectOption.SetOption(option);

            // 호출함수 추가
            selectOption.SetHandler(() => onClickAction.Invoke(option));

            // 추후 파괴를 위해 리스트에 추가
            optionList.Add(obj);

            // 아직 선택된 버튼이 없을 경우
            if (isSelected == false)
            {
                // 해당 선택지 버튼을 선택
                isSelected = true;
                EventSystem.current.SetSelectedGameObject(obj);
            }
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