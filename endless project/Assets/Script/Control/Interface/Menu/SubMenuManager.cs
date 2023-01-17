using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.Control.Interface.Menu
{
    public class SubMenuManager : MonoBehaviour
    {
        public GameObject subMenu;

        void Update()
        {
            if (Input.GetKeyDown(Option.cancel))
            {
                subMenu.SetActive(false);
            }
        }
    }
}