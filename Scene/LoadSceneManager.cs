using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [Header("관련 오브젝트")]
    [SerializeField] private CanvasGroup background;
    [SerializeField] private Material blurMaterial;

    private ILoadAnimation loadingAnimation;

    public static LoadSceneManager _instance;
    public static LoadSceneManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;

            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetAnimation(ILoadAnimation animation)
    {
        loadingAnimation = animation;
    }

    public void OnSceneClosed(string sceneName)
    {
        float delay = 1f;

        DOTween.Sequence()
            .OnStart(() =>
            {
                background.blocksRaycasts = true;

                ControlContext.Instance.NoKeyDown = true;
            })
            .Append(background.DOFade(1, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 10f, delay))
            .OnComplete(() =>
            {
                blurMaterial.SetFloat("_Radius", 0);

                SceneManager.LoadScene(sceneName);
            });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingAnimation?.OnLoadAnimation(() =>
        {
            float delay = 1f;

            DOTween.Sequence()
                .OnStart(() =>
                {
                    blurMaterial.SetFloat("_Radius", 10f);
                })
                .Append(background.DOFade(0, delay))
                .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 0, delay))
                .OnComplete(() =>
                {
                    background.blocksRaycasts = false;

                    ControlContext.Instance.NoKeyDown = false;
                });
        });
    }
}