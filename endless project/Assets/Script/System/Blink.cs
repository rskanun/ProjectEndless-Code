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

        private const float DELAY = 0.75f;
        private const float FRAME = 0.1f;
        private const float MIN_A = 0.25f;
        private const float MAX_A = 1.0f;

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

            if (!isback) color.a -= FRAME;
            else if (isback) color.a += FRAME;

            if (color.a <= MIN_A || color.a >= MAX_A) isback = !isback;

            image.color = color;
        }
    }
}