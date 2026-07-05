
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ButterLowerGrade", menuName = "ScriptableObjects/Butter/LowerGrade")]
public class ButterLowerGrade : SkillAction
{
    [Header("데미지 배율")]
    public float DamageMultiplier;
    [Header("공격횟수")]
    public int AttackCount;
    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(ButterFlyCoroutine(caster,target));
    }

    private System.Collections.IEnumerator ButterFlyCoroutine(Battle_Character caster, Battle_Character target)
    {
        caster.ChangeState(Battle_Character.CharacterState.Stunned);
        for (int i = 0; i < AttackCount; i++)
        {
            if (target == null && target.currentState == Battle_Character.CharacterState.Dead)
            {
                target = caster.FindTarget();
                if(target == null)
                {
                    caster.ChangeState(Battle_Character.CharacterState.Idle); // 다시 대기 상태로 복귀
                    yield break;
                }
            }
            float damage = caster.attack_Power * DamageMultiplier/100f;
            target.TakeDamage(damage);
            yield return new WaitForSeconds(0.2f);

        }
        caster.ChangeState(Battle_Character.CharacterState.Idle); // 다시 대기 상태로 복귀
    }
}
