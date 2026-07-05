using UnityEngine;

[CreateAssetMenu(fileName = "KomiNormalAttack", menuName = "ScriptableObjects/Komi/NormalAttack")]
public class KomiNormalAttack : SkillAction
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
            //  강하게 내려치기 발동!
            //float damage = caster.MyData.attack_Power * strongDamageMultiplier;
            float damage = 0;
            target.TakeDamage(damage);
            // 여기에 화면 흔들림(Camera Shake)이나 큰 이펙트 펑!
        }
        else
        {
            // 일반 평타
            //float damage = caster.MyData.attack_Power;
            float damage = 0f;
            target.TakeDamage(damage);
        }
    }
}