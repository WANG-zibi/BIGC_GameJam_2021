using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridsMgr : MonoBehaviour
{
    public bool bisLock = false;
    // Start is called before the first frame update
    [Header("grid prefab")]
    public GameObject prefab;
    [Header("初始位置")]
    public Vector3 initPos;
    [Header("初始化grid")]
    public List<GameObject> chess;

    public NumberDisplay numberDisplay;

    [Header("instance prefab")]
    public Card_Prefab cardPrefab;

    public Vector2 size;

    [HideInInspector]
    public int curLeftNum = 25;
    Dictionary<Type, Card_Prefab> chessMap = new Dictionary<Type, Card_Prefab>();

    bool isceatechess = false;
    bool bisStart = false;

    public bool bisLevelState = false;
    public Dictionary<Type, int> chessCount = new Dictionary<Type, int>();
    Card_Prefab[] cardList = new Card_Prefab[25];
    GameObject[] gridList = new GameObject[25];
    //Card_Prefab[] cardList = new Card_Prefab[25];
    bool[] bGridisFull = new bool[25];
    Card_Prefab cp;
    Type tpe;
    Vector3 CurPos;

    Type globalType;
    List<Type> priList = new List<Type>();

    //Dictionary<int> Boby 
    bool[] bisStarveToBody = new bool[25];
    void Start()
    {
        priList.Add(Type.bees);
        priList.Add(Type.mouse);
        priList.Add(Type.rabbit);
        priList.Add(Type.sheep);
        priList.Add(Type.snake);
        priList.Add(Type.eagle);
        priList.Add(Type.wolf);
        priList.Add(Type.Bone);
        priList.Add(Type.CanZhi);
        priList.Add(Type.DeadBody);


        float width =size.x;

        float height = size.y;

        float offset = 0;

       

        for (int i = 0; i < 5; i++)
        {
            Vector3 offsetH = Vector3.zero;
            offsetH.y += offset;
            for (int j = 0; j < 5; j++)
            {
                gridList[i * 5 + j] = GameObject.Instantiate(prefab);
                gridList[i * 5 + j].transform.position = initPos + offsetH;
                offsetH += new Vector3(width, 0, 0);
            }

            offset += height;
        }
        for (int i = 0; i < 25; i++)
        {
            bGridisFull[i] = false;
        }
        
        foreach (Type type in Type.GetValues(typeof(Type)))
        {
            Card_Prefab curCard = Instantiate(cardPrefab);
            curCard.gameObject.SetActive(false);

            curCard.Initialize(type);
            chessMap.Add(type, curCard);
        }


        foreach (Type type in Type.GetValues(typeof(Type)))
        {
            chessCount[type] = 0;
        }

        InitGrid();


        
        //StopCoroutine();

    }
    delegate void cal(int pos);
    delegate void eat(Card_Prefab cp,Type type);
    delegate int dir(Vector2Int v);
    delegate void animPlay(Card_Prefab cp,int x);


    //public void UpsetChess()
    //{
    //    ClearGrids();

    //    foreach (Type type in Type.GetValues(typeof(Type)))
    //    {
    //        if(chessCount[type] != 0)
    //        GenrateGrid(chessMap[type], chessCount[type]);
    //    }

    //}

    public void UpsetGrid()
    {

        //随机交换
        for (int i = 0;i<=24;i++)
        {
            var it = Random.Range(0,25);
            while(it == i)
            {
                it = Random.Range(0, 25);
            }

            Swap(i, it);        
        }
    }

    void CreateChess()
    {

       GenrateGrid(cp, 1);
        
    }
    void PriUpdate(Type it)
    {
        cal valUpdate = delegate (int pos)
        {
            chessCount[cardList[pos].curType]--;
            cardList[pos].Animation_Dead();
            if (cardList[pos].gameObject == null) return;

            cardList[pos].gameObject.SetActive(true);
            Destroy(cardList[pos].gameObject, 0.3f);
            bGridisFull[pos] = false;
            curLeftNum++;
        };


        dir DirJudge = delegate (Vector2Int v)
        {
            if (v.x < 0)
            {
                return 4;
            }
            else if (v.x > 0)
            {
                return 3;
            }
            else if (v.y > 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        };

        animPlay AnimPlay = delegate (Card_Prefab cp, int x)
        {

            switch (x)
            {
                case 1:
                    cp.Animation_AttackRight();
                    break;
                case 2:
                    cp.Animation_AttackLeft();
                    break;
                case 3:
                    cp.Animation_AttackUp();
                    break;
                case 4:
                    cp.Animation_AttackDown();
                    break;
                default:
                    break;
            }
        };
        
        for (int i = 0; i < cardList.Length; i++)
        {
            if (it == cardList[i].curType)
            {
 
                bool iseat = false;
                int setpos = 0;
                var curCard = cardList[i];
                Type type = 0;
                Vector3 cPos = Vector3.zero;

                if (curCard == null) continue;
                int curX = i / 5, curY = i % 5;
                List<Vector2Int> dir = new List<Vector2Int>();
                for (int j = -1; j <= 1; j += 2)
                {
                    if (curX + j >= 0 && curX + j < 5)
                    {
                        dir.Add(new Vector2Int(curX + j, curY));
                    }
                }
                for (int j = -1; j <= 1; j += 2)
                {
                    if (curY + j >= 0 && curY + j < 5)
                    {
                        dir.Add(new Vector2Int(curX, curY + j));
                    }
                }

                for (int j = 0; j < dir.Count; j++)
                {

                    var pos = dir[j].x * 5 + dir[j].y;

                    if (cardList[pos] == null) continue;
                    if ((cardList[pos].curType == Type.grass || cardList[pos].curType == Type.flower_red || cardList[pos].curType == Type.flower_white || cardList[pos].curType == Type.flower_yellow) && (curCard.curType == Type.rabbit || curCard.curType == Type.sheep))
                    {
                        iseat = true;
                        type = cardList[pos].curType;
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        //curCard.Update_Sprite_Heart();
                        break;
                        //TODO::Update 草食动物
                        //eatcal(curCard);
                    }
                    else if ((cardList[pos].curType == Type.rabbit || cardList[pos].curType == Type.sheep) && (curCard.curType == Type.wolf))
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        cPos = cardList[pos].transform.position;
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        valUpdate(pos);
                        //curCard.Update_Sprite_Heart();
                        break;
                        //TODO::Update 草肉动物
                        //eatcal(curCard);
                    }
                    else if ((cardList[pos].curType == Type.rabbit || cardList[pos].curType == Type.mouse) && (curCard.curType == Type.snake))
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        cPos = cardList[pos].transform.position;
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        valUpdate(pos);
                        //curCard.Update_Sprite_Heart();
                        break;
                        //TODO::Update 草肉动物
                        //eatcal(curCard);
                    }
                    else if ((cardList[pos].curType == Type.rabbit || cardList[pos].curType == Type.mouse) && (curCard.curType == Type.eagle))
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        // curCard.Update_Sprite_Heart();
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        break;
                        //TODO::Update 草肉动物
                        //eatcal(curCard);
                    }
                    else if ((cardList[pos].curType == Type.snake) && (curCard.curType == Type.eagle))
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        // curCard.Update_Sprite_Heart();
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        break;
                        //TODO::Update 草肉动物
                        //eatcal(curCard);
                    }
                    else if (curCard.curType == Type.mouse && cardList[pos].curType == Type.DeadBody)
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        // curCard.Update_Sprite_Heart();
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        break;
                    }
                    else if (curCard.curType == Type.mouse && cardList[pos].curType == Type.CanZhi)
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        // curCard.Update_Sprite_Heart();
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        break;
                    }
                    else if (curCard.curType == Type.mouse && cardList[pos].curType == Type.Bone)
                    {
                        type = cardList[pos].curType;
                        iseat = true;
                        setpos = pos;
                        valUpdate(pos);
                        cPos = cardList[pos].transform.position;
                        // curCard.Update_Sprite_Heart();
                        AnimPlay(curCard, DirJudge(dir[j] - new Vector2Int(curX, curY)));
                        break;
                    }

                    else if (curCard.curType == Type.DeadBody)
                    {
                        if(!bisStarveToBody[i])
                        curCard.Initialize(Type.CanZhi);
                        curCard.Animation_Dead();
                        curCard.Animation_Reset();
                        break;
                    }
                    else if (curCard.curType == Type.CanZhi)
                    {
                        curCard.Animation_Dead();
                        curCard.Initialize(Type.Bone);
                        
                        curCard.Animation_Reset();
                        break;
                    }
                    else if (curCard.curType == Type.Bone)
                    {

                        chessCount[curCard.curType]--;
                        curCard.Animation_Dead();
                        curLeftNum++;
                        //bGridisFull[i] = false;
                        curCard.gameObject.SetActive(true);
                        Destroy(curCard.gameObject, 0.2f);
                        bGridisFull[i] = false;
                        break;
                    }
                    else if (curCard.curType == Type.bees)
                    {
                        iseat = true;
                        int curEn = 0;
                        int curscr = 0;
                        for (int k = 0; k < cardList.Length; k++)
                        {
                            if (cardList[k].curType == Type.flower_red || cardList[k].curType == Type.flower_white || cardList[k].curType == Type.flower_yellow)
                            {
                                curEn += 5;
                                curscr += 5;
                            }
                        }
                        GameManager.instance.AddEnergy(curEn);
                        GameManager.instance.AddScore(curscr);
                        curCard.gameObject.SetActive(true);
                        Destroy(curCard.gameObject, 0.5f);
                        bGridisFull[i] = false;
                        break;
                    }
                }

                if (!iseat)
                {

                    curCard.card.Time_pass();
                    curCard.Update_Sprite_Heart();
                    if (curCard.card._is_alive == false)
                    {
                        //Debug.Log(curCard.curType);
                        chessCount[curCard.curType]--;
                        curCard.Initialize(Type.DeadBody);
                        bisStarveToBody[i] = true;
                        //curCard.Animation_Dead();
                        //curCard.Animation_Reset();
                        chessCount[curCard.curType]++;
                        //Destroy(curCard.gameObject);
                        //bGridisFull[i] = false;
                    }
                }
                else
                {
                    // Debug.LogWarning(curCard.card._state_Hunger);
                    cp = curCard;
                    tpe = type;
                    CurPos = cPos;
                    //eatcal(curCard,type);
                    bool bIsBreeding = cp.card.Eat(tpe, CurPos, numberDisplay);
                    cp.Update_Sprite_Heart();
                    isceatechess = bIsBreeding;
                    //Invoke("CreateChess", 0.4F);
                    if (bIsBreeding)
                    {
                        Invoke("CreateChess", 0.5F);
                    }//Debug.LogWarning(curCard.card._state_Hunger);
                }
            }
        }
        
    }
    float m_timer = 0f;
    int ik = 0;
    private void Update()
    {
        if(bisLevelState)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= 0.25f)
            {
                m_timer = 0;
                PriUpdate(priList[ik++]);
                if (ik == priList.Count)
                {
                    ik = 0;
                    bisLevelState = false;
                    CalbIsFull();
                    EventUnLock();
                    //GameManager.instance.waitForActionEvent.Invoke();
                    GameManager.instance.roundFinishEvent.Invoke();
                }
            }
            
        }
    }

    public void Levelstate()
    {
        //Debug.Log("轮流计算"); 
        
        

        

        //PriUpdate(priList[0]);
        //Sequence seq = DOTween.Sequence();


        for (int i = 1;i < priList.Count;i++)
        {
            globalType = priList[i];
            Invoke("PriUpdate", 0.1f);
            //    //var it = priList[i];
            //    //PriUpdate(it);
            //    seq.AppendInterval(1);
            //    seq.AppendCallback(() => PriUpdate(priList[i]));
            //    //seq.AppendCallback(() => Debug.Log(priList[i]));
        }
        EventUnLock();
        //seq.AppendCallback(() => EventUnLock());
        //else if()
    }
    void CalbIsFull()
    {
        curLeftNum = 0;
        for (int i = 0; i < bGridisFull.Length; i++)
        {
            if (!bGridisFull[i])
            {
                curLeftNum++;
            }
        }
        for(int i = 0;i < bisStarveToBody.Length;i++)
        {
            bisStarveToBody[i] = false;
        }
    }
    void EventUnLock()
    {
        int rabbitCount = 0;
        int snakeCount = 0;
        int DeadBodyCount = 0;
        for (int i = 0; i < cardList.Length; i++)
        {
            if (cardList[i].curType == Type.rabbit)
            {
                rabbitCount++;
            }
            else if (cardList[i].curType == Type.snake)
            {
                snakeCount++;
            }
            else if (cardList[i].curType == Type.DeadBody || cardList[i].curType == Type.Bone || cardList[i].curType == Type.CanZhi)
            {
                DeadBodyCount++;
            }
        }
        if (rabbitCount >= 5)
        {
            GameManager.instance.AddObtainedTypes(Type.snake);
        }
        if (snakeCount >= 3)
        {
            GameManager.instance.AddObtainedTypes(Type.eagle);
        }
        if (DeadBodyCount >= 4)
        {
            GameManager.instance.AddObtainedTypes(Type.mouse);
        }
    }
    
    // Update is called once per frame


    void Swap(int i, int j)
    {
        if (cardList[i] == null)
        {
            Debug.Log(i);
            return;
        }
        if (cardList[j] == null)
        {
            Debug.Log(j);
            return; 
        }

        var pos1 = cardList[i].transform.position;
        var pos2 = cardList[j].transform.position;

        var tmp = cardList[i];


        tmp.transform.position = pos2;

        cardList[i] = cardList[j];

        cardList[i].transform.position = pos1;
        cardList[j] = tmp;
    }
    void ClearGrids()
    {
        curLeftNum = 25;
        for (int i = 0;i<bGridisFull.Length;i++)
        {
            bGridisFull[i] = false;
        }
        for(int i = 0;i<cardList.Length;i++)
        {
            if(cardList[i] != null)
            {
                Destroy(cardList[i].gameObject);
            }
        }
    }
    void InitGrid()
    {

        Card_Prefab rabbit = chessMap[Type.rabbit];
        Card_Prefab whiteflower = chessMap[Type.flower_white];
        Card_Prefab grass = chessMap[Type.grass];
        Card_Prefab redflower = chessMap[Type.flower_red];
        Card_Prefab yellowflower = chessMap[Type.flower_yellow];
        var rabbitCount = 1;
        var grassCount = 6;
        var whiteflowerCount = 6;
        var redflowerCount = 6;
        var yellowflowerCount = 6;
        //var test = 25; GenrateGrid(chessMap[Type.DeadBody], test);
        GenrateGrid(yellowflower, yellowflowerCount);
        GenrateGrid(redflower, redflowerCount);
        GenrateGrid(rabbit, rabbitCount);
        GenrateGrid(whiteflower, whiteflowerCount);
        GenrateGrid(grass, grassCount);
        bisStart = true;
    }
    
    public void GenrateGrid(Card_Prefab targetChess,int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomNum = Random.Range(0, 25);
            Queue<int> que = new Queue<int>();
            que.Enqueue(randomNum);
            while (que.Count != 0)
            {
                var it = que.Dequeue();
                bool isFull = false;
                for (int j = 0; j < 25; j++)
                {
                    if (!bGridisFull[j])
                    {
                        isFull = true;
                        break;
                    }
                }
                if (!isFull)
                {
                    break;
                }

                if (!bGridisFull[it])
                {
                    bGridisFull[it] = true;
                    Vector3 pos = gridList[it].transform.position;
                    pos.z = -2f;
                    cardList[it] = Instantiate(targetChess,pos,Quaternion.identity);
                    if(!bisStart)
                        chessCount[targetChess.curType]++;
                    curLeftNum--;
                    if (cardList[it].card == null)
                    {
                        Debug.Log(targetChess.curType);
                        cardList[it].Initialize(targetChess.curType);
                        
                    }

                    cardList[it].gameObject.SetActive(true);
                    
                    break;
                }
                else
                {
                    if (it == 0)
                    {
                        que.Enqueue(it + 1);
                    }
                    else if (it == 24)
                    {
                        que.Enqueue(it - 1);
                    }
                    else
                    {
                        que.Enqueue(it + 1);
                        que.Enqueue(it - 1);
                    }
                }

            }
        }
    }




}
