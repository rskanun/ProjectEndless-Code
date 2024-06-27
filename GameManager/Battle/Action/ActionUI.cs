using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [Header("행동 선택창")]
    [SerializeField] private GameObject actionWindow;

    [Header("초기 선택 버튼")]
    [SerializeField] private Button firstButton;

    private void OnEnable()
    {
        firstButton.Select();
    }

    public void ActiveSelection(bool active)
    {
        actionWindow.SetActive(active);
    }

    public void OnSelectEnemy()
    {

    }

    public void OnSelectMember()
    {

    }

    public void OnSelectTarget()
    {

    }

    public void CloseTargetSelection()
    {

    }
}