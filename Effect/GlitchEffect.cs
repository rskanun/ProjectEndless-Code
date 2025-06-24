using Kino;
using UnityEngine;
using UnityEngine.UI;

public class GlitchEffect : Effect
{
    [Header("글리치 에셋 스크립트")]
    [SerializeField] private AnalogGlitch analogGlitch;
    [SerializeField] private DigitalGlitch digitalGlitch;

    public override void SetActive(bool active)
    {
        Debug.Log("Glitch " + active);
        GetComponent<Image>().enabled = active;

        analogGlitch.enabled = active;
        digitalGlitch.enabled = active;
    }
}
