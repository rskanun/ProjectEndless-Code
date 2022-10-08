using UnityEngine;

namespace Assets.Script.Control.Interface
{
    public class Interface : MonoBehaviour
    {
        private int width, height;
        private int x, y;

        public void move(int getX, int getY)
        {
            x = getX; y = getY;

            if (x > width) x = 0;
            else if (x < 0) x = width;
            
            if(y > height) y = 0;
            else if(y < 0) y = height;
        }


    }
}