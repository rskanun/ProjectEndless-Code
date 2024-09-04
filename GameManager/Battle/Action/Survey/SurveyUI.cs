using UnityEngine;

public class SurveyUI : MonoBehaviour
{
    public GameObject linePrefab;
    public Transform container;

    public GameObject CreateArrow(Vector2 actor, Vector2 target)
    {
        GameObject arrow = Instantiate(linePrefab, container);
        DottedArrowLine line = arrow.GetComponent<DottedArrowLine>();

        line.DrawLine(actor, target);

        return arrow;
    }
}