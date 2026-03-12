using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Item/Accessory")]
public class Accessory : Equip
{
    public override ItemType Type => ItemType.Accessory;
}