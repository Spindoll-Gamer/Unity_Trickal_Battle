using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "DianaNormalAttack", menuName = "ScriptableObjects/Diana/NormalAttack")]
public class DianaNormalAttack : SkillAction
{
    [Header("평타")]
    public float normalDamageMultiplier;

    [Header("강화평타배율")]
    public float strongDamageMultiplier;
    [Header("발동확률")]
    [Range(0, 100)] public float strongChance;
    [Header("HP 회복")]
    public float healAmountMultiplier;

    Battle_Character lowestHpCharacter = null;
    float lowestHpPercent = 101f;


    public override void ExecuteSkill(Battle_Character caster, Battle_Character target)
    {
        if (target == null) return;

        // 확률 주사위 굴리기 (0~100)
        float dice = Random.Range(0f, 100f);

        if (dice <= strongChance)
        {
            float damage = caster.MyData.attack_Power * (strongDamageMultiplier/100f);
            target.TakeDamage(damage);

            //살아있는 아군들의 정보를 가져와서 currentHP/maxHP*100 하면 현재 체력 비율 나오니까 걔를 골라서 회복
            List<Battle_Character> aliveMyteam = BattleManager.Instance.aliveMyTeam;

            foreach( Battle_Character aliveCharacter in aliveMyteam)
            {
                float currentHp = aliveCharacter.currentHp;
                float maxHp = aliveCharacter.MyData.maxHP;
                float hpPercent = (currentHp / maxHp) * 100f;
                
                if(hpPercent < lowestHpPercent)
                {
                    lowestHpPercent = hpPercent;
                    lowestHpCharacter = aliveCharacter; 
                }
            }

            if(lowestHpCharacter != null)
            {
                lowestHpCharacter.currentHp = Mathf.Min(lowestHpCharacter.currentHp + damage * (healAmountMultiplier / 100f), lowestHpCharacter.MyData.maxHP);
            }
        }
        else
        {
            // 일반 평타
            float damage = caster.MyData.attack_Power * normalDamageMultiplier/100f;
            target.TakeDamage(damage);
        }
    }
}