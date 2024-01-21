using Assets.Script.System;
using UnityEngine;
using UnityEngine.UI;

public class AwakenPointBarUI : MonoBehaviour
{
    private const int MaxApCount = 5; // AP 갯수
    private const int ApChangeStep = 5; // AP 변화 단계

    private int apImageUnit;

    [SerializeField]
    private Image[] apBar = new Image[MaxApCount];
    public Sprite[] apSteps = new Sprite[ApChangeStep]; // 각각의 변화 단계 이미지

    [Space]
    [Header("참조 스크립트")]
    [SerializeField] private GlitchEffect glitch;

    public void SetApBar(int ap, int maxAp)
    {
        apImageUnit = maxAp / MaxApCount / (ApChangeStep - 1); // AP의 이미지가 변하는 최소 단위

        SetApBar(ap);
    }

    private void SetApBar(int ap)
    {
        int perAP = ap / apImageUnit;

        for (int i = 0; i < MaxApCount; i++)
        {
            // 이미지 변환
            if (perAP >= (ApChangeStep - 1) * (i + 1)) // 해당 자리에서 최대보다 클 경우 MAX값 변환
                apBar[i].sprite = apSteps[ApChangeStep - 1];
            else if (perAP < (ApChangeStep - 1) * i) // 해당 자리에서 최저보다 작을 경우 default값 변환
                apBar[i].sprite = apSteps[0];
            else
                apBar[i].sprite = apSteps[perAP % (ApChangeStep - 1)];
        }
    }

    public void BarUpdate(int ap)
    {
        glitch.ActiveEffect(0.65f);
        SetApBar(ap);
    }
}