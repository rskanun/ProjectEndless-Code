using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingAnimation : MonoBehaviour, ILoadAnimation
{
    [Header("사용 오브젝트")]
    [SerializeField] private GameObject textObj;
    [SerializeField] private TextMeshProUGUI loadingText;

    private bool isLoading;
    private float minTime = 2.0f;

    public void OnPlayAnimation(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, Action completeAction)
    {
        // 이미 로딩중이면 무시
        if (isLoading) return;

        StartCoroutine(SceneLoadCoroutine(loadScenes, unloadScenes, unloadOptions, completeAction));
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        int[] dotCounts = { 3, 0, 1, 2 }; // 점 개수 패턴
        int index = 0;
        float delay = 0.5f;

        float timer = 0.0f;
        while (isLoading || timer < minTime)
        {
            loadingText.text = "Loading" + new string('.', dotCounts[index]);
            index = ++index % dotCounts.Length;

            yield return new WaitForSeconds(delay);

            // 딜레이 만큼 경과 시간 추가
            timer += delay;
        }
    }

    private IEnumerator SceneLoadCoroutine(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, Action completeAction)
    {
        isLoading = true;

        // 사용되지 않을 씬 제거
        foreach (string unloadScene in unloadScenes)
        {
            yield return SceneManager.UnloadSceneAsync(unloadScene, unloadOptions);
        }

        // 사용될 씬 불러오기
        foreach (string loadScene in loadScenes)
        {
            yield return SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
        }

        // 로딩 종료
        isLoading = false;
    }
}