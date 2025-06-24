using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class FieldArea : MonoBehaviour
{
    [Header("구역 정보")]
    [ReadOnly, SerializeField]
    private int _id;
    public int ID
    {
        get
        {
#if UNITY_EDITOR
            if (_id == 0)
                CreateID();
#endif
            return _id;
        }
    }
    [SerializeField]
    private bool _isClearArea;
    public bool IsClearArea
    {
        get { return _isClearArea; }
        set { _isClearArea = value; }
    }
    private PolygonCollider2D _areaCollider;
    public PolygonCollider2D AreaCollider => _areaCollider;
    [SerializeField]
    private List<GameObject> fieldMonsters;
    [SerializeField]
    private BattleFieldData _fieldData;
    public BattleFieldData FieldData => _fieldData;

    private AreaManager manager;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _areaCollider = GetComponent<PolygonCollider2D>();

        if (manager == null)
        {
            manager = transform.GetComponentInParent<AreaManager>();
        }

        // ID값 할당
        CreateID();
    }

    private void CreateID()
    {
        if (_id == 0)
            _id = GetInstanceID();
    }
#endif

    private void OnEnable()
    {
        manager.RegisterArea(this);
    }

    private void OnDisable()
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