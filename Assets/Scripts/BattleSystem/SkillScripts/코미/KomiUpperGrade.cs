using UnityEngine;

[CreateAssetMenu(fileName = "KomiUpperGrade", menuName = "ScriptableObjects/Komi/UpperGrade")]
public class KomiUpperGrade : SkillAction
{
    [Header("1. 거대화 착지 피해")]
    public float landingDamageMultiplier = 3.0f;
    public float impactRadius = 4.0f;

    [Header("2. 거대화 즉시 회복량")]
    public float instantHealValue = 300f;

    [Header("3. 변신 버프 수치 및 지속시간")]
    public float damageBuffRatio = 1.5f;     // 공격력 50% 증가
    public float attackSpeedBuffRatio = 1.4f; // 공격 속도 40% 증가
    public float transformationDuration = 7.0f; // 7초 동안 지속

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(GiantTransformationCoroutine(caster, target));
    }

    private System.Collections.IEnumerator GiantTransformationCoroutine(Battle_Character caster, Battle_Character target)
    {
        Debug.Log($" [코미 고학년] ★★★ 거 대 화 변 신 ★★★");

        // [효과 1] 비주얼 거대화 (스케일을 2배로 뻥튀기)
        if (caster.transform != null) caster.transform.localScale *= 2f;

        // [효과 2] 즉시 체력 회복
        caster.currentHp += instantHealValue;
        //if (caster.currentHp > caster.myData.maxHP) caster.currentHp = caster.myData.maxHP;
        Debug.Log($" [코미 고학년] 거대화 생명력 충전! HP {instantHealValue} 회복.");

        // [효과 3] 착지 충격파 범위 물리 피해 발동
        Vector3 impactCenter = target != null ? target.transform.position : caster.transform.position;
        //ExecuteAreaDamage(caster, impactCenter, impactRadius, caster.GetFinalAttackPower() * landingDamageMultiplier);

        // [효과 4] 지속시간 동안 공격력 및 공격속도 버프 부여
        //caster.damageModifier = damageBuffRatio;
        //caster.attackSpeedModifier = attackSpeedBuffRatio;
        Debug.Log($" [코미 고학년] 버프 활성화! 피해량 x{damageBuffRatio}, 공속 x{attackSpeedBuffRatio}");

        // 7초 동안 이 상태로 신나게 팹니다.
        yield return new WaitForSeconds(transformationDuration);

        // [효과 5] 지속시간 종료 후 원상 복구 (해제)
        if (caster.currentState != Battle_Character.CharacterState.Dead)
        {
            /*if (caster.modelTransform != null) caster.modelTransform.localScale /= 2f;
            caster.damageModifier = 1.0f;
            caster.attackSpeedModifier = 1.0f;*/
            Debug.Log($" [코미 고학년] 거대화 풀림. 코미가 원래 크기로 돌아왔습니다.");
        }
    }

    // 범위 공격 헬퍼 함수
    private void ExecuteAreaDamage(Battle_Character caster, Vector3 center, float radius, float damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Battle_Character enemy = hit.GetComponent<Battle_Character>();
                if (enemy != null && caster.currentState != Battle_Character.CharacterState.Dead)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}