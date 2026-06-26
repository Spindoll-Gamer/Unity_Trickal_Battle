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

    [Header("BattleInfo")]

    public ElementType elementType;
    public PositionRow positionRow;
    public UnitRole unitRole;
    public AttackType attackType;
    
    [Header("Stats")]
    public int maxHP;
    public int currentHP;
    public int maxSP;
    public int currentSP;
    public int regenSP;
    public int physical_Attack_Power;
    public int physical_Defence_Power;
    public int magical_Attack_Power;
    public int magical_Defence_Power;
    public int critical_Probability;
    public int critical_Multiplier;

    [Header(" 캐릭터별 고유 스킬 설정")]

    public SkillData normalAttack;
    public SkillData lowerSkill; // 인펙터에서 저학년 스킬 SO를 드래그해서 넣는 곳
    public SkillData upperSkill; // 인
}