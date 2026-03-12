using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TranslucentObject : MonoBehaviour
{
    private Tilemap tilemap;
    private Coroutine colorChange;

    // 알파값 변화
    private float targetAlpha;
    private float delay = 0.05f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        tilemap = GetComponent<Tilemap>();
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetAlpha = 0.2f;

            if (colorChange == null)
            {
                colorChange = StartCoroutine(ChangedAlpha());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetAlpha = 1.0f;

            if (colorChange == null)
            {
                colorChange = StartCoroutine(ChangedAlpha());
            }
        }
    }

    private IEnumerator ChangedAlpha()
    {
        while (tilemap.color.a != targetAlpha)
        {
            float a = tilemap.color.a;
            float unit = Mathf.Min(0.05f, Mathf.Abs(targetAlpha - a));

            if (a > targetAlpha) SetAlpha(a - unit);
            else if (a < targetAlpha) SetAlpha(a + unit);

            yield return new WaitForSeconds(delay);
        }

        colorChange = null;
    }

    private void SetAlpha(float a)
    {
        Color color = tilemap.color;
        color.a = a;
        tilemap.color = color;
    }
}