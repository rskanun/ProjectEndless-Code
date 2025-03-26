using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    [System.Serializable]
    private class EnemyCamera
    {
        public CinemachineVirtualCamera groupCam;
        public CinemachineTargetGroup groupManager;
        public List<CinemachineVirtualCamera> bodyCams;
    }

    [System.Serializable]
    private class PartyCamera : EnemyCamera
    {
        public List<CinemachineVirtualCamera> selectionCams;
    }

    [Header("시네머신 연출 카메라")]
    [SerializeField]
    private CinemachineVirtualCamera mainCam;

    [SerializeField]
    private PartyCamera partyCamera;
    [SerializeField]
    private EnemyCamera enemyCamera;

    private void OnEnable()
    {
        BattleCameraDirector.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        BattleCameraDirector.Instance.RemoveManager();
    }
}