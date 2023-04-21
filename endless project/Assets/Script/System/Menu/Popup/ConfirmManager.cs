using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu.Popup
{
    public class ConfirmManager : MonoBehaviour
    {
        [SerializeField] private GameObject _confirmPrefab;
        [SerializeField] private Transform _confirmParent;
        public GameObject Confirm
        {
            get
            {
                return Instantiate(_confirmPrefab, _confirmParent);
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
}