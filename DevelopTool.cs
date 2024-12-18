using System.Collections;
using TMPro;
using UnityEngine;

public class DevelopTool : MonoBehaviour
{
    public TextMeshProUGUI timeScaleTxt;

    public void Awake()
    {
        StartCoroutine(DevelopUpdate());
    }

    public IEnumerator DevelopUpdate()
    { 
        while(true)
        {
            timeScaleTxt.text = $"Time Scale = {Time.timeScale}";

            yield return null;
        }
    }
}