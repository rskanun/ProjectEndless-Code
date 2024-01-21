using System.Collections;
using TMPro;
using UnityEngine;
using static ILoadAnimation;

public class TimePassLoadingAnimation : MonoBehaviour, ILoadAnimation
{
    [Header("관련 오브젝트")]
    [SerializeField] private GameObject timer;
    private TextMeshProUGUI timerText;

    [Header("참조 스크립트")]
    [SerializeField] private GlitchEffect glitch;

    private static TimePassLoadingAnimation _instance;
    public static TimePassLoadingAnimation Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        timerText = timer.GetComponent<TextMeshProUGUI>();
    }

    public void OnLoadAnimation(LoadCallBack listener)
    {
        StartCoroutine(TimePassCoroutine(listener));
    }

    private IEnumerator TimePassCoroutine(LoadCallBack listener)
    {
        yield return new WaitForSeconds(1f);

        // On Start
        WaitForSeconds cooldown = new WaitForSeconds(0.6f);
        timer.SetActive(true);

        // Effect Start
        yield return new WaitForSeconds(3.6f);

        glitch.ActiveEffect(0.3f);

        yield return cooldown;

        glitch.ActiveEffect(0.3f);

        // Time Consume
        ReadOnlyGameData.Instance.time.ConsumeTime();

        yield return cooldown;
        // Effect End

        // On Complete
        timer.SetActive(false);
        listener?.Invoke();
    }
}