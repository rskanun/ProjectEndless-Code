using Endless.GameData;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeBlinkAnimation : MonoBehaviour
{
    [Header("연관 오브젝트")]
    [SerializeField] private TextMeshProUGUI timeText;

    // 코루틴
    private Coroutine blinkCoroutine;

    private void OnEnable()
    {
        RemainTime time = ReadOnlyGameData.Instance.Time;

        blinkCoroutine = StartCoroutine(BlinkAnimation(time));
    }

    private void OnDisable()
    {
        StopCoroutine(blinkCoroutine);
    }

    private IEnumerator BlinkAnimation(RemainTime time)
    {
        WaitForSeconds delay = new WaitForSeconds(0.6f);

        while(true)
        {
            timeText.text = time.Hour + ":" + time.Minute + ":" + time.Second;

            yield return delay;

            timeText.text = time.Hour + " " + time.Minute + " " + time.Second;

            yield return delay;
        }
    }
}