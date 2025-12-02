using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestroyLine : Line
{
    [SerializeField]
    private string _target;
    public string target => _target;

#if UNITY_EDITOR
    public DestroyLine(DestroyNodeData nodeData) : base(nodeData.guid, LineType.Destroy)
    {
        _target = nodeData.targetGuid;
    }
#endif
}