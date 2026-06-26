using UnityEngine;
using static BattleEnums;
using UnityEngine.TextCore.Text;
using static Codice.CM.Common.CmCallContext;

public abstract class UnitBase : MonoBehaviour
{
    [Header("Unit Information")]
    public string unitName;             //이름
    public ElementType element;         //성격
    public UnitRole role;               //역할
    public PositionRow row;             //배치
    public AttackType attackType;

    [Header("Stats")]
    public int maxHP;
    public int currentHP;
    public int maxSP;
    public int currentSP;
    public int physical_Attack_Power;
    public int physical_Defence_Power;
    public int magical_Attack_Power;
    public int magical_Defence_Power;
    public int critical_Probability;
    public int critical_Multiplier;

    protected virtual void Awake()
    {
        currentHP = maxHP;
        currentSP = 0;
    }
    
    /*public void TakeDamage(int rawDamage, ElementType attackerElement)
    {
        float modifier = BattleCalculator.GetElementModifier(attackerElement, this.element);
        int finalDamage = Mathf.RoundToInt(rawDamage * modifier);

        currentHP -= finalDamage;
        currentHP = Mathf.Max(0, currentHP);

        BattleEventManager.OnUnitHpChanged?.Invoke(this);

        if (currentHP <= 0)
        {
            Die();
        }
    }*/
    public abstract void UseActiveSkill();
    protected abstract void Die();
}