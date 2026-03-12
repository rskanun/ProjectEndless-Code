using UnityEngine;

public class BattleCameraOption : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField]
    private Transform _bodyPivot;
    public Transform BodyPivot => _bodyPivot;

    [SerializeField]
    private Transform _selectionPivot;
    public Transform SelectionPivot => _selectionPivot;

    [SerializeField]
    private float _size;
    public float Size => _size;
}