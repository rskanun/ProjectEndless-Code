using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private HomeScreenUI homeScreenUI;

    [Header("캐릭터 얼굴")]
    [SerializeField] private GameObject face;

    [Header("메뉴 UI")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject diary;
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

    // 애니메이션 값
    private float diaryDelay = 0.19f;
    private float closeRotate = 70, openRotate = 0;
    private float diaryCloseRotate = 90, diaryOpenRotate = 7;
    private float menuMoveX = 200.0f;
    private float menuRotate = 3.0f;

    // 현재 메뉴 값
    private bool isOpenedMenu;
    private bool isOpenedDiary;

    private void Start()
    {
        menuOption = PhoneOptionSetting.Instance;
        gameData = GameData.Instance;
    }

    /************************************************************
    * [메뉴 제어]
    * 
    * 메뉴의 UI 활성화 여부 및 위치 제어
    ************************************************************/

    public Sequence OpenMenu()
    {
        InitUI();

        return MenuOpenAnimation()
            .AppendCallback(() => isOpenedMenu = true);
    }

    private void InitUI()
    {
        UpdateTime();
        SetWiFi(menuOption.Network);
        SetService(menuOption.Service);

        homeScreenUI.SetAllAppButton(true);
    }

    public Sequence CloseMenu()
    {
        isOpenedMenu = false;

        return MenuCloseAnimation();
    }

    private Sequence MenuOpenAnimation()
    {
        float delay = 0.16f;
        float menuOpenDelay = 0.19f;
        float screenOpenDelay = 0.05f;
        float loadAppDelay = 0.1f;
        float startScale = 10f, resultScale = 1f;

        Vector2 oriPos = face.transform.position;
        Vector3 oriRotate = face.transform.rotation.eulerAngles;

        Image darkPanel = screenPanel.GetComponent<Image>();

        // 휴대폰을 꺼내드는 모션
        Sequence menuOpenSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                face.transform.localRotation = Quaternion.Euler(0, 0, -closeRotate);
                menu.transform.localRotation = Quaternion.Euler(0, 0, closeRotate);
                menu.SetActive(true);

            })
            .Append(menu.transform.DORotate(new Vector3(0, 0, openRotate), menuOpenDelay))
            .Join(DOTween.To(() => 0, x =>
            {
                // 휴대폰이 돌아가는 것과 동일한 속도로 반대 방향으로 돌려서 현 상태 유지시키기
                face.transform.position = oriPos;
                face.transform.rotation = Quaternion.Euler(oriRotate);
            }, 0, menuOpenDelay))
            .SetEase(Ease.OutSine);

        // 화면을 키는 모션
        Sequence turnOnScreenSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                appGroup.transform.localScale = new Vector3(startScale, startScale, 1);
                appGroup.SetActive(true);
                screenPanel.SetActive(true);
            })
            .Append(darkPanel.DOFade(0f, screenOpenDelay))
            .Append(appGroup.transform.DOScale(new Vector3(resultScale, resultScale, 1), loadAppDelay).SetEase(Ease.OutSine));

        return DOTween.Sequence()
            .Append(menuOpenSeq)
            .AppendInterval(delay)
            .Append(turnOnScreenSeq);
    }

    private Sequence MenuCloseAnimation()
    {
        float delay = 0.16f;
        float screenCloseDelay = 0.1f;
        float menuCloseDelay = 0.19f;

        Vector2 oriPos = face.transform.position;
        Vector3 oriRotate = face.transform.rotation.eulerAngles;

        Image darkPanel = screenPanel.GetComponent<Image>();

        // 화면을 끄는 모션
        Sequence turnOffScreenSeq = DOTween.Sequence()
            .Append(darkPanel.DOFade(1f, screenCloseDelay));

        // 휴대폰을 집어넣는 모션
        Sequence menuCloseSeq = DOTween.Sequence()
            .Append(menu.transform.DORotate(new Vector3(0, 0, closeRotate), menuCloseDelay).SetEase(Ease.InQuad))
            .Join(DOTween.To(() => 0, x =>
            {
                // 휴대폰이 돌아가는 것과 동일한 속도로 반대 방향으로 돌려서 현 상태 유지시키기
                face.transform.position = oriPos;
                face.transform.rotation = Quaternion.Euler(oriRotate);
            }, 0, menuCloseDelay))
            .OnComplete(() =>
            {
                menu.SetActive(false);
                menu.transform.localRotation = Quaternion.Euler(0, 0, openRotate);
                face.transform.position = oriPos;
                face.transform.rotation = Quaternion.Euler(oriRotate);
            });

        return DOTween.Sequence()
            .Append(turnOffScreenSeq)
            .AppendInterval(delay)
            .Append(menuCloseSeq);
    }

    /************************************************************
    * [다이어리 제어]
    * 
    * 메뉴의 부가적인 창인 다이어리의 UI 활성화 여부 및 위치 제어
    ************************************************************/

    public Sequence OpenDiary()
    {
        // 다이어리 오픈 애니메이션 구성
        Sequence diarySeq = DOTween.Sequence()
            .Join(MenuMoveAnimation(menu, face))
            .Join(DiaryOpenAnimation(diary))
            .OnKill(() => isOpenedDiary = true);

        // 메뉴가 닫혀있다면, 메뉴부터 먼저 열기
        if (!isOpenedMenu)
            return diarySeq.Insert(0, OpenMenu());

        // 애니메이션 진행
        return diarySeq;
    }

    public Sequence CloseDiary()
    {
        // 다이어리가 열려있지 않다면 애니메이션 재생 X
        if (!isOpenedDiary) return DOTween.Sequence();

        isOpenedDiary = false;

        return DOTween.Sequence()
            .Join(MenuReturnAnimation(menu, face))
            .Join(DiaryCloseAnimation(diary));
    }

    private Sequence DiaryOpenAnimation(GameObject diary)
    {
        // 다이어리 꺼내드는 모션
        return DOTween.Sequence()
            .AppendCallback(() =>
            {
                diary.SetActive(true);
                diary.transform.localRotation = Quaternion.Euler(0, 0, diaryCloseRotate);
            })
            .Append(diary.transform.DORotate(new Vector3(0, 0, diaryOpenRotate), diaryDelay).SetEase(Ease.OutSine));
    }

    private Sequence MenuMoveAnimation(GameObject phone, GameObject face)
    {
        float menuEndX = phone.transform.localPosition.x + menuMoveX;
        float faceEndX = face.transform.localPosition.x - menuMoveX;

        // 휴대폰 화면 옮기는 모션
        return DOTween.Sequence()
            .Join(phone.transform.DOLocalMoveX(menuEndX, diaryDelay))
            .Join(phone.transform.DORotate(new Vector3(0, 0, -menuRotate), diaryDelay).SetEase(Ease.OutSine))
            .Join(face.transform.DOLocalMoveX(faceEndX, diaryDelay)) // 얼굴은 그 자리에 고정
            .Join(face.transform.DORotate(new Vector3(0, 0, menuRotate), diaryDelay).SetEase(Ease.OutSine));
    }

    private Sequence DiaryCloseAnimation(GameObject diary)
    {
        // 다이어리 집어넣는 모션
        return DOTween.Sequence()
            .Append(diary.transform.DORotate(new Vector3(0, 0, diaryCloseRotate), diaryDelay).SetEase(Ease.InQuad))
            .OnKill(() =>
            {
                diary.SetActive(false);
                diary.transform.localRotation = Quaternion.Euler(0, 0, diaryOpenRotate);
            });
    }

    private Sequence MenuReturnAnimation(GameObject phone, GameObject face)
    {
        float menuEndX = phone.transform.localPosition.x - menuMoveX;
        float faceEndX = face.transform.localPosition.x + menuMoveX;

        // 휴대폰 화면 제자리로 돌려놓는 모션
        return DOTween.Sequence()
            .Join(phone.transform.DOLocalMoveX(menuEndX, diaryDelay))
            .Join(phone.transform.DORotate(new Vector3(0, 0, 0), diaryDelay).SetEase(Ease.InQuad))
            .Join(face.transform.DOLocalMoveX(faceEndX, diaryDelay))
            .Join(face.transform.DORotate(new Vector3(0, 0, 0), diaryDelay).SetEase(Ease.InQuad))
            .OnKill(() =>
            {
                phone.transform.localPosition = new Vector3(menuEndX, phone.transform.localPosition.y);
                phone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                face.transform.localPosition = new Vector3(faceEndX, face.transform.localPosition.y);
                face.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            });
    }

    /************************************************************
    * [기타 아이콘 및 설정 조작]
    * 
    * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
    ************************************************************/

    public void EnabledHomeScreen()
    {
        homeScreenUI.EnabledHomeScreen();
    }

    public void DisabledHomeScreen()
    {
        homeScreenUI.DisabledHomeScreen();
    }

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