using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UnityLinker;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UpperGradeUI : MonoBehaviour
{
    
    //1. 현존하는 내덱의 캐릭터의 정보를 받아온다 DataReceiver꺼 가져오면될듯
    //data의 sprite, cooltime,현재 캐릭터의 실시간 체력
    [SerializeField] private Image coolDownImage;
    [SerializeField] private Button skillButton;
    [SerializeField] private TextMeshProUGUI cooldownTimer;

    public CharacterData cardData {get; private set;}
    private float characterCooldownTime;
    private float currentCooldownTime;
    private bool isCooldown = true;
    private Battle_Character myCharacter;
    public void Setup(CharacterData data)
    {
        cardData = data;
        myCharacter = FindMyCharacterFromReceiver(data);
        Image buttonImage = skillButton.GetComponent<Image>();
        buttonImage.sprite = data.upperGradeSprite;
        characterCooldownTime = data.cooldownTime;
        currentCooldownTime = data.cooldownTime;
        coolDownImage.fillAmount = 1f;
        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(OnSkillButtonClick);
    }
    public void Update()
    {
        if (isCooldown)
        {
            currentCooldownTime -= Time.deltaTime;

            if (coolDownImage != null && characterCooldownTime > 0)
            {
                coolDownImage.fillAmount = currentCooldownTime / characterCooldownTime;
                int intTimer = (int)currentCooldownTime;
                cooldownTimer.text = intTimer.ToString();

            }
            if (currentCooldownTime <= 0)
            {
                cooldownTimer.enabled = false;
                isCooldown = false;
                currentCooldownTime = 0f;
                if (coolDownImage != null) coolDownImage.fillAmount = 0f;

            }
        }
    }
    private Battle_Character FindMyCharacterFromReceiver(CharacterData data)
    {
        var allBattleCharacters = BattleDataReceiver.Instance.allBattleCharacters;

        foreach (Battle_Character battleCharacter in allBattleCharacters)
        {
            if (battleCharacter.MyData == data)
            {
                return battleCharacter;
            }
        }

        return null;
    }

    private void OnSkillButtonClick()
    {
        if (isCooldown) return;
        if (myCharacter != null)
        {
            myCharacter.UpperGrade();

            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        if (characterCooldownTime <= 0f) return;
        currentCooldownTime = characterCooldownTime;
        isCooldown = true;
        cooldownTimer.enabled = true;
    }
}
