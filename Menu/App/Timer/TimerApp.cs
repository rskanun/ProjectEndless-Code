using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerApp : App
{
    [SerializeField] private MenuManager menuManager;

    private TimerUI timerUI;
    private Endless.GameData.Time currentTime;

    private void OnValidate()
    {
        if (ui == null || ui is not TimerUI timerUI) return;

        this.timerUI = timerUI;
    }

    public void UpdateTimer()
    {
        currentTime = timerUI.GetTime();

        // 시간 변화에 따른 UI 업데이트
        timerUI.SetRemainTime(GameData.Instance.RespiteTime - currentTime);
        timerUI.SetTimeRange(GameData.Instance.RespiteTime);
        UpdateDetailInfo();
    }

    private void UpdateDetailInfo()
    {
        if (currentTime.TotalSeconds == 0)
        {
            // 회복량 숨기기
            timerUI.HideRegenAmount();
        }
        else
        {
            OptionData option = OptionData.Instance;

            // sp, hp 계산
            int hp = option.RotaryRegenHP * currentTime.TotalSeconds / 600;
            int sp = option.RotaryRegenSP * currentTime.TotalSeconds / 600;

            // 회복량 띄우기
            timerUI.ShowRegenAmount();
            timerUI.SetRegenAmount(hp, sp);
        }
    }

    public void RotaryStart()
    {
        // 휴식 시간이 설정되지 않았다면 무시
        if (currentTime.TotalSeconds == 0) return;

        // 메뉴 닫기
        menuManager.CloseMenu();

        // 임시 화면 전환
        SceneLoadManager.loadingCallBack += () => RegenStat();
        SceneLoadManager.Instance.LoadFieldScene(GameData.Instance.MapData.SceneName, UnloadSceneOptions.None, SceneFadeEffect.BlurFadeOut, SceneFadeEffect.BlurFadeIn, LoadingScreen.Loading);
    }

    private void RegenStat()
    {
        OptionData option = OptionData.Instance;
        float hpPercent = option.RotaryRegenHP * currentTime.TotalSeconds / 600 / 100.0f;
        float spPercent = option.RotaryRegenSP * currentTime.TotalSeconds / 600 / 100.0f;

        foreach (CharacterData character in PartyData.Instance.Characters)
        {
            // 해금되지 않은 캐릭터면 무시
            if (!character.IsUnlocked) return;

            // 해당 캐릭터의 HP와 SP 일정 회복
            character.Stat.HP += Mathf.RoundToInt(character.Stat.MaxHP * hpPercent);
            character.Stat.SP += Mathf.RoundToInt(character.Stat.MaxSP * spPercent);
        }
    }

    protected override void OnOpen()
    {
        currentTime = new Endless.GameData.Time(0);

        // 타이머 UI 업데이트
        timerUI.SetTime(currentTime);
        timerUI.SetRemainTime(GameData.Instance.RespiteTime);
        timerUI.SetTimeRange(GameData.Instance.RespiteTime);
        UpdateDetailInfo();

        // 타이머 선택
        timerUI.SelectFirstTimer();
    }
}