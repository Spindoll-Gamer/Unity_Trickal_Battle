using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    public string skillName;            // 스킬 이름
    [TextArea] public string description; // 스킬 툴팁 설명

    [Header("실제 작동할 행동 모터 (로직 칩)")]
    public SkillAction skillAction; //여기에 KomiNormalAttack 같은 진짜 행동 코드를 꽂아줍니다!
}