using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [Header("아이콘 생성 위치")]
    public Transform container;

    // 버프 아이콘 목록
    private Dictionary<StatusEffect, GameObject> effectIconDic;

    private void Awake()
    {
        effectIconDic = new Dictionary<StatusEffect, GameObject>();
    }

    public void CreateBuffIcon(StatusEffect buff)
    {
        CreateEffectIcon(StatusEffectResource.Instance.buffIcon, buff);
    }

    public void CreateDebuffIcon(StatusEffect debuff)
    {
        CreateEffectIcon(StatusEffectResource.Instance.debuffIcon, debuff);
    }

    private void CreateEffectIcon(GameObject prefab, StatusEffect effect)
    {
        // 아이콘 생성
        GameObject iconObj = CreateIcon(prefab, effect);

        // 해당 아이콘을 목록에 저장
        effectIconDic.Add(effect, iconObj);
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

    public void DeleteIcon(StatusEffect effect)
    {
        GameObject deleteObj = effectIconDic[effect];

        effectIconDic.Remove(effect);
        Destroy(deleteObj);
    }

    public void HideIcon(StatusEffect effect)
    {
        GameObject hideObj = effectIconDic[effect];

        hideObj.SetActive(false);
    }

    public void ViewIcon(StatusEffect effect)
    {
        GameObject hideObj = effectIconDic[effect];

        hideObj.SetActive(true);
    }

}