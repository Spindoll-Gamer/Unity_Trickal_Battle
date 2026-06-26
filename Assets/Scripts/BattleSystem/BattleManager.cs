using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [Header("전투 전용 캐릭터 프리팹 (실제 전투 로직/애니메이션이 있는 오브젝트)")]
    public GameObject battleCharacterPrefab;

    [Header("전투 맵의 전/중/후열 소환 위치 (Transform 9개)")]
    public GameObject[] frontSpawnPoints = new GameObject[3];
    public GameObject[] middleSpawnPoints = new GameObject[3];
    public GameObject[] backSpawnPoints = new GameObject[3];
    

    // 실제 전투 필드에 태어난 캐릭터 스크립트들을 담아둘 리스트
    private List<Battle_Character> activeAllies = new List<Battle_Character>();

    private void Start()
    {
        // 전투 씬이 시작되자마자 팀을 스폰합니다.
        SpawnMyTeam();
    }

    private void SpawnMyTeam()
    {
        if (BattleDataReceiver.Instance == null)
        {
            Debug.LogError("전투에 사용할 팀 데이터가 없습니다!");
            return;
        }

        // 1. 전열 스폰
        SpawnLine(BattleDataReceiver.Instance.frontLine, frontSpawnPoints);
        // 2. 중열 스폰
        SpawnLine(BattleDataReceiver.Instance.middleLine, middleSpawnPoints);
        // 3. 후열 스폰
        SpawnLine(BattleDataReceiver.Instance.backLine, backSpawnPoints);

        Debug.Log($"총 {activeAllies.Count}마리의 아군 캐릭터가 전투 필드에 배치되었습니다!");

        // 4. 여기서부터 이제 "전투 시작!" 루틴이나 턴제 타이머를 돌리면 됩니다.
    }

    private void SpawnLine(CharacterData[] lineData, GameObject[] spawnPoints)
    {
        for (int i = 0; i < lineData.Length; i++)
        {
            // 데이터가 null이면 배치가 안 된 빈자리이므로 패스!
            if (lineData[i] == null) continue;
            // 해당 자리에 지정된 소환 위치(Spawn Point)에 프리팹 생성
            GameObject go = Instantiate(battleCharacterPrefab, spawnPoints[i].transform.position, Quaternion.identity, spawnPoints[i].transform);
            go.transform.position = spawnPoints[i].transform.position;

            // 실질적인 전투 컴포넌트(체력, 공격력 등)를 가져와서 데이터 주입
            Battle_Character battleChar = go.GetComponent<Battle_Character>();
            if (battleChar != null)
            {
                battleChar.Init(lineData[i]); // 네르, 엘레나 등의 순수 스탯/스프라이트 데이터 주입
                activeAllies.Add(battleChar);
            }
        }
    }
}