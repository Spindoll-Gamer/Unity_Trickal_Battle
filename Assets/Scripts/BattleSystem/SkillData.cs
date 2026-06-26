using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("스킬 기본 정보")]
    public string skillName;            // 스킬 이름
    [TextArea] public string description; // 스킬 툴팁 설명

    [Header("밸런스 조정 수치 (설명서 내용)")]
    public float damageMultiplier = 1.0f; // 데미지 배율
    public float skillValue = 0f;          // 고정 치유량 혹은 실드량
    public float duration = 0f;            // 지속 시간

    [Header("실제 작동할 행동 모터 (로직 칩)")]
    public SkillAction skillAction; //여기에 KomiNormalAttack 같은 진짜 행동 코드를 꽂아줍니다!
}