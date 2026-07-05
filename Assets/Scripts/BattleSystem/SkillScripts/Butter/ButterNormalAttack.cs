using UnityEngine;

[CreateAssetMenu(fileName = "ButterNormalAttack", menuName = "ScriptableObjects/Butter/NormalAttack")]
public class ButterNormalAttack : SkillAction
{
    [Header("평타")]
    public float normalDamageMultiplier;

    [Header("강화평타배율")]
    public float strongDamageMultiplier;
    [Header("발동확률")]
    [Range(0, 100)] public float strongChance;

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        if (target == null) return;

        // 확률 주사위 굴리기 (0~100)
        float dice = Random.Range(0f, 100f);

        if (dice <= strongChance)
        {   
            //개껌발사
            float damage = caster.MyData.attack_Power * strongDamageMultiplier;
            target.TakeDamage(damage);
        }
        else
        {
            //일반 평타
            //float damage = caster.MyData.attack_Power;
            float damage = 0f;
            target.TakeDamage(damage);
        }
    }
}