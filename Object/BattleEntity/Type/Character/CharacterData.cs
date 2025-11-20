using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity Data/Character", fileName = "Character Data")]
public class CharacterData : EntityData
{
    [SerializeField, PropertyOrder(0)]
    private bool _isUnlocked;
    public virtual bool IsUnlocked
    {
        get => _isUnlocked;
        set => _isUnlocked = value;
    }

    [SerializeField, PropertyOrder(0)]
    private bool _isParty;
    public virtual bool IsParty
    {
        get => _isParty;
        set => _isParty = value;
    }

    [SerializeField, PropertyOrder(0)]
    private bool _isSlain; // 캐릭터의 영구적인 사망(주인공에게 토벌 당했는가)
    public bool IsSlain
    {
        get => _isSlain;
        set => _isSlain = value;
    }

    [SerializeField, PropertyOrder(0)]
    private CharacterProfile _profile;
    public CharacterProfile Profile => _profile;

    [SerializeField, PropertyOrder(10)]
    private List<Skill> _hasSkills; // 해당 캐릭터가 지닌 스킬 목록
    public List<Skill> HasSkills => _hasSkills;

    [Title("장비 정보")]
    [SerializeField, PropertyOrder(10)]
    private WeaponType _usableWeaponType;
    public WeaponType UsableWeaponType => _usableWeaponType;

    [SerializeField, PropertyOrder(10)]
    private Weapon _mainWeapon;
    public Weapon MainWeapon
    {
        get => _mainWeapon;
        set => _mainWeapon = value;
    }

    [SerializeField, PropertyOrder(10)]
    private Weapon _offWeapon;
    public Weapon OffWeapon
    {
        get => _offWeapon;
        set => _offWeapon = value;
    }

    [SerializeField, PropertyOrder(10)]
    private Accessory _accessory1;
    public Accessory Accessory1
    {
        get => _accessory1;
        set => _accessory1 = value;
    }

    [SerializeField, PropertyOrder(10)]
    private Accessory _accessory2;
    public Accessory Accessory2
    {
        get => _accessory2;
        set => _accessory2 = value;
    }
}

[System.Serializable]
public class CharacterProfile
{
    [SerializeField, PreviewField]
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