using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static BattleEnums;

public class BattleDataReceiver : MonoBehaviour
{
    public static BattleDataReceiver Instance { get; private set; }

    // 전투로 들고 갈 전/중/후열 캐릭터 최종 데이터 배열
    public CharacterData[] frontLine = new CharacterData[3];
    public CharacterData[] middleLine = new CharacterData[3];
    public CharacterData[] backLine = new CharacterData[3];

    public CharacterData[] eFrontLine = new CharacterData[3];
    public CharacterData[] eMiddleLine = new CharacterData[3];
    public CharacterData[] eBackLine = new CharacterData[3];

    public List<Battle_Character> aliveMyTeam
    {
        get => BattleManager.Instance.aliveMyTeam;
    }

    public List<Battle_Character> aliveEnemyTeam
    {
        get => BattleManager.Instance.aliveEnemyTeam;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 넘어가도 이 데이터는 파괴되지 않고 유지됩니다!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 전투 시작 버튼을 누를 때 TeamPlacementManager의 데이터를 복사해오는 함수
    public void PackTeamData(TeamPlacementManager manager)
    {
        // manager에서 작업했던 slotGroups의 데이터를 안전하게 복사합니다.
        frontLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Front).placedCharacters.Clone();
        middleLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Mid).placedCharacters.Clone();
        backLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Back).placedCharacters.Clone();

        //eFrontLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Front).placedCharacters.Clone();
        //eMiddleLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Mid).placedCharacters.Clone();
        //eBackLine = (CharacterData[])manager.slotGroups.Find(g => g.positionType == PositionRow.Back).placedCharacters.Clone();
        //allCharacters.Clear(); // 혹시 이전 데이터가 남아있을 수 있으니 깔끔하게 비워줍니다.

        //AddCharactersFromLine(frontLine);
        //AddCharactersFromLine(middleLine);
        //AddCharactersFromLine(backLine);
    }

    /*private void AddCharactersFromLine(CharacterData[] line)
    {
        if (line == null) return;

        for (int i = 0; i < line.Length; i++)
        {
            // 슬롯이 비어있을 수도 있으므로(3명 꽉 안 채우고 출전하는 경우), null이 아닐 때만 리스트에 넣습니다.
            if (line[i] != null)
            {
                allCharacters.Add(line[i]);
            }
        }
    }*/
}