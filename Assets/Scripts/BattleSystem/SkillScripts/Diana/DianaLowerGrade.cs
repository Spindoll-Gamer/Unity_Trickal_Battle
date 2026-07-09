
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DianaLowerGrade", menuName = "ScriptableObjects/Diana/LowerGrade")]
public class DianaLowerGrade : SkillAction
{
    [Header("첫번째 회복량 : 최대 HP의 %")]
    public float firstHealAmountPercent;
    [Header("두번째 회복량 : 공격력의 %")]
    public float secondHealAmountPercent;

    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        caster.StartCoroutine(HealCoroutine(caster));
    }

    private System.Collections.IEnumerator HealCoroutine(Battle_Character caster)
    {
        caster.ChangeState(Battle_Character.CharacterState.Stunned);
        List<Battle_Character> aliveMyTeam = BattleManager.Instance.aliveMyTeam;

        foreach (Battle_Character aliveCharacter in aliveMyTeam)
        {
            float maxHp = aliveCharacter.MyData.maxHP;
            float healAmount = (maxHp * firstHealAmountPercent) / 100f;
            aliveCharacter.currentHp = Mathf.Min(aliveCharacter.currentHp + healAmount, aliveCharacter.MyData.maxHP);
        }

        yield return new WaitForSeconds(1.5f);

        aliveMyTeam = BattleManager.Instance.aliveMyTeam;
        foreach (Battle_Character aliveCharacter in aliveMyTeam)
        {
            float healAmount = (caster.attack_Power * secondHealAmountPercent) / 100f;
            aliveCharacter.currentHp = Mathf.Min(aliveCharacter.currentHp + healAmount, aliveCharacter.MyData.maxHP);

        }
        caster.ChangeState(Battle_Character.CharacterState.Idle); // 다시 대기 상태로 복귀
    }
}