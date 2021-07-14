using System;
using UnityEngine;
using UnityEngine.UI;

public class StageSummary : MonoBehaviour
{
    private GameManager gm;
    public GameObject summaryPanel;

    public Button storeBtn;
    public Button continueBtn;
    // public Text roundText;
    // public Text scoreText;
    // public Text reductionText;
    // public Text remainingText;
    public NumberDisplay roundNum;
    public NumberDisplay scoreNum;
    public NumberDisplay reductionNum;
    public NumberDisplay remainingNum;

    private int energyReduction;

    // private bool debugFlag;
    private void OnStageSummary()
    {
        // Debug.Log("StageSummary.OnStageSummary");
        energyReduction = gm.GetEnergyReduction();
        bool success = gm.SpendEnergy(energyReduction);
        // Game Over
        if (!success)
        {
            gm.gameOverEvent.Invoke();
            return;
        }

        // Show summary UI
        UpdateSummaryPanel();
        summaryPanel.SetActive(true);
    }

    private void UpdateSummaryPanel()
    {
        // roundText.text = gm.GetRoundIndex().ToString();
        // scoreText.text = gm.GetScore().ToString();
        // reductionText.text = energyReduction.ToString();
        // remainingText.text = gm.GetEnergy().ToString();
        
        roundNum.UpdateNumber(gm.GetRoundIndex());
        scoreNum.UpdateNumber(gm.GetScore());
        reductionNum.UpdateNumber(energyReduction);
        remainingNum.UpdateNumber(gm.GetEnergy());
        
    }

    
    
    private void OnStageSummaryExit()
    {
        summaryPanel.SetActive(false);
    }

    
    public void OnStoreBtnClick()
    {
        AudioManager.instance.PlayEffectById(0);
        gm.stageSummaryExitEvent.Invoke();
        gm.inStoreEvent.Invoke();
    }
    
    public void OnContinueBtnClick()
    {
        AudioManager.instance.PlayEffectById(0);
        gm.stageSummaryExitEvent.Invoke();
        gm.waitForActionEvent.Invoke();
    }
    
    
    private void Start()
    {
        gm = GameManager.instance;
        gm.stageSummaryEvent.AddListener(OnStageSummary);
        gm.stageSummaryExitEvent.AddListener(OnStageSummaryExit);
        summaryPanel.SetActive(false);

        // debugFlag = true;
    }

    private void Update()
    {
        // For Debugging
        // if ((int)(Time.deltaTime * 10000) % 10 == 0 && debugFlag)
        // {    
        //     gm.stageSummaryEvent.Invoke();
        //     debugFlag = false;
        // }
    }
}
