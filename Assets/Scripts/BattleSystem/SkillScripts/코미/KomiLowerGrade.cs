using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KomiLowerGrade", menuName = "ScriptableObjects/Komi/LowerGrade")]
public class KomiLowerGrade : SkillAction
{
    public float healAmountPerSecond = 50f; // 초당 힐량
    public int totalDuration = 3;          // 지속 시간 (3초)

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        // 스킬을 시전하면, 캐릭터에게 코루틴을 돌리라고 명령합니다.
        caster.StartCoroutine(HealOverTimeCoroutine(caster));
    }

    private System.Collections.IEnumerator HealOverTimeCoroutine(Battle_Character caster)
    {   
        Debug.Log($" [코미 저학년] 코미가 누워서 잠에 듭니다... (채널링 시작)");

        // 잠자는 동안 기절/행동불능 상태로 만듭니다. (평타나 이동 방지)
        caster.ChangeState(Battle_Character.CharacterState.Stunned);

        int elapsed = 0;
        while (elapsed < totalDuration)
        {
            yield return new WaitForSeconds(1.0f); // 1초 대기

            if (caster.currentState == Battle_Character.CharacterState.Dead) yield break; // 중간에 죽으면 탈출

            // HP 회복 처리
            caster.currentHp += healAmountPerSecond;
            //if (caster.currentHp > caster.myData.maxHP) caster.currentHp = caster.myData.maxHP;

            Debug.Log($" [코미 저학년] 쿨쿨... HP {healAmountPerSecond} 회복! (현재: {caster.currentHp})");
            elapsed++;
        }

        Debug.Log($" [코미 저학년] 코미가 잠에서 깨어났습니다.");
        caster.ChangeState(Battle_Character.CharacterState.Idle); // 다시 대기 상태로 복귀
    }
}