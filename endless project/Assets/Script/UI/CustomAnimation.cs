using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets.Script.UI
{
    public class CustomAnimation : MonoBehaviour
    {
        public IEnumerator bigger(GameObject obj, float scale, float time, int frame)
        {
            if (scale > 1)
            {
                float perTime = time / frame;
                WaitForSeconds wait = new WaitForSeconds(perTime);

                float perSize = (scale - 1) / frame;
                float size = perSize;

                for (int timer = 0; timer < frame; timer++)
                {
                    obj.transform.localScale = Vector2.one * (1 + size);
                    size += perSize;

                    yield return wait;
                }
            }
        }

        public IEnumerator moveTo(GameObject obj, Vector2 loc, float time, int frame)
        {
            if (!obj.transform.localPosition.Equals(loc))
            {
                float perTime = time / frame;
                WaitForSeconds wait = new WaitForSeconds(perTime);

                Vector2 nowLocation = obj.transform.localPosition;
                float x = (loc.x - nowLocation.x) / frame;
                float y = (loc.y - nowLocation.y) / frame;

                for(int timer = 0; timer < frame; timer++)
                {
                    nowLocation = obj.transform.localPosition;
                    obj.transform.localPosition = new Vector2(nowLocation.x + x, nowLocation.y + y);

                    yield return wait;
                }
            }
        }
    }
}