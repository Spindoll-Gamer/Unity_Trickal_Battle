using System;
using UnityEngine;

public class UpperGradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upperGradePrefab;
    [SerializeField] private Transform upperGradeUITransform;


    public void GetCharacterData()
    {
        var aliveMyTeam = BattleDataReceiver.Instance.aliveMyTeam;

        // 그냥 이 리스트 통째로 루프를 돌리면 출전한 아군 수만큼 카드가 착착 뽑힙니다!
        foreach (Battle_Character battle_Character in aliveMyTeam )
        {
            CreateUpperUI(battle_Character);
        }
    }
    public void CreateUpperUI(Battle_Character battle_Character)
    {
        GameObject upperGradeUI = Instantiate(upperGradePrefab, upperGradeUITransform);

        UpperGradeUI UPUI = upperGradeUI.GetComponent<UpperGradeUI>();
        upperGradeUI.transform.localPosition = Vector3.zero;
        upperGradeUI.transform.localScale = Vector3.one;
        if (UPUI != null)
        {
            UPUI.Setup(battle_Character);
        }
        
    }
}
