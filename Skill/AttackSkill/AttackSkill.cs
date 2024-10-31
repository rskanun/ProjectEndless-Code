using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/AttackSkill", fileName = "Attack_Skill")]
public class AttackSkill : Skill
{
    [Header("데미지 정보")]
    [SerializeField]
    private float _damage;
    public float Damage
    {
        get { return _damage; }
    }
    [SerializeField]
    private float _mpDegree;
    public float MpDegree
    {
        get { return _mpDegree; }
    }
    [SerializeField]
    private float _strDegree;
    public float StrDegree
    {
        get { return _strDegree; }
    }

    [Header("피격시 디버프")]
    [SerializeField]
    private Debuff _debuff;
    public Debuff Debuff
    {
        get { return _debuff; }
    }

    public override void OnCasting(Entity caster, List<Entity> targets)
    {
        float damage = GetSkillDmg(caster);

        foreach (Entity entity in targets)
        {
            entity.OnDamage(damage, caster.Stat.MP);

            if (Debuff != null)
            {
                // 적용할 디버프가 있으면 디버프 적용
                entity.AddEffect(Debuff);
            }
        }
    }

    public float GetSkillDmg(Entity caster)
    {
        // 데미지 공식 = 기본 데미지 + 시전자 MP 계수 + 시전자 STR 계수
        return Damage + caster.Stat.MP * MpDegree + caster.Stat.STR * StrDegree;
    }
}