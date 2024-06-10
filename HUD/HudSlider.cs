using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Mask))]
public class HudSlider : MonoBehaviour
{
    [Header("슬라이더 정보")]
    [SerializeField] private RectTransform FillTransform;

    [Range(0f, 1f)]
    [SerializeField] 
    private float _value;
    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;

            UpdateSlider();
        }
    }

    // 현재 정보
    private float minX;

    private void Start()
    {
        UpdateSlider();
    }

    private void OnValidate()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (FillTransform != null)
        {
            if (minX == 0)
            {
                // 이미지가 완벽히 안 보이는 지점 설정
                minX = -FillTransform.rect.width;
            }

            Vector3 pos = FillTransform.localPosition;
            pos.x = minX * (1 - Value);

            FillTransform.localPosition = pos;
        }
    }
}