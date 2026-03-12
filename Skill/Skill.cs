using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [Header("스킬 정보")]
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField, PreviewField]
    private Sprite _iconSprite;
    public Sprite IconSprite => _iconSprite;

    [SerializeField]
    private TargetType _targetType;
    public TargetType TargetType => _targetType;

    [SerializeField]
    private AttackType _actionType;
    public AttackType ActionType => _actionType;

    [SerializeField]
    private float _costTurn;
    public float CostTurn => _costTurn;

    [SerializeField]
    private int _costSP;
    public int CostSP => _costSP;

    [SerializeField]
    [TextArea(0, 3)]
    private string _description;
    public string Description => _description;

    [Title("애니메이션 정보")]
    [SerializeField]
    private string _skillMotionName;
    private int _skillMotion;
    public int SkillMotion => _skillMotion;

    [SerializeField]
    private string _skillTriggerName;
    private int _skillTrigger;
    public int SkillTrigger => _skillTrigger;

    private void OnValidate()
    {
        _skillMotion = Animator.StringToHash(_skillMotionName);
        _skillTrigger = Animator.StringToHash(_skillTriggerName);
    }

    public abstract void OnCasting(Entity caster, List<Entity> targets);
    public abstract string GetTypeName();
}