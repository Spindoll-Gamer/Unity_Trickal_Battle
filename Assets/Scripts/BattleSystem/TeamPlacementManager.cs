using UnityEngine;
using System.Collections.Generic;
using static BattleEnums;

public class TeamPlacementManager : MonoBehaviour
{
    public static TeamPlacementManager Instance { get; private set; }

    [System.Serializable]
    public class PositionSlotGroup
    {
        public PositionRow positionType;
        public List<PlaceSlot> uiSlots = new List<PlaceSlot>(); // 에디터에서 각 열에 맞는 3개 슬롯 연결
        public CharacterData[] placedCharacters;
    }

    [Header("포지션별 슬롯 그룹 설정 (전/중/후 3개 조 생성)")]
    public List<PositionSlotGroup> slotGroups = new List<PositionSlotGroup>();

    
    public int maxCharactersPerLine = 3; // 한 줄당 최대 배치 수

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (slotGroups != null)
        {
            foreach (var group in slotGroups)
            {
                // 게임 시작 시 후열/중열/전열 각 그룹의 데이터 배열 크기를 무조건 3으로 꽉 묶어줍니다.
                if (group.placedCharacters == null || group.placedCharacters.Length != 3)
                {
                    group.placedCharacters = new CharacterData[3];
                }
            }
        }
    }

    // 캐릭터 카드가 클릭되었을 때 실행되는 핵심 함수!
    public void TryPlaceCharacter(CharacterData data)
    {
        // 1. 클릭한 캐릭터의 포지션에 맞는 그룹을 찾습니다.
        PositionSlotGroup targetGroup = slotGroups.Find(g => g.positionType == data.positionRow);
        if (targetGroup == null) return;

        // 2. 이미 해당 열에 배치되어 있다면? -> 팀에서 제외 (토글)
        for (int i = 0; i < targetGroup.placedCharacters.Length; i++)
        {
            if (targetGroup.placedCharacters[i] == data)
            {
                targetGroup.placedCharacters[i] = null; // B, C를 당기지 않고 그 자리만 null로 만듦
                Debug.Log($"{data.positionRow}의 {data.characterName} 배치 해제 (칸 번호: {i})");
                UpdateLineUI(targetGroup);
                return;
            }
        }
        // 3. 자리가 있다면 해당 포지션 라인에 쏙 추가!
        for (int i = 0; i < targetGroup.placedCharacters.Length; i++)
            {
                if (targetGroup.placedCharacters[i] == null)
                {
                    targetGroup.placedCharacters[i] = data;
                    Debug.Log($"{data.characterName}를 {data.positionRow}에 배치 완료 (칸 번호: {i})");
                    UpdateLineUI(targetGroup);
                    return;
                }
            }
       
        // 4. 해당 열이 이미 꽉 찼다면? (예: 전열 3마리 풀 방인데 전열 캐릭 또 넣으려고 할 때)
        if (targetGroup.placedCharacters.Length >= maxCharactersPerLine)
        {
            Debug.Log($"{data.positionRow} 자리가 꽉 찼습니다! 더 이상 배치할 수 없습니다.");
            return;
        }
    }

    // 특정 라인의 UI만 갱신해주는 함수
    private void UpdateLineUI(PositionSlotGroup group)
    {
        for (int i = 0; i < group.uiSlots.Count; i++)
        {
            if (i < group.placedCharacters.Length)
            {
                // 배열의 데이터(있으면 캐릭터 정보, 없으면 null)와 순서(i)를 그대로 넘깁니다.
                group.uiSlots[i].SetCharacter(group.placedCharacters[i], i);
            }
        }
    }
}