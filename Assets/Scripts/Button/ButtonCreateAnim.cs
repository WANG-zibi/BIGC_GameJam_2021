using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonCreateAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("grass type")]
    public Card_Prefab grass;
    Card_Prefab mgrass;
    [Header("anim ")]
    public Card_Prefab cp;
    [Header("GridMgr")]
    public GridsMgr gmr;
    bool bIsEventStart = false;
    Button button;
    void Start()
    {
        grass.gameObject.name = "lvcao2";
        grass.gameObject.SetActive(false);
        mgrass = Instantiate(grass);
        mgrass.Initialize(Type.grass);
        mgrass.gameObject.name = "dulidier";

        GameManager.instance.randomButtonPressedEvent.AddListener(UpdateButton);
        //var animc = Instantiate(cp);
        //animc.Initialize(Type.wolf);
        //animc.gameObject.SetActive(false);
        //GameManager.instance.obtainedCards.Add(animc);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnEvent);
    }
    private void OnEvent()
    {
        AudioManager.instance.PlayEffectById(0);
        gmr.bisLock = true;
        GameManager.instance.randomButtonPressedEvent.Invoke();
        bIsEventStart = true;

    }
    void UpdateButton()
    {
        if (gmr.bisLock)
        {
            createAnim();
            FullPlant();

            Invoke("Upset", 0.6f);

            Invoke("Eating", 1.2f);
        }

    }
    void createAnim()
    {
        //TODO: 扣分
        if (GameManager.instance != null)
        {
            int AnimLen = GameManager.instance.GetObtainedTypes().Count;
            if (AnimLen == 0)
            {
                Debug.Log("当前无解锁！");
                return;
            }
            int randomNum = Random.Range(0, AnimLen);
            var curType = GameManager.instance.GetObtainedTypes()[randomNum];
            var animc = Instantiate(cp);
            animc.Initialize(curType);
            animc.gameObject.SetActive(false);

           // Debug.Log(GameManager.instance.obtainedCards[randomNum].curType);
            gmr.GenrateGrid(animc, 1);
        }
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
            var gr = Instantiate(mgrass);
            int randomNum = Random.Range(0, 4);
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
            else if (randomNum == 3)
            { 
                gr.Initialize(Type.grass);
                gr.gameObject.SetActive(false);
                gmr.chessCount[Type.grass]++;
                gmr.GenrateGrid(gr, 1);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bIsEventStart)
        {
           
            bIsEventStart = false;
        }
    }
}
