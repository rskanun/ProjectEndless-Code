using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ContactUI : AppUI
{
    // 애니메이션 값
    private float delay = 0.19f;
    private float diaryCloseRotate = 90, diaryOpenRotate = 7;
    private float menuMoveX = 200.0f;
    private float menuRotate = 3.0f;

    [SerializeField] private GameObject window;

    [Header("연락처 오브젝트")]
    [SerializeField] private Contact playerContact;
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    [Header("참조 오브젝트")]
    [SerializeField] private GameObject appBackground;
    [SerializeField] private GameObject face;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject diary;

    [Header("참조 스크립트")]
    [SerializeField] private HomeScreenUI homeScreenUI;

    public void SetPlayerContact(PlayerData player)
    {
        playerContact.SetInfo(player);
    }

    public Contact CreateContact(CharacterData character)
    {
        GameObject contactObj = Instantiate(contactPrefab, contactTrans);
        Contact contact = contactObj.GetComponent<Contact>();

        contact.SetInfo(character);

        return contact;
    }

    protected override Sequence AppCloseAnimation(bool isPlayAnimation)
    {
        homeScreenUI.EnabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation)
        {
            // 애니메이션 스킵
            menu.transform.localPosition = menu.transform.localPosition - new Vector3(menuMoveX, 0);
            menu.transform.localRotation = Quaternion.Euler(0, 0, 0);
            face.transform.localPosition = face.transform.localPosition + new Vector3(menuMoveX, 0);
            face.transform.localRotation = Quaternion.Euler(0, 0, 0);

            diary.SetActive(false);
            window.SetActive(false);
            appBackground.SetActive(false);

            return DOTween.Sequence();
        }

        return DOTween.Sequence()
            .Join(MenuAnimation.AppCloseAnimation(window, appBackground))
            .Join(MenuReturnAnimation(menu, face))
            .Join(DiaryCloseAnimation(diary));
    }

    private Sequence MenuReturnAnimation(GameObject phone, GameObject face)
    {
        float menuEndX = phone.transform.localPosition.x - menuMoveX;
        float faceEndX = face.transform.localPosition.x + menuMoveX;

        // 휴대폰 화면 제자리로 돌려놓는 모션
        return DOTween.Sequence()
            .Join(phone.transform.DOLocalMoveX(menuEndX, delay))
            .Join(phone.transform.DORotate(new Vector3(0, 0, 0), delay).SetEase(Ease.InQuad))
            .Join(face.transform.DOLocalMoveX(faceEndX, delay))
            .Join(face.transform.DORotate(new Vector3(0, 0, 0), delay).SetEase(Ease.InQuad));
    }

    private Sequence DiaryCloseAnimation(GameObject diary)
    {
        // 다이어리 집어넣는 모션
        return DOTween.Sequence()
            .Append(diary.transform.DORotate(new Vector3(0, 0, diaryCloseRotate), delay).SetEase(Ease.InQuad))
            .OnComplete(() =>
            {
                diary.SetActive(false);
                diary.transform.localRotation = Quaternion.Euler(0, 0, diaryOpenRotate);
            });
    }

    protected override Sequence AppOpenAnimation(bool isPlayAnimation)
    {
        homeScreenUI.DisabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation)
        {
            // 애니메이션 스킵
            menu.transform.localPosition = menu.transform.localPosition + new Vector3(menuMoveX, 0);
            menu.transform.localRotation = Quaternion.Euler(0, 0, -menuRotate);
            face.transform.localPosition = face.transform.localPosition - new Vector3(menuMoveX, 0);
            face.transform.localRotation = Quaternion.Euler(0, 0, menuRotate);

            diary.SetActive(true);
            window.SetActive(true);
            appBackground.SetActive(true);

            return DOTween.Sequence();
        }

        return DOTween.Sequence()
            .Join(MenuAnimation.AppOpenAnimation(window, appBackground))
            .Join(MenuMoveAnimation(menu, face))
            .Join(DiaryOpenAnimation(diary));
    }

    private Sequence MenuMoveAnimation(GameObject phone, GameObject face)
    {
        float menuEndX = phone.transform.localPosition.x + menuMoveX;
        float faceEndX = face.transform.localPosition.x - menuMoveX;

        // 휴대폰 화면 옮기는 모션
        return DOTween.Sequence()
            .Join(phone.transform.DOLocalMoveX(menuEndX, delay))
            .Join(phone.transform.DORotate(new Vector3(0, 0, -menuRotate), delay).SetEase(Ease.OutSine))
            .Join(face.transform.DOLocalMoveX(faceEndX, delay))
            .Join(face.transform.DORotate(new Vector3(0, 0, menuRotate), delay).SetEase(Ease.OutSine));
    }

    private Sequence DiaryOpenAnimation(GameObject diary)
    {
        // 다이어리 꺼내드는 모션
        return DOTween.Sequence()
            .OnStart(() =>
            {
                diary.SetActive(true);
                diary.transform.localRotation = Quaternion.Euler(0, 0, diaryCloseRotate);
            })
            .Append(diary.transform.DORotate(new Vector3(0, 0, diaryOpenRotate), delay).SetEase(Ease.OutSine));
    }
}