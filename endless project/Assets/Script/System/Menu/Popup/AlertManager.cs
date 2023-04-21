using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu.Popup
{
    public class AlertManager : MonoBehaviour
    {
        [SerializeField] private GameObject _alertPrefab;
        [SerializeField] private Transform _alertParent;
        
        public GameObject Alert
        {
            get
            {
                return Instantiate(_alertPrefab, _alertParent);
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
}