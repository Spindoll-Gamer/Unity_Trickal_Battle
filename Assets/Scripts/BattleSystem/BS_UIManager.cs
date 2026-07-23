using NUnit.Framework;
using PlasticGui.WorkspaceWindow.Locks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BS_UIManager : MonoBehaviour
{
    public static BS_UIManager Instance {get ; private set;}

    [Header("UI包府")]
    public bool isPaused = false;
    public bool isAutoMode = false;

    [Header("加档UI滚瓢")]
    public int currentSpeed = 1;
    public TextMeshProUGUI speedText;



    [SerializeField] public Slider myTeamSlider;
    [SerializeField] public Slider enemyTeamSlider;
    [SerializeField] public TextMeshProUGUI myText;
    [SerializeField] public TextMeshProUGUI enemyText;



    public float myTeamCurrentHP = 0;
    public float myTeamMaxHP;
    public float enemyTeamCurrentHP = 0;
    public float enemyTeamMaxHP;
    public int myTeamPercent;
    public int enemyTeamPercent;

    public float timer = 0;
    public float updateTime = 0.5f;

    public void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    public void TeamHPUI()
    {
        myTeamCurrentHP = BattleManager.Instance.aliveMyTeam.Sum(c => c.currentHp);
        myTeamMaxHP = BattleManager.Instance.aliveMyTeam.Sum(c => c.maxHp);
        enemyTeamCurrentHP = BattleManager.Instance.aliveEnemyTeam.Sum(c => c.currentHp);
        enemyTeamMaxHP = BattleManager.Instance.aliveEnemyTeam.Sum(c => c.maxHp);

        myTeamSlider.maxValue = myTeamMaxHP;
        myTeamSlider.value = myTeamCurrentHP;
        enemyTeamSlider.maxValue = enemyTeamMaxHP;
        enemyTeamSlider.value = enemyTeamCurrentHP;

        myTeamPercent = (int)Mathf.Round((myTeamCurrentHP / myTeamMaxHP) * 100f);
        enemyTeamPercent =(int)Mathf.Round((enemyTeamCurrentHP / enemyTeamMaxHP) * 100f);
    }
    public void HPUpdate()
    {
        myTeamCurrentHP= BattleManager.Instance.aliveMyTeam.Where(c => c != null).Sum(c => c.currentHp);
        myTeamPercent = (int)Mathf.Round((myTeamCurrentHP / myTeamMaxHP) * 100f);
        myText.text = myTeamPercent.ToString() + "%";
        myTeamSlider.value = myTeamCurrentHP;

        enemyTeamCurrentHP = BattleManager.Instance.aliveEnemyTeam.Where(c => c != null).Sum(c => c.currentHp);
        enemyTeamPercent = (int)Mathf.Round((enemyTeamCurrentHP / enemyTeamMaxHP) * 100f);
        enemyText.text = enemyTeamPercent.ToString() + "%";
        enemyTeamSlider.value = enemyTeamCurrentHP;
    }
    public void Update()
    {
        if (BattleManager.Instance.battleOver)
        {
            return;
        }
        timer += Time.time;
            if (timer >= updateTime)
        {
            timer = 0;
            HPUpdate();
        }
        
        

    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Time.timeScale = 0f;
            Debug.Log("肛勉");
        }

        else 
        {
            Time.timeScale = currentSpeed;
            Debug.Log("秦力");
        }
    }

    public void AccelSpeed()
    {
        if (isPaused) return;
        if(currentSpeed + 1 > 3 )
        {
            currentSpeed = 1;
        }
        else { currentSpeed += 1; }
        speedText.text = "x"+currentSpeed.ToString();
        Debug.Log(currentSpeed);
        Time.timeScale = currentSpeed;
    }

    public void ToggleAutoMode()
    {
        isAutoMode = !isAutoMode;
        if (isAutoMode)
        {
            Debug.Log("Automode On");
        }

        else
        {
            Debug.Log("Automode Off");
        }

    }

}
