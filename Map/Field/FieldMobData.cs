using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldMobData
{
    [Header("출현 몬스터")]
    [SerializeField]
    private List<GameObject> _fieldMonsterObjs;
    public List<GameObject> FieldMonsterObjs
    {
        get { return _fieldMonsterObjs; }
    }
    public List<Monster> FieldMonsters
    {
        get
        {
            return FieldMonsterObjs.Select(obj => obj.GetComponent<Monster>()).ToList();
        }
    }
}