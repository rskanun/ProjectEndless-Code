using Kino;
using UnityEngine;

public class GlitchEffect : Effect
{
    [Header("글리치 이펙트 오브젝트")]
    [SerializeField] private GameObject glitchObj;

    [Header("글리치 에셋 스크립트")]
    [SerializeField] private AnalogGlitch analogGlitch;
    [SerializeField] private DigitalGlitch digitalGlitch;

    public override void SetActive(bool active)
    {
        glitchObj.SetActive(active);

        analogGlitch.enabled = active;
        digitalGlitch.enabled = active;
    }
}
