using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.System
{
    public class Blink : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        private Color color;

        private bool isback = false;
        private float accumTime = 0f;

        private const float DELAY = 0.3f;

        void Update()
        {
            accumTime += Time.fixedDeltaTime;
            if(accumTime >= DELAY)
            {
                setA();
                accumTime = 0;
            }
        }

        private void setA()
        {
            color = image.color;

            if (!isback) color.a -= 0.1f;
            else if (isback) color.a += 0.1f;

            if (color.a <= 0 || color.a >= 1) isback = !isback;

            image.color = color;
        }
    }
}