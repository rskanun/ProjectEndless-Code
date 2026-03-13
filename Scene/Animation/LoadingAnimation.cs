using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour, ILoadAnimation
{
    [SerializeField] private GameObject textObj;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Title("Settings")]
    [SerializeField] private float minTime = 2.0f;
    [SerializeField] private float loadingDelay = 0.5f;

    private CancellationTokenSource animCt;
    private readonly string[] loadingStrings =
    {
        "Loading...",
        "Loading",
        "Loading.",
        "Loading..",
    };

    public async UniTask PlayAnimation()
    {
        // 이미 로딩중이면 무시
        if (animCt != null) return;

        // 애니메이션 멈춤 토큰
        var ct = this.GetCancellationTokenOnDestroy();
        animCt = CancellationTokenSource.CreateLinkedTokenSource(ct);

        // 로딩 텍스트바 활성화
        textObj.SetActive(true);

        // 로딩간 실행될 애니메이션 백그라운드에서 재생
        PlayLoadingAnimation(animCt.Token).Forget();

        // 최소 보장 시간 리턴
        await UniTask.Delay(TimeSpan.FromSeconds(minTime), true, cancellationToken: animCt.Token)
            .SuppressCancellationThrow();
    }

    public void StopAnimation()
    {
        // 애니메이션 중지
        animCt?.Cancel();

        // 토근 해제
        animCt?.Dispose();
        animCt = null;

        // 로딩 텍스트바 비활성화
        if (textObj != null)
            textObj.SetActive(false);
    }

    private async UniTask PlayLoadingAnimation(CancellationToken ct)
    {
        int index = 0;
        while (!ct.IsCancellationRequested)
        {
            loadingText.text = loadingStrings[index];
            index = (index + 1) % loadingStrings.Length;

            // 딜레이를 걸며 그 사이에 오브젝트 파괴 여부 감지
            var isCalceled = await UniTask.Delay(TimeSpan.FromSeconds(loadingDelay), true, cancellationToken: ct).SuppressCancellationThrow();

            // ct 호출 시 빠져나오기
            if (isCalceled) break;
        }
    }
}