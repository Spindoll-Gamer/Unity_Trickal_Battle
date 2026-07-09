using log4net.Util;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DianaUpperGrade", menuName = "ScriptableObjects/Diana/UpperGrade")]
public class DianaUpperGrade : SkillAction
{
    [Header("데미지 배율 %")]
    public float firstDamageMultiplier;
    public float secondDamageMultiplier;

    [Header("범위 지정")]
    public float areaRadius;
    public float areaDamage;
    public LayerMask enemyLayer;
    public int hitCount;
    public float delayPerHit;

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(DianaUpperCoroutine(caster, target));
    }


    private IEnumerator DianaUpperCoroutine(Battle_Character caster, Battle_Character target)
    {
        yield return new WaitForSeconds(1.5f);

        TakeAreaDamage(caster, caster.transform.position, areaRadius, firstDamageMultiplier);

        yield return new WaitForSeconds(0.5f);

        TakeAreaDamage(caster, caster.transform.position, areaRadius, secondDamageMultiplier);
    }

    // 범위 공격 헬퍼 함수
    private void TakeAreaDamage(Battle_Character caster, Vector3 center, float width, float damageMultiplier)
    {
        Vector3 startPos = caster.transform.position;
        Vector3 centerPos = startPos + (caster.transform.right * (width / 2f)) + (caster.transform.up / 2f);
        Vector2 boxSize = new Vector2(width, 1f);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(caster.transform.position, boxSize, 0f, enemyLayer);
        float damage = caster.attack_Power * damageMultiplier;

        for (int i = 0; i < hitCount; i++)
        {
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
}