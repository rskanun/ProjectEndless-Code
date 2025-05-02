using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class BrainCameraManager : MonoBehaviour
{
    private CinemachineBrain brain;

    public void OnEnable()
    {
        if (brain == null)
        {
            brain = gameObject.GetComponent<CinemachineBrain>();
        }

        BattleCameraDirector.Instance.ResiterBrainCamera(brain);
    }
}