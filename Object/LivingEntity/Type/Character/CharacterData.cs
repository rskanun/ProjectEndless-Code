using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private bool _isUnlocked;
    public virtual bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }

    [SerializeField]
    private bool _isParty;
    public virtual bool IsParty
    {
        get { return _isParty; }
        set
        {
            if (IsUnlocked)
            {
                _isParty = value;
            }
        }
    }

    [SerializeField]
    private bool _isDead; // 전투 사망 X
    public bool IsDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    [SerializeField]
    private CharacterProfile _profile;
    public CharacterProfile Profile => _profile;

    [SerializeField]
    private BattlePosition _position;
    public BattlePosition Position => _position;

    [SerializeField]
    private AttackType _attackType;
    public AttackType AttackType => _attackType;

    [SerializeField]
    private PersonalityType _personality;
    public PersonalityType Personality => _personality;

    [Header("스킬 정보")]
    [SerializeField]
    private List<Skill> _hasSkills; // 해당 캐릭터가 지닌 스킬 목록
    public List<Skill> HasSkills => _hasSkills;
    [SerializeField]
    private List<Skill> _usableSkills; // 해당 캐릭터의 사용 할 수 있는 스킬 목록
    public List<Skill> UsableSkills => _usableSkills;

    [Header("장비 정보")]
    [SerializeField]
    private Weapon _mainWeapon;
    public Weapon MainWeapon
    {
        get => _mainWeapon;
        set => _mainWeapon = value;
    }

    [SerializeField]
    private Weapon _offWeapon;
    public Weapon OffWeapon
    {
        get => _offWeapon;
        set => _offWeapon = value;
    }

    [SerializeField]
    private Accessory _accessory1;
    public Accessory Accessory1
    {
        get => _accessory1;
        set => _accessory1 = value;
    }

    [SerializeField]
    private Accessory _accessory2;
    public Accessory Accessory2
    {
        get => _accessory2;
        set => _accessory2 = value;
    }

    [Header("스탯")]
    [SerializeField]
    private EntityStat _stat;
    public EntityStat Stat => _stat;
}

[System.Serializable]
public class CharacterProfile
{
    [SerializeField]
    private Sprite _profileImage;
    public Sprite ProfileImage => _profileImage;

    [SerializeField]
    private string _occupation;
    public string Occupation
    {
        get => _occupation;
        set => _occupation = value;
    }

    [SerializeField]
    private string _ability;
    public string Ability
    {
        get => _ability;
        set => _ability = value;
    }

    [SerializeField]
    private string _hobby;
    public string Hobby
    {
        get => _hobby;
        set => _hobby = value;
    }
}