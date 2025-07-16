using System;
using DG.Tweening;
using UnityEngine;

public class ContactUI : AppUI
{
    // 애니메이션 값
    private float delay = 0.19f;
    private float diaryCloseRotate = 90, diaryOpenRotate = 7;
    private float menuMoveX = 200.0f;
    private float menuRotate = 3.0f;

    [Header("참조 오브젝트")]
    [SerializeField] private GameObject face;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject diary;

    protected override void ActiveApp(Action openHandler)
    {
        // 추가적으로 활성화 여부를 설정
        diary.SetActive(true);
        menu.transform.localPosition = menu.transform.localPosition + new Vector3(menuMoveX, 0);
        menu.transform.localRotation = Quaternion.Euler(0, 0, -menuRotate);
        face.transform.localPosition = face.transform.localPosition - new Vector3(menuMoveX, 0);
        face.transform.localRotation = Quaternion.Euler(0, 0, menuRotate);

        // 기존 활성화 여부 설정
        base.ActiveApp(openHandler);
    }

    protected override void ActiveAppWithAnimation(Action openHandler)
    {
        DOTween.Sequence()
            .Join(MenuAnimation.AppOpenAnimation(window, appBackground, openHandler))
            .Join(MenuMoveAnimation(menu, face))
            .Join(DiaryOpenAnimation(diary))
            .AppendCallback(() => _isOpened = true);
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

    protected override void DeactiveApp()
    {
        // 추가적으로 활성화 여부를 설정
        diary.SetActive(false);
        menu.transform.localPosition = menu.transform.localPosition - new Vector3(menuMoveX, 0);
        menu.transform.localRotation = Quaternion.Euler(0, 0, 0);
        face.transform.localPosition = face.transform.localPosition + new Vector3(menuMoveX, 0);
        face.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // 기존 활성화 여부 설정
        base.DeactiveApp();
    }

    protected override void DeactiveAppWithAnimation()
    {
        _isOpened = false;

        DOTween.Sequence()
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

}