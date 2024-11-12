[System.Serializable]
public class PlayerData : CharacterData
{
    public override bool IsUnlocked
    {
        get
        {
            // 주인공은 상시 해금 상태
            if (base.IsUnlocked == false)
                base.IsUnlocked = true;

            return base.IsUnlocked;
        }
    }

    public override bool IsParty
    {
        get
        {
            // 주인공은 상시 파티에 가입된 상태
            if (base.IsParty == false)
                base.IsParty = true;

            return base.IsParty;
        }
    }

    public PlayerData()
    {
        base.IsUnlocked = true;
        base.IsParty = true;
    }
}