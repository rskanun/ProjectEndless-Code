using System;
using System.Collections;
using UnityEngine;

public class MessengerApp : App
{
    [Header("참조 스크립트")]
    [SerializeField] private MenuManager menuManager;

    private PhoneOptionSetting setting;
    private Coroutine networkChecking;

    private void Start()
    {
        setting = PhoneOptionSetting.Instance;
    }

    protected override void LoadData()
    {
        if (setting.Network == true)
        {
            // load messenger data

            OpenMainScreen();
        }
        else
        {
            // 네트워크 체크
            if (networkChecking != null)
                StopCoroutine(networkChecking);

            networkChecking = StartCoroutine(CheckingNetwork());
        }
    }

    private IEnumerator CheckingNetwork()
    {
        // 로딩 시간
        yield return new WaitForSeconds(1.0f);

        if (setting.Network == false)
        {
            Alert.CreateMsg("네트워크 상태가 원활하지 않습니다. 네트워크를 연결한 후 다시 접속해주세요.")
            .SetOkHandler(() =>
            {
                menuManager.CloseApp();
            }).Show();
        }
        else OpenMainScreen();

        networkChecking = null;
    }

    private void OpenMainScreen()
    {
        // 홈 화면 출력
    }

    public override void Close(bool isPlayAnimation)
    {
        if (networkChecking != null)
            StopCoroutine(networkChecking);

        base.Close(isPlayAnimation);
    }
}