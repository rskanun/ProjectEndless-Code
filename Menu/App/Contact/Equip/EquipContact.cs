using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipContact : Contact
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI statField;
    [SerializeField] private TextMeshProUGUI countField;
    [SerializeField] private GameObject equipMark;
    [Space]
    [SerializeField] private TextMeshProUGUI categoryField;
    [SerializeField] private TextMeshProUGUI descriptionField;

    public void UpdateInfo(Equip equip, int count, bool isEquipped)
    {
        icon.sprite = equip.IconSprite;
        nameField.text = equip.Name;
        statField.text = GetEllipsisText(GetStatDisplay(equip));
        countField.text = count.ToString();
        equipMark.SetActive(isEquipped);

        // 디테일 정보 미리 기입
        categoryField.text = GetCategoryDisplay(equip);
        descriptionField.text = GetDescription(equip);
    }

    private string GetStatDisplay(Equip equip)
    {
        List<string> stats = new List<string>();

        if (equip.STR != 0) stats.Add($"STR {(equip.STR > 0 ? "+" : "-")}{Mathf.Abs(equip.STR)}");
        if (equip.DEF != 0) stats.Add($"DEF {(equip.DEF > 0 ? "+" : "-")}{Mathf.Abs(equip.DEF)}");
        if (equip.AGI != 0) stats.Add($"AGI {(equip.AGI > 0 ? "+" : "-")}{Mathf.Abs(equip.AGI)}");
        if (equip.DEX != 0) stats.Add($"DEX {(equip.DEX > 0 ? "+" : "-")}{Mathf.Abs(equip.DEX)}");

        // 완성된 문장의 너비 초과분 자르기
        return string.Join(" · ", stats);
    }

    private string GetEllipsisText(string text)
    {
        float fieldWidth = statField.GetComponent<RectTransform>().rect.width;
        float textWidth = statField.GetPreferredValues(text).x;

        // 문장이 필드 너비에서 벗어나지 않는다면 그대로 사용
        if (textWidth <= fieldWidth) return text;

        // 범위를 벗어난다면 문장 자르기
        float ellipsisWidth = statField.GetPreferredValues("...").x;

        // 한 글자씩 자르며 너비 체크
        for (int i = 0; i < text.Length; i++)
        {
            string str = text.Substring(0, i + 1);
            float strWidth = statField.GetPreferredValues(str).x;

            // 잘려진 문장이 범위를 초과한다면, 이전 글자까지 자르기
            if (strWidth + ellipsisWidth > fieldWidth)
            {
                return text.Substring(0, i) + "...";
            }
        }

        return "...";
    }

    private string GetCategoryDisplay(Equip equip)
    {
        // 임시
        if (equip is Weapon) return "무기";
        else if (equip is Accessory) return "악세서리";

        return "기타";
    }

    private string GetDescription(Equip equip)
    {
        string description = equip.Description + "\n";

        // 스텟 설명 추가
        description += "\n" + GetStatDisplay(equip);

        // 스킬 설명 추가
        if (equip.Skill != null) description += "\n고유스킬: " + equip.Skill.Name;

        return description;
    }

    /// <summary>
    /// 해당 아이템의 자세한 설명 띄우기
    /// </summary>
    private void ShowDetails()
    {
    }

    private IEnumerator ExpandObject()
    {
        DOTween expandTween = DOTween.Sequence()
            .
    }
}