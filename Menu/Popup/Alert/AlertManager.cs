using UnityEngine;

public class AlertManager : MonoBehaviour
{
    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private Transform alertParent;

    public GameObject Alert
    {
        get
        {
            return Instantiate(alertPrefab, alertParent);
        }
    }

    private static AlertManager _instance;
    public static AlertManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}