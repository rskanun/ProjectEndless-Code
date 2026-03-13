using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

    private CancellationTokenSource animCt;
    private DateTime time;

    public async UniTask PlayAnimation()
    {
        // 이미 로딩중이면 무시
        if (animCt != null) return;

        // 토큰 발행
        var ct = this.GetCancellationTokenOnDestroy();
        animCt = CancellationTokenSource.CreateLinkedTokenSource(ct);

        // 로딩 애니메이션 실행
        await TimerEventAnimation(animCt.Token).SuppressCancellationThrow();
    }

    public void StopAnimation()
    {
        // 애니메이션 중지
        loading.StopAnimation();
        animCt?.Cancel();

        // 토큰 해제
        animCt?.Dispose();
        animCt = null;
    }

    private async UniTask TimerEventAnimation(CancellationToken ct)
    {
        int hour = GameData.Instance.Time.Hour;
        int min = GameData.Instance.Time.Minute;
        int sec = GameData.Instance.Time.Second;

        // 시계 점멸 애니메이션은 백그라운드로 실행
        TimerBlink(ct).Forget();

        // 게임 데이터의 시간을 가져와 변환 가능한 DateTime으로 변환
        time = new DateTime(1, 1, 1, hour, min, sec);

        // 데이터 값의 시간 미리 줄여놓기
        GameData.Instance.Time.ConsumeTime();

        // 해당 씬 도중엔 시간이 흐르도록 하기
        Time.timeScale = 1.0f;

        // 애니메이션 실행 부분
        try
        {
            // 잠시 텀을 준 뒤 타이머 띄우기
            await UniTask.Delay(TimeSpan.FromSeconds(1f), true, cancellationToken: ct);
            timer.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(3.6f), true, cancellationToken: ct);

            // 화면 전체에 글리치 이펙트
            glitch.ActiveEffect(0.3f);

            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), true, cancellationToken: ct);

            // 두 번에 나눠서 화면 전체에 글리치 이펙트
            glitch.ActiveEffect(0.3f);

            // 표시용 시간 줄이기
            time = time.AddSeconds(-1);

            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), true, cancellationToken: ct);

            // 연출 이후 타이머 숨기기
            timer.SetActive(false);

            // 텀을 준 뒤 로딩화면 띄우기
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), true, cancellationToken: ct);
            await loading.PlayAnimation();
        }
        finally
        {
            // 타이머 끄기
            if (timer != null)
                timer.SetActive(false);
        }
    }

    private async UniTask TimerBlink(CancellationToken ct)
    {
        bool showColon = true;
        while (!ct.IsCancellationRequested)
        {
            string format = showColon ? "{0:D2}:{1:D2}:{2:D2}" : "{0:D2} {1:D2} {2:D2}";
            timerText.text = string.Format(format, time.Hour, time.Minute, time.Second);

            // 콜론 상태 반전
            showColon = !showColon;

            // 딜레이를 걸며 그 사이에 오브젝트 파괴 여부 감지
            bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(0.6f), true, cancellationToken: ct).SuppressCancellationThrow();

            // ct 호출 시 빠져나오기
            if (isCanceled) break;
        }
    }
}