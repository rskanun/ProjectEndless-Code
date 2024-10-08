using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("캐릭터 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private bool _isPlayer;
    public bool IsPlayer
    {
        get { return _isPlayer; }
    }

    [SerializeField]
    private bool _isUnlocked;
    public bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }

    [SerializeField]
    private bool _isParty;
    public bool IsParty
    {
        get { return _isParty; }
        set
        {
            if (IsUnlocked != false)
            {
                _isParty = value;
            }
        }
    }

    [SerializeField]
    private BattlePosition _position;
    public BattlePosition Position
    {
        get { return _position; }
    }

    [SerializeField]
    private AttackType _attackType;
    public AttackType AttackType
    {
        get { return _attackType; }
    }

    [Header("스킬 정보")]
    [SerializeField]
    private List<Skill> _skills;
    public List<Skill> Skills
    {
        get { return _skills; }
    }

    [Header("장비 정보")]
    [SerializeField]
    private Armor _armor;
    // 추후 추가

    [Header("스탯")]
    [SerializeField]
    private EntityStat _stat;
    public EntityStat Stat
    {
        get { return _stat; }
    }
}