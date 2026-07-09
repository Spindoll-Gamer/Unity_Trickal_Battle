    
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KomiLowerGrade", menuName = "ScriptableObjects/Komi/LowerGrade")]
public class KomiLowerGrade : SkillAction
{
    [Header("회복량 : 최대 HP의 %")]
    public float healAmountPercent;
    [Header("코미 꿀잠시간")]
    public int totalDuration;
    
    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(HealOverTimeCoroutine(caster));
    }

    private System.Collections.IEnumerator HealOverTimeCoroutine(Battle_Character caster)
    {
        float healAmountPerSecond = caster.MyData.maxHP * (healAmountPercent/100f) / (float)totalDuration;
        Debug.Log($" [코미 저학년] 푹신푹신타임");


        int elapsedTime = 0;
        while (elapsedTime < totalDuration)
        {
            yield return new WaitForSeconds(1.0f);

            if (caster.currentState == Battle_Character.CharacterState.Dead) yield break;

            caster.currentHp = Mathf.Min(caster.currentHp + healAmountPerSecond, caster.MyData.maxHP);
            caster.HPValueChanged();
            elapsedTime++;
        }
        caster.ChangeState(Battle_Character.CharacterState.Idle); // 다시 대기 상태로 복귀
    }
}