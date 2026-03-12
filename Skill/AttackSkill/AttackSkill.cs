using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/AttackSkill", fileName = "Attack_Skill")]
public class AttackSkill : Skill
{
    [Header("공격 정보")]
    [SerializeField]
    private bool _isAssistable;
    public bool IsAssistable => _isAssistable;

    [SerializeField]
    private bool _isParryable;
    public bool IsParryable => _isParryable;

    [SerializeField]
    private bool _isDodgeable;
    public bool IsDodgeable => _isDodgeable;

    [SerializeField]
    private float _damage;
    public float Damage => _damage;

    [SerializeField]
    private float _mpDegree;
    public float MpDegree => _mpDegree;

    [SerializeField]
    private float _strDegree;
    public float StrDegree => _strDegree;

    [SerializeField]
    private Debuff _debuff;
    public Debuff Debuff => _debuff;

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        float damage = GetSkillDmg(caster);

        foreach (Entity target in targets)
        {
            float criticalChance = caster.GetCriticalChance(target);
            target.OnDamage(damage, caster.FinalStats.MP, criticalChance);

            if (!Debuff.IsEmpty())
            {
                // 적용할 디버프가 있으면 디버프 적용
                target.AddEffect(Debuff);
            }
        }
    }

    public float GetSkillDmg(Entity caster)
    {
        // 데미지 공식 = 기본 데미지 + 시전자 MP 계수 + 시전자 STR 계수
        return Damage + caster.FinalStats.MP * MpDegree + caster.FinalStats.STR * StrDegree;
    }

    public override string GetTypeName()
    {
        return "액티브 스킬";
    }
}