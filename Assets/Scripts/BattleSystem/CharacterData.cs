using UnityEngine;
using static BattleEnums;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "BattleSystem/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Profile")]
    public string characterID;    // 고유 ID (데이터 검색용)
    public string characterName;  // 화면 표시 이름
    public Sprite iconSprite;     // 인벤토리 UI용 아이콘
    public Sprite portraitSprite; // 배치 화면용 일러스트
    public Sprite upperGradeSprite;

    [Header("BattleInfo")]

    public ElementType elementType;
    public PositionRow positionRow;
    public UnitRole unitRole;
    public AttackType attackType;
    
    [Header("Stats")]
    public float maxHP;
    public float maxSP;
    public float regenSP;
    public int attack_Power;
    public int defence_Power;
    public int critical_Probability;
    public int critical_Multiplier;
    public float attackRange;

    [Header(" 캐릭터별 고유 스킬 설정")]

    public SkillData normalAttack;
    public SkillData lowerSkill; 
    public SkillData upperSkill; 

    [Header("고학년 스킬 쿨타임")]
    public float cooldownTime;
}