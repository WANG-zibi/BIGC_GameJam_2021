using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;

public class mainUI : MonoBehaviour
{
    public static mainUI instance;

    //界面切换
    public Button button_Title;
    public Button button_Tutorial;
    public GameObject black_Tutorial;

    //主界面
    public Button Button;
    public Text text_debug;
    //下面板
    public Button button_tempStart;
    public Button button_Random;
    //文字显示
    public NumberDisplay number_Energy;
    public NumberDisplay number_Score;
    public NumberDisplay number_round;
    public NumberDisplay number_Less;
    public NumberDisplay number_Rent;

    //Window01
    public Button Window01;
    public Button button_Window_Continue;
    public Button button_Window_Shop;

    //Obtained_Animals
    public Image Info_Plane;
    public Text Info_Name;
    public Text Info_Info;
    //public Card_Prefab card_Prefab_rabbit;
    //public Card_Prefab card_Prefab_Bees;
    //public Card_Prefab card_Prefab_Worfs;
    public Card_Prefab[] card_Prefabs;

    private Card_Prefab curPrefab;

    #region Singleton
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
    private void Awake()
    {
        CreateSingleton();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        button_Title.gameObject.SetActive(true);
        button_Tutorial.gameObject.SetActive(true);
        Window01.gameObject.SetActive(true);
        black_Tutorial.SetActive(true);

        //Info 面板初始化
        Setup_Info();

        //界面切换
        button_Title.onClick.AddListener(Onclick_Title);
        button_Tutorial.onClick.AddListener(Onclick_Tutorial);
        Button.onClick.AddListener(Onclick_test);
        button_Random.onClick.AddListener(Onclick_Summon);

        //Window01
        button_Window_Continue.onClick.AddListener(Onclick_Window_Continue);
        button_Window_Shop.onClick.AddListener(Onclick_Window_Shop);
        Window01.gameObject.SetActive(false);

        //GM互动
        GameManager.instance.randomButtonPressedEvent.AddListener(Disable_Buttons);
        GameManager.instance.waitForActionEvent.AddListener(Enable_Buttons);
        GameManager.instance.waitForActionEvent.AddListener(Update_Animals);
        GameManager.instance.waitForActionEvent.AddListener(Update_Numbers);
        GameManager.instance.randomButtonPressedEvent.AddListener(Update_Numbers);
        GameManager.instance.stageSummaryEvent.AddListener(Update_Numbers);

        //Obtained_Animals 初始化
        Update_Animals();
        Update_Numbers();

        
    }

    private void Update()
    {
        // Update_Numbers();
    }

    public void OnMouseOver_Prefab(Card_Prefab card_Prefab)
    {
        curPrefab = card_Prefab;
        //text_debug.text = "" + card_Prefab.curType + "\r\n" + card_Prefab.card._state_Hunger;
        text_debug.text = "";
        if (true)
        {
            Info_Plane.gameObject.SetActive(true);
            Info_Plane.transform.position = curPrefab.transform.position;
            if (Info_Plane.transform.localPosition.y > 100)
                Info_Plane.transform.localPosition += Vector3.down * 400 ;
            if (Info_Plane.transform.localPosition.x < 200)
                Info_Plane.transform.localPosition += Vector3.right * 700;
            //Info_Name.text = "" + curPrefab.race_Animal;
            Info_Name.text = Info_Names[(int)curPrefab.curType];
            //Info_Info.text = "" + curPrefab.race;
            Info_Info.text = Info_Texts[(int)curPrefab.curType];
        }
    }
    public void OnMouseExit_Prefab()
    {
        Info_Plane.gameObject.SetActive(false);
    }

    private void Onclick_test()
    {
        AudioManager.instance.PlayEffectById(0);
        button_Tutorial.gameObject.SetActive(true);
        black_Tutorial.SetActive(true);
    }
    private void Onclick_Title()
    {
        button_Title.gameObject.SetActive(false);
    }
    private void Onclick_Tutorial()
    {
        button_Tutorial.gameObject.SetActive(false);
        black_Tutorial.SetActive(false);
    }
    private void Onclick_Summon()
    {
        GameManager.instance.SpendEnergy(50);
        number_Energy.UpdateNumber(GameManager.instance.GetEnergy());
    }
    private void Onclick_Window_Continue()
    {
        Window01.gameObject.SetActive(false);
    }
    private void Onclick_Window_Shop()
    {
        //--------------------------
        // TODO: 
        //-------------------------
    }
    private void Disable_Buttons()
    {
        button_tempStart.interactable = false;
        button_Random.interactable = false;
    }
    private void Enable_Buttons()
    {
        button_tempStart.interactable = true;
        if (GameManager.instance.GetEnergy() >= 50)
        {
            button_Random.interactable = true;
        }
    }

    private List<string> Info_Texts = new List<string>();
    private List<string> Info_Names = new List<string>();
    //Info_Texts.
    private void Setup_Info()
    {
        Info_Texts.Add("每回合消灭旁边一块草或者花，获得5点能量。生得太快了。");                                              //兔子
        Info_Texts.Add("每回合消灭旁边一块草或者花，获得5点能量。天然无膻味。");                                              //羊
        Info_Texts.Add("每回合消灭旁边一只兔子或者老鼠，获得20点能量。");                                                     //蛇
        Info_Texts.Add("每回合消灭旁边一只兔子或者老鼠，获得20点能量；没有兔子或者老鼠时，消灭旁边的蛇，获得40点能量。");     //鹰
        Info_Texts.Add("根据场景内所有花的数量，每朵获得10点能量，再乘以场景内花的种类数，结算完蜜蜂消失。");                 //蜜蜂
        Info_Texts.Add("每回合消灭旁边的一个尸体，优先消灭死尸>残肢>骨头，获得10 / 5 /0点能量。");                            //老鼠
        Info_Texts.Add("每回合消灭旁边的一只羊，获得50点能量；或者消灭旁边的一只兔子，获得20能量。");                         //狼
        Info_Texts.Add("充满生机。");                                                                                        //草
        Info_Texts.Add("普通的花，能降火。");                                                                                //花
        Info_Texts.Add("甜蜜的回忆。");                                                                                       //花
        Info_Texts.Add("送给逝去的兔子。");                                                                                     //花
        Info_Texts.Add("如果没有被吃掉，那么下个回合会变成残肢。被饿死了，太惨啦。");                                        //尸体
        Info_Texts.Add("如果没有被吃掉，那么下个回合会变成骨头。");                                                             //残肢
        Info_Texts.Add("如果没有被吃掉，那么下个回合会消失。RIP");                                                            //骨骼

        Info_Names.Add("兔子");                                              //兔子
        Info_Names.Add("羊");
        Info_Names.Add("蛇");
        Info_Names.Add("鹰");
        Info_Names.Add("蜜蜂");
        Info_Names.Add("老鼠");
        Info_Names.Add("狼");
        Info_Names.Add("草");
        Info_Names.Add("黄花");
        Info_Names.Add("红花");
        Info_Names.Add("白花");
        Info_Names.Add("尸体");
        Info_Names.Add("残肢");
        Info_Names.Add("骨头");
    }
    public void Update_Animals()
    {
        List<Type> obtainedTypes = GameManager.instance.GetObtainedTypes();
        for(int i=0; i < obtainedTypes.Count; ++i)
        {
            card_Prefabs[i].gameObject.SetActive(true);
            card_Prefabs[i].Initialize(obtainedTypes[i]);
        }
        if(obtainedTypes.Count < 6)
        {
            for (int i = obtainedTypes.Count; i < 6; ++i)
            {
                card_Prefabs[i].gameObject.SetActive(false);
            }
        }
    }
    public void Update_Numbers()
    {
        number_Score.UpdateNumber(GameManager.instance.GetScore());
        number_Energy.UpdateNumber(GameManager.instance.GetEnergy());
        int roundIndex = GameManager.instance.GetRoundIndex();
        number_round.UpdateNumber(roundIndex);
        EnergyReductionRule rule = GameManager.instance.energyReductionRules.FirstOrDefault(r => r.roundIndex >= roundIndex);
        if(rule == null)
        {
            number_Less.UpdateNumber(999);
            number_Rent.UpdateNumber(999);
            return;
        }
        number_Less.UpdateNumber(rule.roundIndex - roundIndex);
        number_Rent.UpdateNumber(rule.energyReduction);
    }
}
