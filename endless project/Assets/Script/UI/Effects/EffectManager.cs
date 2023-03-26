using Assets.Script.UI.Effects;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System
{
    public class EffectManager : MonoBehaviour
    {
        [Space]
        [Header("이펙트 오브젝트")]
        [SerializeField] private GameObject glitchObj;

        private Coroutine glitchCoroutine;

        public void glitchEffect(float time)
        {
            if (glitchCoroutine == null)
            {
                glitchCoroutine = StartCoroutine(glitch(time));
            }
        }

        IEnumerator glitch(float time)
        {
            glitchObj.SetActive(true);
            yield return new WaitForSeconds(time);
            glitchObj.SetActive(false);

            glitchCoroutine = null;
        }
    }
}