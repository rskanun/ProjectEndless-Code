using UnityEngine;

public class TurnSelectionUI : MonoBehaviour
{
    public GameObject insertIcon;

    public void SetActiveInsertIcon(bool isActive)
    {
        insertIcon.SetActive(isActive);
    }

    public void SetInsertIconImage(GameObject actor)
    {
        InsertIcon script = insertIcon.GetComponent<InsertIcon>();

        script.SetImage(actor);
    }

    public void SetSiblingIcon(int index)
    {
        insertIcon.transform.SetSiblingIndex(index);
    }
}