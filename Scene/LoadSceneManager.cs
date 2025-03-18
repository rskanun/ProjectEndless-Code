using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneAnimationType
{
    BlurClose,
    NormalSaveDataLoading,
    TimePassSaveDataLoading
}

public class LoadSceneManager
{
    private static LoadSceneManager _instance;
    public static LoadSceneManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LoadSceneManager();

            return _instance;
        }
    }

    private SceneAnimationManager animationManager;

    public void RegisterManager(SceneAnimationManager manager)
    {
        animationManager = manager;
    }

    public void RemoveManager()
    {
        animationManager = null;
    }

    public void LoadBattleScene(SceneAnimationType animationType)
    {
        // 전투 돌입 애니메이션 띄우기
    }

    public void LoadFieldScene(SceneAnimationType startAnimation, SceneAnimationType loadAnimation, SceneAnimationType endAnimation)
    {
        // 전환용 애니메이션이 실행될 씬 열기
        // -> 전환용 애니메이션의 시작 애니메이션 실행
        // -> 


        // 전환 애니메이션 띄우기
        PlayAnimation(startAnimation);
    }

    public void LoadTitleScene(SceneAnimationType animationType)
    {

    }

    private void PlayAnimation(SceneAnimationType animationType, Action completeAction = null)
    {
        // 해당 타입의 애니메이션 실행
        animationManager?.PlayAnimation(animationType, completeAction);
    }
}