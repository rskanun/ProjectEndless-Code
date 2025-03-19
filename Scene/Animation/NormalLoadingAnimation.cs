using System.Collections;
using TMPro;
using UnityEngine;
using static ILoadAnimation;

public class NormalLoadingAnimation : MonoBehaviour, ILoadAnimation
{
    [Header("연관 오브젝트")]
    [SerializeField] private GameObject percentObj;

    private TextMeshProUGUI percent;

    private static NormalLoadingAnimation _instance;
    public static NormalLoadingAnimation Instance
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

    private void OnEnable()
    {

    }

    private void Start()
    {
        percent = percentObj.GetComponent<TextMeshProUGUI>();
    }

    public void OnLoadAnimation(LoadCallBack listener)
    {
        percentObj.SetActive(true);
        percent.text = "Loading...\n" + 0 + "%";

        StartCoroutine(LoadingCoroutine(listener));
    }

    private IEnumerator LoadingCoroutine(LoadCallBack listener)
    {
        float percentage = 0;

        yield return new WaitForSeconds(0.6f);

        while (percentage < 100)
        {
            yield return null;

            percentage += Time.deltaTime * 70;
            percent.text = "Loading...\n" + (int)percentage + "%";

            if (percentage >= 100)
            {
                yield return new WaitForSeconds(0.6f);

                percentObj.SetActive(false);
                listener?.Invoke();
            }
        }
    }
}