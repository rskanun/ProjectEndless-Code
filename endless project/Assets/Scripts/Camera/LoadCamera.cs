using UnityEngine;

public class LoadCamera : MonoBehaviour
{
    private static LoadCamera _instance;
    public static LoadCamera Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else 
            DestroyImmediate(gameObject);
    }
}