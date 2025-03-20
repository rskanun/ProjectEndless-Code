using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockLoadingAnimation : MonoBehaviour, ILoadAnimation
{
    [Header("사용 오브젝트")]
    [SerializeField] private GameObject timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GlitchEffect glitch;

    [Header("참조 스크립트")]
    [SerializeField] private LoadingAnimation loading;

    private bool isLoading;
    private bool isConsumed;

    public void OnPlayAnimation(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, Action completeAction)
    {
        // 이미 로딩중이면 무시
        if (isLoading) return;

        isLoading = true;
        StartCoroutine(LoadingCoroutine(loadScenes, unloadScenes, unloadOptions, completeAction));
        StartCoroutine(TimerBlink());
    }

    private IEnumerator LoadingCoroutine(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOpions, Action completeAction)
    {
        // On Start
        yield return new WaitForSeconds(1f);
        timer.SetActive(true);

        // Effect Start
        yield return new WaitForSeconds(3.6f);

        glitch.ActiveEffect(0.3f);

        yield return new WaitForSeconds(0.6f);

        glitch.ActiveEffect(0.3f);

        // Time Consume
        ReadOnlyGameData.Instance.Time.ConsumeTime();

        yield return new WaitForSeconds(0.6f);
        // Effect End

        // On Complete
        timer.SetActive(false);
    }

    private IEnumerator TimerBlink()
    {
        WaitForSeconds delay = new WaitForSeconds(0.6f);

        while (isLoading)
        {
            timerText.text = time.Hour + ":" + time.Minute + ":" + time.Second;

            yield return delay;

            timerText.text = time.Hour + " " + time.Minute + " " + time.Second;

            yield return delay;
        }
    }
}