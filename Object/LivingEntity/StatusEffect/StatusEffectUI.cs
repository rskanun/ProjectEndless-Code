using UnityEngine;

public class StatusEffectUI : MonoBehaviour
{
    [Header("버프 & 디버프 아이콘")]
    public GameObject buffPrefab;
    public GameObject debuffPrefab;

    [Header("아이콘 생성 위치")]
    public Transform container;

    public void CreateBuffIcon(Buff buff)
    {
        CreateEffectIcon(buffPrefab, buff);
    }

    public void CreateDebuffIcon(Debuff debuff)
    {
        CreateEffectIcon(debuffPrefab, debuff);
    }

    private void CreateEffectIcon(GameObject prefab, StatusEffect effect)
    {
        GameObject iconObj = Instantiate(prefab, container);

        // 아이콘 정보 등록
        StatusEffectIcon icon = iconObj.GetComponent<StatusEffectIcon>();
        icon.SetEffect(effect);
    }
}