using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldMobData
{
    [SerializeField]
    private List<GameObject> _fieldMonsters;
    public List<GameObject> FieldMonsters
    {
        get { return _fieldMonsters; }
    }
}