using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRatio : MonoBehaviour
{
    void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        int width = 1920;
        int height = 1080;

        int deviceWidth = Screen.width; // 기기의 너비
        int deviceHeight = Screen.height; // 기기의 높이

        float ratio = width / height; // 게임 해상도
        float deviceRatio = deviceWidth / deviceHeight; // 기기 해상도

        Screen.SetResolution(width, (int)(width / deviceRatio), true);

        if (ratio < deviceRatio) // 기기의 해상도가 더 큰 경우
        {
            float newWidth = ratio / deviceRatio;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }

        else // 게임의 해상도가 더 큰 경우
        {
            float newHeight = deviceRatio / ratio;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
