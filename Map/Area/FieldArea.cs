using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaData
{
    [ReadOnly]
    public int id;
    public bool isClearArea;

    public AreaData(int id, bool isClearArea)
    {
        this.id = id;
        this.isClearArea = isClearArea;
    }
}

[RequireComponent(typeof(PolygonCollider2D))]
public class FieldArea : MonoBehaviour
{
    [Header("구역 정보")]
    [SerializeField]
    private AreaData areaData;
    public int ID
    {
        get
        {
            if (areaData.id == 0)
                areaData.id = GetInstanceID();

            return areaData.id;
        }
    }
    public bool IsClearArea
    {
        get { return areaData.isClearArea; }
        set
        {
            areaData.isClearArea = value;
            if (IsClearArea)
            {
                SetActiveMonsters(false);
            }
        }
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

    private AreaManager manager;

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

    public AreaData GetAreaData()
    {
        return new AreaData(ID, IsClearArea);
    }

    public void SetActiveMonsters(bool isActive)
    {
        foreach (GameObject mobObj in fieldMonsters)
        {
            if (mobObj != null) mobObj.SetActive(isActive);
        }
    }
}