using UnityEngine;

[CreateAssetMenu(fileName = "KomiNormalAttack", menuName = "ScriptableObjects/Komi/NormalAttack")]
public class KomiNormalAttack : SkillAction
{
    public float normalDamageMultiplier = 1.0f;
    public float strongDamageMultiplier = 1.8f; // 강화 평타 배율
    [Range(0, 100)] public float strongChance = 20f; // 20% 확률

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        if (target == null) return;

        // 확률 주사위 굴리기 (0~100)
        float dice = Random.Range(0f, 100f);

        if (dice <= strongChance)
        {
            //  강하게 내려치기 발동!
            float damage = 70f;
            target.TakeDamage(damage);
            Debug.Log($" [코미 평타] 쾅! 베개를 강하게 내려쳤습니다! ({strongDamageMultiplier}배 대미지: {damage})");
            // 여기에 화면 흔들림(Camera Shake)이나 큰 이펙트 펑!
        }
        else
        {
            // 일반 평타
            float damage = 10f;
            target.TakeDamage(damage);
            Debug.Log($"?? [코미 평타] 베개 툭. ({damage} 대미지)");
        }
    }
}