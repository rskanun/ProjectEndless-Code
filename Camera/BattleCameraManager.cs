using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    [System.Serializable]
    private class EnemyCamera
    {
        public CinemachineVirtualCamera groupCam;
        public CinemachineTargetGroup groupManager;
        public List<GameObject> camObjs;
        public List<CinemachineVirtualCamera> bodyCams;
    }

    [System.Serializable]
    private class PlayerCamera : EnemyCamera
    {
        public List<CinemachineVirtualCamera> selectionCams;
    }

    [Header("시네머신 연출 카메라")]
    [SerializeField]
    private CinemachineVirtualCamera mainCam;

    [SerializeField]
    private PlayerCamera playerCamera;
    private int playerCamNum;
    private int selectionCamNum;

    [SerializeField]
    private EnemyCamera enemyCamera;
    private int enemyCamNum;


    private Dictionary<int, CinemachineVirtualCamera> bodyCamMap;
    private Dictionary<int, CinemachineVirtualCamera> selectionCamMap;

    private CinemachineVirtualCamera currentLiveCamera;

    private void OnEnable()
    {
        BattleCameraDirector.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        BattleCameraDirector.Instance.RemoveManager();
    }

    public void RegisterCameraToPlayerParty(List<Character> party)
    {
        bodyCamMap = new Dictionary<int, CinemachineVirtualCamera>();
        selectionCamMap = new Dictionary<int, CinemachineVirtualCamera>();

        foreach (Character chr in party)
        {
            playerCamera.groupManager.AddMember(chr.cameraOption.BodyPivot, 1.0f, 1.0f);

            playerCamera.camObjs[playerCamNum].SetActive(true);

            bodyCamMap.Add(chr.GetInstanceID(), RegisterPlayerCamera(chr.cameraOption.BodyPivot));
            selectionCamMap.Add(chr.GetInstanceID(), RegisterSelectionCamera(chr.cameraOption.SelectionPivot));
        }
    }

    private CinemachineVirtualCamera RegisterPlayerCamera(Transform transform)
    {
        if (playerCamNum >= playerCamera.bodyCams.Count)
        {
            // 준비된 카메라보다 많이 등록하려면 빈 카메라 리턴
            return null;
        }

        CinemachineVirtualCamera registCam = playerCamera.bodyCams[playerCamNum++];

        registCam.Follow = transform;
        return registCam;
    }

    private CinemachineVirtualCamera RegisterSelectionCamera(Transform transform)
    {
        if (selectionCamNum >= playerCamera.selectionCams.Count)
        {
            // 준비된 카메라보다 많이 등록하려면 빈 카메라 리턴
            return null;
        }

        CinemachineVirtualCamera registCam = playerCamera.selectionCams[selectionCamNum++];

        registCam.Follow = transform;
        return registCam;
    }

    public void RegisterCameraToEnemyParty(List<Monster> party)
    {
        bodyCamMap = new Dictionary<int, CinemachineVirtualCamera>();

        foreach (Monster mob in party)
        {
            enemyCamera.groupManager.AddMember(mob.cameraOption.BodyPivot, 1.0f, 1.0f);

            enemyCamera.camObjs[enemyCamNum].SetActive(true);

            bodyCamMap.Add(mob.GetInstanceID(), RegisterEnemyCamera(mob.cameraOption.BodyPivot));
        }
    }

    private CinemachineVirtualCamera RegisterEnemyCamera(Transform transform)
    {
        if (enemyCamNum >= enemyCamera.bodyCams.Count)
        {
            // 준비된 카메라보다 많이 등록하려면 빈 카메라 리턴
            return null;
        }

        CinemachineVirtualCamera registCam = enemyCamera.bodyCams[enemyCamNum++];

        registCam.Follow = transform;
        return registCam;
    }

    /***************************************************************
    * [ 라이브 카메라 ]
    * 
    * 현재 화면을 비출 카메라 설정
    ***************************************************************/

    public void LiveMainCamera()
    {
        LiveCamera(mainCam);
    }

    public void LivePlayerPartyCamera()
    {
        LiveCamera(playerCamera.groupCam);
    }

    public void LiveEnemyPartyCamera()
    {
        LiveCamera(enemyCamera.groupCam);
    }

    public void LiveCharacterCamera(int instanceID)
    {
        if (!bodyCamMap.ContainsKey(instanceID)) return;

        LiveCamera(bodyCamMap[instanceID]);
    }

    public void LiveSelectionCamera(int instanceID)
    {
        if (!selectionCamMap.ContainsKey(instanceID)) return;

        LiveCamera(selectionCamMap[instanceID]);
    }

    private void LiveCamera(CinemachineVirtualCamera liveCamera)
    {
        if (!liveCamera.isActiveAndEnabled) return;

        // 현재 라이브 중인 카메라의 우선도 낮추기
        if (currentLiveCamera != null)
            currentLiveCamera.Priority = 0;

        // 라이브 할 카메라의 우선도 높이기
        liveCamera.Priority = 1;
        currentLiveCamera = liveCamera;
    }
}