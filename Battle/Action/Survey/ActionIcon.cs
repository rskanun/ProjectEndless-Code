using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour
{
    public Image icon;

    [Header("아이코 대신 쓰일 임시 색")]
    public Color attackColor;
    public Color skillColor;
    public Color itemColor;
    public Color waitColor;
    public Color runColor;
    public Color noneColor;

    public void SetIcon(ActionType type)
    {
        // 아이콘 대신 임시로 색 바꾸기
        icon.color = GetColor(type);
    }

    public void ClearIcon()
    {
        // 아이콘 대신 임시로 색 바꾸기
        icon.color = noneColor;
    }

    private Color GetColor(ActionType type)
    {
        return type switch
        {
            ActionType.Attack => attackColor,
            ActionType.Skill => skillColor,
            ActionType.Item => itemColor,
            ActionType.Wait => waitColor,
            ActionType.Run => runColor,

            _ => noneColor
        };
    }
}