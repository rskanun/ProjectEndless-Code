using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class FieldArea : MonoBehaviour
{
    private AreaManager manager;

    [Header("구역 정보")]
    private bool _isClearArea;
    public bool IsClearArea
    {
        get { return _isClearArea; }
        set { _isClearArea = value; }
    }
    private PolygonCollider2D _areaCollider;
    public PolygonCollider2D AreaCollider
    {
        get { return _areaCollider; }
    }
    [SerializeField]
    private List<GameObject> fieldMonsters;
    [SerializeField]
    private BattleFieldData fieldData;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _areaCollider = GetComponent<PolygonCollider2D>();

        if (manager == null)
        {
            manager = transform.GetComponentInParent<AreaManager>();
        }
    }
#endif

    public void OnEnable()
    {
        manager.RegisterArea(this);
    }

    public void OnDisable()
    {
        manager.RemoveArea(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.OnEntedArea(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.OnExitedArea(this);
        }
    }

    public void SetActiveMonsters(bool isActive)
    {
        foreach (GameObject mobObj in fieldMonsters)
        {
            if (mobObj != null) mobObj.SetActive(isActive);
        }
    }
}