using Assets.Script.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Interface.Menu.App
{
    public class App : MonoBehaviour
    {
        public GameObject window;
        public Stack<GameObject> subWindows = new Stack<GameObject>();

        [Header("참조 스크립트")]
        public MenuUI ui;
        public MenuControl menuCtr;
        public CustomAnimation cusAnimation;

        public void open()
        {
            StartCoroutine(start());
        }

        public virtual IEnumerator start()
        {
            window.SetActive(true);
            yield return null;
        }

        public virtual void close()
        {
            if(subWindows.Count > 0)
            {
                GameObject subWindow = subWindows.Pop();

                subWindow.SetActive(false);
            }
            else
            {
                window.SetActive(false);
            }
        }
    }
}