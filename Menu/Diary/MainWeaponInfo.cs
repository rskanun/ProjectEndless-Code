public class MainWeaponInfo : EquipInfo
{
    protected override string GetTagName()
    {
        return "<주 무기 칸>";
    }

    protected override void ShowEquips()
    {
        // 주무기 선택 화면 띄우기
        app.ShowWeapons();
    }
}