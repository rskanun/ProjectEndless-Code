using UnityEngine;

public class OffWeaponInfo : EquipInfo
{
    [SerializeField] private GameObject unavailableMark;

    /// <summary>
    /// 메인 무기에 따라 해당 칸에 장비를 설정할 수 있는 지 여부를 업데이트
    /// </summary>
    public void UpdateAvailable()
    {
        Weapon mainWeapon = app.SelectCharacter.MainWeapon;
        isAvailable = mainWeapon != null && !mainWeapon.WeaponType.IsTwoHand();

        unavailableMark.SetActive(!IsAvailable);
        nameField.alpha = IsAvailable ? 1.0f : 0.75f;
        nameField.text = IsAvailable ? nameField.text : mainWeapon.Name;
    }

    public override void UpdateInfo(Equip equip)
    {
        base.UpdateInfo(equip);

        // 해당 칸에 장비를 착용할 수 있는 지 여부 설정
        UpdateAvailable();
    }

    protected override string GetTagName()
    {
        return "<보조 무기 칸>";
    }
    protected override void ShowEquips()
    {
        app.ShowOffWeapons();
    }
}