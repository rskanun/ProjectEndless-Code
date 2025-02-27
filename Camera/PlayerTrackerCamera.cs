using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineConfiner2D))]
public class PlayerTrackerCamera : MonoBehaviour
{
    private CinemachineConfiner2D cinemachine;

#if UNITY_EDITOR
    private void OnValidate()
    {
        cinemachine = GetComponent<CinemachineConfiner2D>();
    }
#endif

    public void OnUpdateArea()
    {
        cinemachine.m_BoundingShape2D = MapManager.CurrentArea;
        cinemachine.InvalidateCache();
    }
}
