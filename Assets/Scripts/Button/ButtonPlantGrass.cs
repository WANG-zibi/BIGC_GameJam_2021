using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPlantGrass : MonoBehaviour
{
    [Header("grass type")]
    public Card_Prefab grass;
    Card_Prefab mgrass;
    [Header("GridMgr")]
    public GridsMgr gmr;

    bool bIsEventStart = false;
    Button button;
    void Start()
    {
        grass.gameObject.name = "lvcao1";
        grass.gameObject.SetActive(false);
        mgrass = Instantiate(grass);
        mgrass.Initialize(Type.grass);
        mgrass.gameObject.name = "dulidiyi";
        GameManager.instance.randomButtonPressedEvent.AddListener(updateButton);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnEvent);
    }
    private void OnEvent()
    {
        gmr.bisLock = false;
        AudioManager.instance.PlayEffectById(0);

        GameManager.instance.randomButtonPressedEvent.Invoke();
        bIsEventStart = true;
    }
    void updateButton ()
    {
        if (!gmr.bisLock)
        {
            FullPlant(); //填充
                         //GameManager.instance.randomButtonPressedEvent.Invoke();

            Invoke("Upset", 0.5f);
            //GameManager.instance.stageSummaryEvent.Invoke();
            Invoke("Eating", 1.0f);
        }
}
    private void Update()
    {
     

        
    }
    void Upset()
    {
        gmr.UpsetGrid();
    }
    void Eating()
    {
        gmr.bisLevelState = true;
    }
    void FullPlant()
    {
        int cnt = gmr.curLeftNum;
        //TODO: to start laohuji流程
        for (int i = 0; i < cnt; i++)
        {
            int randomNum = Random.Range(0, 4);
            var gr = Instantiate(mgrass);
            if (randomNum == 0)
            {

                
                gr.Initialize(Type.flower_red);
                gr.gameObject.SetActive(false);
                gmr.chessCount[Type.flower_red]++;
                gmr.GenrateGrid(gr, 1);
            }
            else if (randomNum == 1)
            {

                gr.Initialize(Type.flower_white);
                gr.gameObject.SetActive(false);
                gmr.chessCount[Type.flower_white]++;
                gmr.GenrateGrid(gr, 1);
            }
            else if (randomNum == 2)
            {
       
                gr.Initialize(Type.flower_yellow);
                gr.gameObject.SetActive(false);
                gmr.chessCount[Type.flower_yellow]++;
                gmr.GenrateGrid(gr, 1);
            }
            else if(randomNum == 3)
            {
            
                gr.Initialize(Type.grass);
                gr.gameObject.SetActive(false);
                gmr.chessCount[Type.grass]++;
                gmr.GenrateGrid(gr, 1);
            }
        }
    }
    // Update is called once per frame


}
