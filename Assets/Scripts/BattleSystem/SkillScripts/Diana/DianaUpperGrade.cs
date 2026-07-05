using log4net.Util;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DianaUpperGrade", menuName = "ScriptableObjects/Diana/UpperGrade")]
public class DianaUpperGrade : SkillAction
{
    [Header("HP 회복 : 입힌 피해량의 %")]
    public float healAmountPercent;
    [Header("물리피해 %")]
    public float attackPowerPercent;
    [Header("거대화 시 피해량 증가 %")]
    public float gigantDamagePercent;
    [Header("거대화 시 공격속도 증가 %")]
    public float gigantSpeedPercent;
    [Header("거대화 지속시간")]
    public float gigantDuration;

    [Header("범위 지정")]
    public float areaRadius;
    public float areaDamage;
    public LayerMask enemyLayer;

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(GiantTransformationCoroutine(caster, target));
    }


    private IEnumerator GiantTransformationCoroutine(Battle_Character caster, Battle_Character target)
    {
        // 내 위치(transform.position)를 중심으로 attackRadius 크기의 와이어 원을 그립니다.
        // (공격 범위 변수명인 attackRadius나 attackRange에 맞게 넣어주세요!)
        Debug.Log($" [코미 고학년] ★★★ 거 대 화 변 신 ★★★");
        yield return new WaitForSeconds(2f);
        // [효과 1] 비주얼 거대화 (스케일을 2배로 뻥튀기)
        if (caster.transform != null) caster.transform.localScale *= 2f;
        float temp = caster.attackCooldown;
        int temp2 = caster.attack_Power;

        float buffedAttackSpeed = 1 / caster.attackCooldown * (1 + gigantSpeedPercent / 100f);
        caster.attackCooldown = 1 / buffedAttackSpeed;
        float buffedAttackPower = (float)caster.attack_Power * gigantDamagePercent / 100f;
        caster.attack_Power = caster.attack_Power + (int)buffedAttackPower;

        TakeAreaDamage(caster, caster.transform.position, areaRadius, areaDamage);

        // 7초 동안 이 상태로 신나게 팹니다.
        yield return new WaitForSeconds(gigantDuration);

        // [효과 5] 지속시간 종료 후 원상 복구 (해제)
        if (caster.currentState != Battle_Character.CharacterState.Dead)
        {
            caster.transform.localScale /= 2f;
            caster.attackCooldown = temp;
            caster.attack_Power = temp2;
            Debug.Log($" [코미 고학년] 거대화 풀림. 코미가 원래 크기로 돌아왔습니다.");
        }
    }

    // 범위 공격 헬퍼 함수
    private void TakeAreaDamage(Battle_Character caster, Vector3 center, float radius, float damage)
    {
        Vector2 boxSize = new Vector2(areaRadius, 1f);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(caster.transform.position, boxSize, 0f, enemyLayer);
        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                Battle_Character enemy = hit.GetComponentInParent<Battle_Character>();
                if (enemy != null && caster.currentState != Battle_Character.CharacterState.Dead)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

    }
}