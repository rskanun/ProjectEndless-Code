using System.Collections.Generic;
using UnityEngine;

public class SurveyEffectUI : MonoBehaviour
{
    [Header("아이콘 생성 위치")]
    public Transform container;

    // 버프 아이콘 목록
    private List<GameObject> tempIconList;

    private void Awake()
    {
        tempIconList = new List<GameObject>();
    }

    public void CreateTempBuffIcon(StatusEffect effect)
    {
        CreateEffectIcon(StatusEffectResource.Instance.buffIcon, effect);
    }

    public void CreateDebuffIcon(StatusEffect effect)
    {
        CreateEffectIcon(StatusEffectResource.Instance.debuffIcon, effect);
    }

    public void CreateEffectIcon(GameObject prefab, StatusEffect effect)
    {
        GameObject tempIcon = CreateIcon(prefab, effect);

        // 임시 아이콘은 반투명한 이미지를 가짐
        StatusEffectIcon icon = tempIcon.GetComponent<StatusEffectIcon>();
        icon.SetAlpha(0.5f);

        // 임시 아이콘 목록에 추가
        tempIconList.Add(tempIcon);
    }

    private GameObject CreateIcon(GameObject prefab, StatusEffect effect)
    {
        GameObject iconObj = Instantiate(prefab, container);

        // 생성된 아이콘을 가장 처음으로 이동
        iconObj.transform.SetAsFirstSibling();

        // 아이콘 이미지 등록
        StatusEffectIcon icon = iconObj.GetComponent<StatusEffectIcon>();
        icon.SetIcon(effect.Icon);

        // 생성된 아이콘 리턴
        return iconObj;
    }

    public void ClearIcons()
    {
        foreach (GameObject tempIcon in tempIconList)
        {
            Destroy(tempIcon);
        }

        tempIconList.Clear();
    }
}