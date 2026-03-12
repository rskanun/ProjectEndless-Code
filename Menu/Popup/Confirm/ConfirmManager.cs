using UnityEngine;

public class ConfirmManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmPrefab;
    [SerializeField] private Transform confirmParent;

    public GameObject Confirm
    {
        get
        {
            return Instantiate(confirmPrefab, confirmParent);
        }
    }

    private static ConfirmManager _instance;
    public static ConfirmManager Instance
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