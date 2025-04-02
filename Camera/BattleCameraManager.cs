using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [Header("시네머신 카메라")]
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera groupCam;
    [SerializeField] private CinemachineVirtualCamera singleCam;

    private CinemachineVirtualCamera liveCam;
    private List<Transform> liveGroup = new List<Transform>();


    private void OnEnable()
    {
        BattleCameraDirector.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        BattleCameraDirector.Instance.RemoveManager();
    }

    /***************************************************************
    * [ 라이브 카메라 ]
    * 
    * 현재 화면을 비출 카메라 설정
    ***************************************************************/

    public void LiveMainCamera()
    {
        // 전체 화면 캠 라이브 시작
        LiveCamera(mainCam);
    }

    public void LiveGroupCamera(List<Transform> transforms)
    {
        // 기존 그룹 지우기
        foreach (Transform transform in liveGroup)
        {
            targetGroup.RemoveMember(transform);
        }

        // 새 그룹 추가
        foreach (Transform transform in transforms)
        {
            targetGroup.AddMember(transform, 1.0f, 1.0f);
            liveGroup.Add(transform);
        }

        // 그룹캠 라이브 시작
        LiveCamera(groupCam);
    }

    public void LiveSingleCamera(Transform transform)
    {
        // 대상 설정
        singleCam.Follow = transform;

        // 단일캠 라이브 시작
        LiveCamera(singleCam);
    }

    private void LiveCamera(CinemachineVirtualCamera liveCamera)
    {
        if (!liveCamera.isActiveAndEnabled) return;

        // 현재 라이브 중인 카메라의 우선도 낮추기
        if (liveCam != null)
            liveCam.Priority = 0;

        // 라이브 할 카메라의 우선도 높이기
        liveCamera.Priority = 1;
        liveCam = liveCamera;
    }
}