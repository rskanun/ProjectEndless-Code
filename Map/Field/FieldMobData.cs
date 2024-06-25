using System.Collections.Generic;
using UnityEngine;

public class FieldMobData
{
    [Header("출현 몬스터")]
    [SerializeField]
    private List<Monster> fieldMonsters;
    public List<Monster> FieldMonsters
    {
        get { return fieldMonsters; }
    }
}