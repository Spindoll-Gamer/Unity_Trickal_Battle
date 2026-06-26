using UnityEngine;

public abstract class SkillAction : ScriptableObject
{
    // 스킬이 발동될 때 실행될 추상 함수
    public abstract void ExecuteSkill(Battle_Character caster, Battle_Character target);

    // 평타를 칠 때마다 특수한 처리가 필요한 캐릭터들을 위한 가상 함수 (기본적으론 비어있음)
    public virtual void OnNormalAttack(Battle_Character caster) { }
}