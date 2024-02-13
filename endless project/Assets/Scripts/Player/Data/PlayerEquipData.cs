using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player/Equipment", fileName = "PlayerEquipmentData")]
public class PlayerEquipData : ScriptableObject
{
    [Header("장비")]

    [SerializeField]
    private Weapon _weapon;
    public Weapon weapon
    {
        get { return _weapon; }
        set
        {
            _weapon = value;
        }
    }
}