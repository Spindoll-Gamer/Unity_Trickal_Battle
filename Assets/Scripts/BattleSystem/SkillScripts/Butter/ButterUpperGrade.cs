using log4net.Util;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ButterUpperGrade", menuName = "ScriptableObjects/Butter/UpperGrade")]
public class ButterUpperGrade : SkillAction
{
    [Header("범위 지정")]
    public float areaRadius;
    public float areaDamage;
    public LayerMask enemyLayer;

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(SlingShotStrikeCoroutine(caster, target));
    }


    private IEnumerator SlingShotStrikeCoroutine(Battle_Character caster, Battle_Character target)
    {
        Debug.Log($" [버터 고학년] 새쭁 쓔튜라이크");
        yield return new WaitForSeconds(2f);

        TakeAreaDamage(caster,target, target.transform.position, areaRadius, areaDamage);

        yield return new WaitForSeconds(1f);

        if (caster.currentState != Battle_Character.CharacterState.Dead)
        {
            caster.ChangeState(Battle_Character.CharacterState.Idle);
        }
    }

    // 범위 공격 헬퍼 함수
    private void TakeAreaDamage(Battle_Character caster, Battle_Character target, Vector3 center, float radius, float damage)
    {
        Vector2 boxSize = new Vector2(areaRadius, 1f);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(target.transform.position, boxSize, 0f, enemyLayer);
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