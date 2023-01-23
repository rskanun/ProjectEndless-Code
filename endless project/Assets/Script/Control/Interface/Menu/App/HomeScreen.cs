using System.Collections;
using UnityEngine;

namespace Assets.Script.Control.Interface.Menu.App
{
    public class HomeScreen : App
    {
        public override void close()
        {
            if(subWindows.Count > 0)
            {
                GameObject subWindow = subWindows.Pop();
            }
        }
    }
}