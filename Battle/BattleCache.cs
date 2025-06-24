public enum BattleResult
{
    None,
    Victory,
    Defeat,
    Escape
}

public class BattleTempData
{
    public BattleFieldData FieldData { get; set; }
    public BattleResult Result { get; set; }
}

public static class BattleCache
{
    public static BattleTempData Current { get; private set; } = new BattleTempData();

    public static void Clear()
    {
        Current = new BattleTempData();
    }
}