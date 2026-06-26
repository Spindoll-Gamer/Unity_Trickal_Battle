using UnityEngine;
using UnityEngine.SceneManagement;
using static BattleEnums;

public class BattleDataReceiver : MonoBehaviour
{
    public static BattleDataReceiver Instance { get; private set; }

    // 전투로 들고 갈 전/중/후열 캐릭터 최종 데이터 배열
    public CharacterData[] frontLine = new CharacterData[3];
    public CharacterData[] middleLine = new CharacterData[3];
    public CharacterData[] backLine = new CharacterData[3];

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
    }
}