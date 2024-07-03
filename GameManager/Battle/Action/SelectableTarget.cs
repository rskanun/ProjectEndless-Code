using UnityEngine;

public class SelectableTarget : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectionManager manager;
    [SerializeField] private Entity entity;

    [Header("선택 애니메이션")]
    [SerializeField] private GameObject selectIcon;

    private bool isSelectable = false;
    public bool isEnemy => entity is Monster;
    public bool isFront => entity.Position == BattlePosition.Front;

    private void OnEnable()
    {
        manager.RegisterListener(this);
    }

    private void OnDisable()
    {
        manager.RemoveListener(this);
    }

    public void SetSelectable(bool isSelectable)
    {
        this.isSelectable = isSelectable;
    }

    public void OnHover()
    {
        if (isSelectable)
        {
            manager.HoverTarget(this);
        }
    }

    public void SelectThis()
    {
        SelectAnimation(true);
    }

    public void SelectCancel()
    {
        SelectAnimation(false);
    }

    private void SelectAnimation(bool active)
    {
        selectIcon.SetActive(active);
    }

    public void OnSelect()
    {
        if (isSelectable)
        {
            manager.OnSelect();
        }
    }
}