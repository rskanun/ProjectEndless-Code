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

        bool hasMainWeapon = mainWeapon != null;
        bool isOneHandWeapon = hasMainWeapon && !mainWeapon.WeaponType.IsTwoHand();

        // 한손 무기를 든 상태에서만 보조 무기를 착용할 수 있음
        isAvailable = isOneHandWeapon;

        // 보조 무기 착용 여부에 따른 UI 표시 갱신
        unavailableMark.SetActive(!isAvailable);
        nameField.alpha = isAvailable ? 1.0f : 0.75f;
        nameField.text = isAvailable ? nameField.text : (mainWeapon?.Name ?? GetTagName());
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