using Codice.Client.BaseCommands;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Codice.CM.Common.CmCallContext;

public class Battle_Character : MonoBehaviour
{
    [SerializeField]  private CharacterData myData;
    public CharacterState currentState = CharacterState.Idle;
    public enum CharacterState
    {
        Idle,           // 대기
        Move,           // 적을 향해 이동
        NormalAttack,   // 평타 시전 중
        SkillCasting,   // 서브/EX 스킬 시전 중
        Stunned,        // 기절/제어 불능
        Dead            // 사망
    }

    [Header("HP/SP 바")]
    [SerializeField]private Slider hpSlider;
    [SerializeField]private Slider spSlider;


    [Header("전투 실시간 상태")]
    public float currentHp;
    public float attackCooldown = 1.5f; // 평타 주기 (초)
    private float lastAttackTime;


    public enum TeamType { PlayerA, PlayerB }
    public TeamType myTeam;


    SpriteRenderer spriteRenderer;
    Battle_Character currentTarget;
    
    private bool isSPRegenStart = false;
    private Coroutine spRegenCoroutine;

    // BattleManager가 소환 후 데이터를 넣어주는 함수
    public void Init(CharacterData data)
    {
        myData = data;
        currentHp = data.maxHP;
        data.currentSP = 0;
        // 소환되자마자 바로 때리는 걸 방지하기 위해 랜덤 딜레이를 살짝 줍니다.
        lastAttackTime = Time.time + Random.Range(0f, 0.5f);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = myData.portraitSprite;
        spriteRenderer.flipX = true;
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.transform.localPosition = new Vector3(-0.65f, 5.9f, 0f);
        spriteRenderer.transform.localRotation = Quaternion.identity;
        spriteRenderer.transform.localScale = new Vector3(7.0f, 7.0f, 1f);


        if (hpSlider != null)
        {
            hpSlider.maxValue = data.maxHP;
            hpSlider.value = currentHp;
        }
        if(spSlider != null)
        {
            spSlider.maxValue = data.maxSP;
            spSlider.value = myData.currentSP;
        }
    }

    private void Update()
    {
        if (currentState == CharacterState.Dead) return;

        if (myData.currentSP >= myData.maxSP && currentState != CharacterState.SkillCasting && currentState != CharacterState.Stunned)
        {
            ChangeState(CharacterState.SkillCasting);
        }

        if (spSlider != null)
        {
            spSlider.value = myData.currentSP;
        }
        // ★ 현재 상태에 따라 기계적으로 딱 자기 할 일만 시킵니다!
        switch (currentState)
        {
            case CharacterState.Idle:
                // 적이 있는지 탐색하고, 사거리를 체크해서 이동할지 공격할지 결정
                currentTarget = FindTarget();
                if (currentTarget == null)
                {
                    //Move(); 탐색해서 적이 없을경우 이동
                    ChangeState(CharacterState.Move);
                }
                else
                {
                    if(isSPRegenStart == false)
                    {
                        spRegenCoroutine = StartCoroutine(RegenSPRoutine());
                        isSPRegenStart = true;
                    }
                    ChangeState(CharacterState.NormalAttack);

                }

                break;

            case CharacterState.Move:
                // 적을 향해 실시간으로 걸어가는 로직
                StopCoroutine(spRegenCoroutine);
                spRegenCoroutine = null;


                break;

            case CharacterState.NormalAttack:
                // 평타 모션이 끝날 때까지 대기 (타이머는 여기서만 작동!)

                if (currentTarget == null || currentTarget.currentState == CharacterState.Dead)
                {
                    ChangeState(CharacterState.Idle);
                    break;
                }

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    NormalAttack(currentTarget);
                }
                break;

            case CharacterState.SkillCasting:
                // 스킬 시전 중에는 아무것도 안 하고 가만히 연출만 대기 (평타 차단 완벽 구현!)
                LowerGrade(); // 저학년스킬발동
                break;

            case CharacterState.Stunned:

                StopCoroutine(spRegenCoroutine);
                spRegenCoroutine = null;
                // 기절 상태일 때는 타이머고 이동이고 전부 올스톱
                break;
        }
    }
    public void ChangeState(CharacterState newState)
    {
        if (currentState == CharacterState.Dead) return; // 죽은 자는 상태를 바꿀 수 없다.

        currentState = newState;

        // 상태가 바뀔 때 최초 1번 실행되어야 하는 로직들 처리
        switch (newState)
        {
            case CharacterState.SkillCasting:
                // 애니메이터에 스킬 트리거 켜기 등
                break;
            case CharacterState.Dead:
                // 콜라이더 끄고 사망 애니메이션 재생 등
                break;
        }
    }

    private IEnumerator RegenSPRoutine()
    {
        while (currentState != CharacterState.Dead)
        {
            yield return new WaitForSeconds(1.0f);

            if (myData.currentSP < myData.maxSP)
            {
                myData.currentSP += myData.regenSP;
                if (myData.currentSP > myData.maxSP) myData.currentSP = myData.maxSP;

                Debug.Log($"{gameObject.name} SP 회복 중: {myData.currentSP}");
            }
        }
    }







    private void NormalAttack(Battle_Character currentTarget)
    {
        if (currentTarget != null)
        {
            lastAttackTime = Time.time;

            // 공격 애니메이션 트리거 (공격 모션 재생)
            // GetComponent<Animator>().SetTrigger("Attack");
            myData.normalAttack.skillAction.ExecuteSkill(this, currentTarget);
            Debug.Log($"{myData.characterName}의 자동 평타! 적에게 데미지.");

        }
    }

    
    private Battle_Character FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Battle_Character closestEnemy = null;
        float closestDistance = Mathf.Infinity; // 비교를 위해 처음엔 무한대 값으로 설정
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Battle_Character enemyChar = enemy.GetComponent<Battle_Character>();

            // 살아있는 적만 검사
            if (enemyChar != null && enemyChar.currentState != CharacterState.Dead)
            {
                // 나와 이 적 사이의 거리를 계산 (제곱 거리를 쓰면 루트 계산이 빠져서 성능에 더 좋습니다)
                float distanceToEnemy = (enemy.transform.position - currentPosition).sqrMagnitude;

                // 방금 검사한 적이 기존에 찾은 적보다 더 가깝다면?
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy; // 가장 짧은 거리 갱신
                    closestEnemy = enemyChar;          // 이번 녀석을 타겟으로 찜!
                }
            }
        }

        // 최종적으로 가장 가까운 적(없으면 null)을 반환합니다.
        return closestEnemy;
    }


    // EX 스킬 발동 함수 (외부 UI 버튼에서 이 함수를 호출할 겁니다)
    public void LowerGrade()
    {
        if (currentState == CharacterState.Dead) return;
        StopCoroutine(RegenSPRoutine());
        myData.currentSP = 0;
        myData.lowerSkill.skillAction.ExecuteSkill(this,currentTarget);
        Debug.Log($" {myData.characterName}의 저학년 스킬 발동!");
        // 여기에 전체 광역기나 힐, 특수 애니메이션 로직 배치
    }

    public void UpperGrade()
    {
        if (currentState == CharacterState.Dead) return;
        myData.upperSkill.skillAction.ExecuteSkill(this, currentTarget);
        Debug.Log($" {myData.characterName}의 고학년 스킬 발동!");
        // 여기에 전체 광역기나 힐, 특수 애니메이션 로직 배치
    }

    public void TakeDamage(float damage)
    {
        if (currentState == CharacterState.Dead) return;
        currentHp -= damage;
        Debug.Log($" 현재 체력 : {currentHp} " );
        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
        }

        if (currentHp <= 0)
        {
            currentHp = 0;
            WeekendFarm();
        }
    }

    private void WeekendFarm()
    {
        ChangeState(CharacterState.Dead);
        Debug.Log($"{myData.characterName} 사망...");
        // 사망 애니메이션이나 오브젝트 처리
    }
}