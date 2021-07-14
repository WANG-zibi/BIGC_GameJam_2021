using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public enum WorldState
{
    GameMenu,
    WaitForAction,
    RandomButtonPressed,
    RoundFinish,
    StageSummary,
    InStore,
    GameOver
}

[Serializable]
public class EnergyReductionRule
{
    public int roundIndex;
    public int energyReduction;
}

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    

    public WorldState defaultState;
    private WorldState worldState;

    private int score;
    private int energy;

    private int roundIndex;
    public List<EnergyReductionRule> energyReductionRules;
    public Type[] initialTypes;

    // public List<Card_Prefab> obtainedCards;
    private List<Type> obtainedTypes;
    
    public GameObject gameOverPanel;
    
    public UnityEvent gameStartEvent;
    public UnityEvent waitForActionEvent;
    public UnityEvent randomButtonPressedEvent;
    public UnityEvent roundFinishEvent;
    public UnityEvent stageSummaryEvent;
    public UnityEvent stageSummaryExitEvent;
    public UnityEvent inStoreEvent;
    public UnityEvent inStoreExitEvent;
    public UnityEvent gameOverEvent;
    public UnityEvent obtainTypeEvent;

    #region Main Flow

    public void OnGameStart()
    {
        gameOverPanel.SetActive(false);
        InitGameData();
        waitForActionEvent.Invoke();
    }
    public void OnWaitForAction()
    {
        if (worldState == WorldState.WaitForAction)
        {
            Debug.LogWarning("Switch to WorldState.WaitForAction failed");
            return;
        }
        worldState = WorldState.WaitForAction;
    }
        
    public void OnRandomButtonPressed()
    {
        if (worldState == WorldState.RandomButtonPressed)
        {
            Debug.LogWarning("Switch to WorldState.RandomButtonPressed failed");
            return;
        }
        worldState = WorldState.RandomButtonPressed;

    }
    
    public void OnRoundFinish()
    {
        if (worldState == WorldState.RoundFinish)
        {
            Debug.LogWarning("Switch to WorldState.RoundFinish failed");
            return;
        }
        worldState = WorldState.RoundFinish;
        
        if (CheckEnergyReduction())
        {
            stageSummaryEvent.Invoke();
        }
        else
        {
            waitForActionEvent.Invoke();
        }
        roundIndex++;

    }
    
    public void OnStageSummary()
    {
        if (worldState == WorldState.StageSummary)
        {
            Debug.LogWarning("Switch to WorldState.StageSummary failed");
            return;
        }
        worldState = WorldState.StageSummary;
    }
    
    public void OnInStore()
    {
        if (worldState == WorldState.InStore)
        {
            Debug.LogWarning("Switch to WorldState.InStore failed");
            return;
        }
        worldState = WorldState.InStore;
    }
    
    public void OnGameOver()
    {
        if (worldState == WorldState.GameOver)
        {
            Debug.LogWarning("Switch to WorldState.GameOver failed");
            return;
        }
        worldState = WorldState.GameOver;
        
        gameOverPanel.SetActive(true);
    }

    public void InitGameData()
    {
        worldState = defaultState;
        obtainedTypes = new List<Type>(initialTypes);
        score = 0;
        energy = 0;
        roundIndex = 1;
        
        gameOverPanel.SetActive(false);
    }

    public void OnRestartBtn()
    {
        gameStartEvent.Invoke();
    }

    #endregion

    #region Tools

    private void CreateSingleton()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyImmediate(this);
        }
    }

    #endregion

    #region Data Manage

    public WorldState GetWorldState()
    {
        return worldState;
    }

    public int GetRoundIndex()
    {
        return roundIndex;
    }
    
    public bool CheckEnergy(int target)
    {
        return energy >= target;
    }
    
    public void AddEnergy(int target)
    {
        energy += target;
    }

    public int GetEnergy()
    {
        return energy;
    }
    public bool SpendEnergy(int target)
    {
        if (CheckEnergy(target))
        {
            energy -= target;
            return true;
        }
        return false;
    }
    
    public void AddScore(int target)
    {
        score += target;
    }
    public int GetScore()
    {
        return score;
    }

    
    public bool CheckObtainedTypes(Type target)
    {
        return obtainedTypes.Any(card => card == target);
    }

    public bool AddObtainedTypes(Type target)
    {
        if (!CheckObtainedTypes(target))
        {
            obtainedTypes.Add(target);
            return true;
        }
        return false;
    }

    public List<Type> GetObtainedTypes()
    {
        return obtainedTypes;
    }

    public bool CheckEnergyReduction()
    {
        if (energyReductionRules != null)
        {
            return energyReductionRules.Any(r => r.roundIndex == roundIndex);
        }

        return false;
    }
    
    public int GetEnergyReduction()
    {
        if (energyReductionRules != null)
        {
            EnergyReductionRule rule = energyReductionRules.FirstOrDefault(r => r.roundIndex == roundIndex);
            return rule == null ? 0 : rule.energyReduction;
        }
        return 0;
    }

    #endregion

    
    #region Mono Events

    private void Awake()
    {
        CreateSingleton();
    }

    private void Start()
    {
        InitGameData();
        
        // register event listener
        gameStartEvent.AddListener(OnGameStart);
        waitForActionEvent.AddListener(OnWaitForAction);
        randomButtonPressedEvent.AddListener(OnRandomButtonPressed);
        roundFinishEvent.AddListener(OnRoundFinish);
        stageSummaryEvent.AddListener(OnStageSummary);
        inStoreEvent.AddListener(OnInStore);
        gameOverEvent.AddListener(OnGameOver);
    }

    #endregion


    private void Update()
    {
        //Debug.LogWarning(worldState);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
