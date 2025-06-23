using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [Header("캐릭터 얼굴")]
    [SerializeField] private GameObject face;

    [Header("메뉴 UI")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject appGroup;
    [SerializeField] private GameObject displayUI;
    [SerializeField] private GameObject screenPanel;

    [Header("WiFi")]
    [SerializeField] private GameObject wifiIcon;

    [Header("전파")]
    [SerializeField] private GameObject serviceIcon;
    [SerializeField] private GameObject noServiceIcon;

    [Header("배터리")]
    [SerializeField] private TextMeshProUGUI battery;

    [Header("시계")]
    [SerializeField] private TextMeshProUGUI timeText;

    // 참조 데이터
    private PhoneOptionSetting menuOption;
    private GameData gameData;

    private void Start()
    {
        menuOption = PhoneOptionSetting.Instance;
        gameData = GameData.Instance;
    }

    public Sequence OpenMenu()
    {
        InitUI();

        return MenuAnimation.MenuOpenAnimation(menu, screenPanel, appGroup, face);
    }

    private void InitUI()
    {
        UpdateTime();
        SetWiFi(menuOption.Network);
        SetService(menuOption.Service);
    }

    public Sequence CloseMenu()
    {
        return MenuAnimation.MenuCloseAnimation(menu, screenPanel, face);
    }

    /************************************************************
    * [기타 아이콘 및 설정 조작]
    * 
    * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
    ************************************************************/

    public void SetWiFi(bool isHaving)
    {
        menuOption.Network = isHaving;

        wifiIcon.SetActive(isHaving);
    }

    public void SetService(bool isService)
    {
        menuOption.Service = isService;

        serviceIcon.SetActive(isService);
        noServiceIcon.SetActive(!isService);
    }

    public void SetBattery(int percent)
    {
        battery.text = percent + "%";
    }

    public void UpdateTime()
    {
        int hour = gameData.Time.Hour;
        int min = gameData.Time.Minute;

        string timeTxt = (hour < 12) ? "AM" : "PM";
        timeTxt += " ";
        timeTxt += (hour > 12) ? (hour - 12) : hour;
        timeTxt += ":";
        timeTxt += (min < 10) ? "0" + min : min;

        timeText.text = timeTxt;
    }
}