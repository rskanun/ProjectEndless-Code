using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Header("이펙트 매니져")]
    [SerializeField] private Effect glitch;

    public void GlitchEffect(float time)
    {
        glitch.ActiveEffect(time);
    }
}