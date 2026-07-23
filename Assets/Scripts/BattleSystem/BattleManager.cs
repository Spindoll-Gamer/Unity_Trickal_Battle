using System;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Character;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private UpperGradeManager upperGradeManager;
    [SerializeField] private BS_UIManager bs_UIManager;

    public bool battleOver = false;


[Header("전투 전용 캐릭터 프리팹 (실제 전투 로직/애니메이션이 있는 오브젝트)")]
    public GameObject battleCharacterPrefab;

    [Header("전투 맵의 전/중/후열 소환 위치 (Transform 9개)")]
    public GameObject[] frontSpawnPoints = new GameObject[3];
    public GameObject[] middleSpawnPoints = new GameObject[3];
    public GameObject[] backSpawnPoints = new GameObject[3];

    //==============적군=========================================
    [Header("적군의 전/중/후열 소환 위치 (Transform 9개)")]
    public GameObject[] eFrontSpawnPoints = new GameObject[3];
    public GameObject[] eMiddleSpawnPoints = new GameObject[3];
    public GameObject[] eBackSpawnPoints = new GameObject[3];

    public CharacterData[] eFrontLine = new CharacterData[3];
    public CharacterData[] eMiddleLine = new CharacterData[3];
    public CharacterData[] eBackLine = new CharacterData[3];
    //===========================================================

    public static BattleManager Instance { get; private set; }
     // 실시간으로 관리되는 살아있는 캐릭터 명단
    public List<Battle_Character> aliveMyTeam = new List<Battle_Character>();
    public List<Battle_Character> aliveEnemyTeam = new List<Battle_Character>();

    

    // 캐릭터가 죽을 때(WeekendFarm) 매니저 리스트에서 빼달라고 신호를 보낼 겁니다.
    public void RemoveCharacter(Battle_Character deadCharacter)
    {
        if (deadCharacter.myTeam == TeamType.PlayerA)
        {
            aliveMyTeam.Remove(deadCharacter);
        }
        else
        {
            aliveEnemyTeam.Remove(deadCharacter);
        }
        CheckBattleOver();
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 전투 씬이 시작되자마자 팀을 스폰합니다.
        SpawnMyTeam();
        SpawnEnemyTeam();
        upperGradeManager.GetCharacterData();
        bs_UIManager.TeamHPUI();
    }

    public void Update()
    {
        HurtAllCharacter();
    }
    private void SpawnMyTeam()
    {
        if (BattleDataReceiver.Instance == null)
        {
            Debug.LogError("전투에 사용할 팀 데이터가 없습니다!");
            return;
        }

        //전열 스폰
        SpawnLine(BattleDataReceiver.Instance.frontLine, frontSpawnPoints,TeamType.PlayerA);
        //중열 스폰
        SpawnLine(BattleDataReceiver.Instance.middleLine, middleSpawnPoints, TeamType.PlayerA);
        //후열 스폰
        SpawnLine(BattleDataReceiver.Instance.backLine, backSpawnPoints, TeamType.PlayerA);
    }


    private void SpawnEnemyTeam()
    {
        if (BattleDataReceiver.Instance == null)
        {
            Debug.LogError("전투에 사용할 팀 데이터가 없습니다!");
            return;
        }
        SpawnLine(BattleDataReceiver.Instance.eFrontLine, eFrontSpawnPoints, TeamType.PlayerB);
        SpawnLine(BattleDataReceiver.Instance.eMiddleLine, eMiddleSpawnPoints, TeamType.PlayerB);
        SpawnLine(BattleDataReceiver.Instance.eBackLine, eBackSpawnPoints, TeamType.PlayerB);
    }

    private void SpawnLine(CharacterData[] lineData, GameObject[] spawnPoints, TeamType team)
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
                battleChar.Init(lineData[i], team); // 네르, 엘레나 등의 순수 스탯/스프라이트 데이터 주입
                battleChar.myTeam = team;
                if (team == TeamType.PlayerA) aliveMyTeam.Add(battleChar);
                else aliveEnemyTeam.Add(battleChar);
            }
        }
    }
    public void HurtAllCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Battle_Character character in aliveMyTeam)
            {
                character.TakeDamage(10000);
            }
        }
    }
    private void CheckBattleOver()
    {
        
        //2초 정도 
        if (aliveMyTeam.Count == 0)
        {
            battleOver = false;
            Debug.Log("전투패배");
        }
        else if (aliveEnemyTeam.Count == 0)
        {
            battleOver = true;
            Debug.Log("전투승리");
        }

        if(battleOver)
        { 
            aliveMyTeam.Clear();
            aliveEnemyTeam.Clear();
            DamageTextPool.Instance.ReturnAllToPool();
        }
    }
}