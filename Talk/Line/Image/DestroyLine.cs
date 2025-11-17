using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestroyLine : Line
{
    [SerializeField]
    private string _target;
    public string target => _target;

    public DestroyLine() : base(LineType.Destroy)
    {
        // 파괴 대상이 되는 오브젝트는 추후에 추가
    }

#if UNITY_EDITOR
    public DestroyLine(DestroyNodeData nodeData) : base(nodeData.guid, LineType.Destroy)
    {
        _target = nodeData.targetGuid;
    }
#endif
}