using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineConfiner2D))]
public class PlayerTrackerCamera : MonoBehaviour
{
    public static PolygonCollider2D cameraArea;
    private CinemachineConfiner2D cinemachine;

#if UNITY_EDITOR
    private void OnValidate()
    {
        cinemachine = GetComponent<CinemachineConfiner2D>();
    }
#endif

    public void OnUpdateArea()
    {
        cinemachine.m_BoundingShape2D = cameraArea;
        cinemachine.InvalidateCache();
    }
}
