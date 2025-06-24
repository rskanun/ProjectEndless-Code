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
    private DateTime time;

    public void OnPlayAnimation(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, Action loadAction, Action completeAction)
    {
        // 이미 로딩중이면 무시
        if (isLoading) return;

        isLoading = true;
        StartCoroutine(LoadingCoroutine(loadScenes, unloadScenes, unloadOptions, loadAction, completeAction));
        StartCoroutine(TimerBlink());
    }

    private IEnumerator LoadingCoroutine(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOpions, Action loadAction, Action completeAction)
    {
        int hour = GameData.Instance.Time.Hour;
        int min = GameData.Instance.Time.Minute;
        int sec = GameData.Instance.Time.Second;

        // 게임 데이터의 시간을 가져와 변환 가능한 DateTime으로 변환
        time = new DateTime(1, 1, 1, hour, min, sec);

        // 데이터 값의 시간 미리 줄여놓기
        GameData.Instance.Time.ConsumeTime();

        // 해당 씬 도중엔 시간이 흐르도록 하기
        Time.timeScale = 1.0f;

        // 잠시 텀을 준 뒤 타이머 띄우기
        yield return new WaitForSecondsRealtime(1f);
        timer.SetActive(true);

        yield return new WaitForSecondsRealtime(3.6f);

        // 화면 전체에 글리치 이펙트
        glitch.ActiveEffect(0.3f);

        yield return new WaitForSecondsRealtime(0.6f);

        // 두 번에 나눠서 화면 전체에 글리치 이펙트
        glitch.ActiveEffect(0.3f);

        // 표시용 시간 줄이기
        time = time.AddSeconds(-1);

        yield return new WaitForSecondsRealtime(0.6f);

        // 연출 이후 타이머 숨기기
        timer.SetActive(false);

        // 텀을 준 뒤 로딩화면 띄우기
        yield return new WaitForSecondsRealtime(0.6f);
        loading.OnPlayAnimation(loadScenes, unloadScenes, unloadOpions, loadAction, completeAction);
    }

    private IEnumerator TimerBlink()
    {
        while (isLoading)
        {
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hour, time.Minute, time.Second);

            yield return new WaitForSecondsRealtime(0.6f);

            timerText.text = string.Format("{0:D2} {1:D2} {2:D2}", time.Hour, time.Minute, time.Second);

            yield return new WaitForSecondsRealtime(0.6f);
        }
    }
}