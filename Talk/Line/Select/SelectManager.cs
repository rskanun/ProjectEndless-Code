using System;
using System.Linq;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectUI ui;

    private bool _isSelectOpen;
    public bool IsSelectOpen
    {
        private set { _isSelectOpen = value; }
        get { return _isSelectOpen; }
    }

    public void OpenSelect(SelectLine select, Action<int> onSelect)
    {
        IsSelectOpen = true;

        // 선택창 활성화
        var options = select.options.ToList();
        ui.OpenSelection(options, (option) =>
        {
            // 선택 시 해당 옵션과 이어진 대사 선택
            onSelect?.Invoke(options.IndexOf(option));

            // 선택 후 창 닫기
            CloseSelect();
        });
    }

    public void CloseSelect()
    {
        ui.CloseSelection();
        ui.DestroySelect();

        IsSelectOpen = false;
    }
}