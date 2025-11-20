using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    [SerializeField, PropertyOrder(0)]
    private string _name;
    public string Name => _name;

    [SerializeField, PropertyOrder(0)]
    [PreviewField]
    private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField, PropertyOrder(1)]
    private BattlePosition _position;
    public BattlePosition Position => _position;

    [SerializeField, PropertyOrder(1)]
    private AttackType _attackType;
    public AttackType AttackType => _attackType;

    [SerializeField, PropertyOrder(1)]
    private PersonalityType _personality;
    public PersonalityType Personality => _personality;

    [Title("스킬 정보")]
    [SerializeField, PropertyOrder(10)]
    private List<Skill> _skills;
    public List<Skill> Skills => _skills;

    [Title("능력치 정보")]
    [SerializeField, PropertyOrder(20)]
    private EntityStats _stats;
    public EntityStats Stats => _stats;
}