using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Type
{
    rabbit,             //兔子
    sheep,              //羊
    snake,              //蛇
    eagle,              //鹰
    bees,               //蜜蜂
    mouse,              //老鼠
    wolf,               //狼
    grass,              //草
    flower_yellow,      //黄花
    flower_red,         //红花
    flower_white,       //白花
    DeadBody,           //尸体
    CanZhi,             //残肢
    Bone                //骨骼
}


public class Card_Prefab : MonoBehaviour
{
    // 图片
    public Sprite[] sprite_Animal;
    public Sprite[] sprite_Plant;
    public Sprite[] sprite_Other;
    public Sprite[] sprite_Heart;
    // 种族 设置用Initialize方法
    public Race race;
    public Race_Animal race_Animal;
    public Race_Plant race_Plant;
    public Race_Other race_Other;
    public Type curType;
    // 卡片实例
    public Card card;

    // 价格
    public int price;

    // 事件
    public UnityEvent timepassEvent;
    public UnityEvent eatEvent;
    public UnityEvent breedingEvent;
    public UnityEvent dieEvent;

    //UI相关
    public bool is_Obtained_Animal = false;
    public bool is_Shop_Animal = false;

    //public Dictionary<Race_Animal, int> animal_Energy = new Dictionary<Race_Animal, int>();
    //animal_Energy.Add()

    public NumberDisplay numberdisplay;

    private Animator m_animator;

    public static int hash_Animtor_AttackUp = 0;
    public static int hash_Animtor_AttackDown = 0;
    public static int hash_Animtor_AttackRight = 0;
    public static int hash_Animtor_AttackLeft = 0;
    public static int hash_Animtor_Dead = 0;
    public static int hash_Animator_Reset = 0;

    private SpriteRenderer sprite;
    public SpriteRenderer sprite_heart;
    private BoxCollider2D collider;

    // 类初始化
    public void Initialize(Type type)
    {
        curType = type;

        //sprite = GetComponent<SpriteRenderer>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
     
        if (type < Type.grass)
        {
            race = Race.animal;
            race_Animal = (Race_Animal)type;
            card = new Animal(race_Animal);
            
            timepassEvent.AddListener(Update_Sprite_Heart);
            eatEvent.AddListener(Update_Sprite_Heart);
            if (race_Animal == Race_Animal.sheep)
                price = 200;
            else if (race_Animal == Race_Animal.bees)
                price = 500;
            else if (race_Animal == Race_Animal.wolf)
                price = 200;
            else if (race_Animal == Race_Animal.eagle)
                price = 200;
            else if (race_Animal == Race_Animal.mouse)
                price = 100;
        }
        else if (type < Type.DeadBody)
        {
            race = Race.plant;
            race_Plant = (Race_Plant)(type - Type.grass);
            card = new Plant(race_Plant);
        }
        else if(type <= Type.Bone)
        {
            race_Other = (Race_Other)(type - Type.DeadBody);
            race = Race.other;
            card = new Other_card(race_Other);
        }
        else
        {
            Debug.LogError("Card_Prefab : Initialize : type out of range : " + type);
        }
        Update_Sprite();

        m_animator = GetComponent<Animator>();

        if(hash_Animtor_AttackUp == 0)
        {
            hash_Animtor_AttackUp = Animator.StringToHash("AttackUp");
            hash_Animtor_AttackDown = Animator.StringToHash("AttackDown");
            hash_Animtor_AttackLeft = Animator.StringToHash("AttackLeft");
            hash_Animtor_AttackRight = Animator.StringToHash("AttackRight");
            hash_Animtor_Dead = Animator.StringToHash("Dead");
            hash_Animator_Reset = Animator.StringToHash("Reset");
        }

        //关闭collider
        GameManager.instance.inStoreEvent.AddListener(Disable_collider);
        GameManager.instance.waitForActionEvent.AddListener(Ensable_collider);
    }

    // 更新图片
    public void Update_Sprite()
    {
        if (!sprite)
        {
            //sprite = GetComponent<SpriteRenderer>();
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        if (race == Race.animal)
        {
            sprite.sprite = sprite_Animal[(int)race_Animal];
        }
        else if (race == Race.plant)
        {
            sprite.sprite = sprite_Plant[(int)race_Plant];
        }
        else if (race == Race.other)
        {
            sprite.sprite = sprite_Other[(int)race_Other];
        }
        else
        {
            Debug.LogError("Card_Prefab : Update_Sprite : race out of range : " + race);
        }
        Update_Sprite_Heart();
    }
    public void Update_Sprite_Heart()
    {
        if(card._race != Race.animal)
        {
            sprite_heart.sprite = null;
            return;
        }
        if(card._state_Hunger == State_Hunger.hunger)
        {
            sprite_heart.sprite = sprite_Heart[1];
            //sprite_heart.gameObject.transform.localScale = new Vector3(1,1);
            //sprite_heart.gameObject.transform.position = new Vector3(0.1f, -0.1f);
        }
        else if(card._state_Hunger == State_Hunger.full)
        {
            sprite_heart.sprite = sprite_Heart[0];
            //sprite_heart.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
        }
        else
        {
            sprite_heart.sprite = null;
        }

    }

    #region Mono Events
    // Start is called before the first frame update
    public void Start()
    {

        //Update_Sprite();
    }

    private void OnMouseOver()
    {
        // TODO: Draw a outline for sprite
    }

    private void OnMouseEnter()
    {
        Vector3 vector3 = transform.localScale;
        float tmp = 4.0f / 3.0f;
        transform.localScale = new Vector3(vector3.x * tmp, vector3.y * tmp, vector3.z * tmp);
        if (GameManager.instance.GetWorldState() == WorldState.WaitForAction)
        {
            mainUI.instance.OnMouseOver_Prefab(this);
        }
    }

    private void OnMouseExit()
    {
        Vector3 vector3 = transform.localScale;
        float tmp = 3.0f / 4.0f;
        transform.localScale = new Vector3(vector3.x * tmp, vector3.y * tmp, vector3.z * tmp);
        //transform.localScale = new Vector3(4.5f, 4.5f, 6);
        if (GameManager.instance.GetWorldState() == WorldState.WaitForAction)
        {
            mainUI.instance.OnMouseExit_Prefab();
        }
    }


    public void OnMouseDown()
    {

        if (GameManager.instance.GetWorldState() == WorldState.InStore)
        {
            Store store = transform.parent.GetComponent<Store>();
            store.OnCardClick(this);
        }
    }
    #endregion

    #region 动画调用 ===================
    public void Animation_AttackUp()
    {
        m_animator.SetTrigger(hash_Animtor_AttackUp);
    }

    public void Animation_AttackDown()
    {
        m_animator.SetTrigger(hash_Animtor_AttackDown);
    }

    public void Animation_AttackLeft()
    {
        m_animator.SetTrigger(hash_Animtor_AttackLeft);
    }

    public void Animation_AttackRight()
    {
        m_animator.SetTrigger(hash_Animtor_AttackRight);
    }

    public void Animation_Dead()
    {
        m_animator.SetTrigger(hash_Animtor_Dead);
    }
    public void Animation_reset()
    {
        Invoke("Animation_Reset", 0.3f);
    }
    public void Animation_Reset()
    {
        m_animator.SetTrigger(hash_Animator_Reset);
    }


    #endregion

    private void Disable_collider()
    {
        if (is_Shop_Animal)
        {
            return;
        }
        collider.enabled = false;
    }
    private void Ensable_collider()
    {
        collider.enabled = true;
    }
}

#region Enums

public enum State_Hunger
{
    hunger,     //饥饿
    ordinary,   //普通
    full        //饱腹
}
public enum Race
{
    animal,     //动物
    plant,      //植物
    other       //其他
}
public enum Race_Animal
{
    rabbit,     //兔子
    sheep,      //羊
    snake,      //蛇
    eagle,      //鹰
    bees,       //蜜蜂
    mouse,      //老鼠
    wolf        //狼
}
public enum Race_Plant
{
    grass,              //草
    flower_yellow,      //黄花
    flower_red,         //红花
    flower_white,       //白花
}
public enum Race_Other
{
    DeadBody,   //尸体
    CanZhi,     //残肢
    Bone        //骨骼
}

#endregion

public class Card
{
    //动植物类型
    public Race _race;
    //具体种族
    public Race_Animal _race_Animal;
    public Race_Plant _race_Plant;
    public Race_Other _race_Other;

    //商店价格
    public int _price_store;
    //单次能量
    public int _energy_pertime;

    //饱食度
    public State_Hunger _state_Hunger;
    //是否活着
    public bool _is_alive = true;

    //行为
    public virtual void Time_pass() { }
    public virtual bool Eat() { return false; }
    public virtual bool Eat(Type type,Vector3 pos,NumberDisplay numberDisplay) { return false; }
    public virtual void Breeding() { }
    public virtual void Die() { }
}

public class Animal : Card
{
    
    public Animal()
    {
        _race = Race.animal;
        _state_Hunger = State_Hunger.ordinary;
    }
    public Animal(Race_Animal race_Animal)
    {
        _race = Race.animal;
        _state_Hunger = State_Hunger.ordinary;
        _race_Animal = race_Animal;
    }
    public override void Time_pass()
    {
        base.Time_pass();
        if(_state_Hunger == State_Hunger.hunger)
        {
            Die();
            return;
        }
        _state_Hunger = State_Hunger.hunger;
    }
    public override bool Eat()
    {

        if (_state_Hunger == State_Hunger.full)
        {
            Breeding();
            _state_Hunger = State_Hunger.ordinary;
            return true;
        }
        _state_Hunger = State_Hunger.full;
        return false;
    }
    public override bool Eat(Type type, Vector3 pos, NumberDisplay numberDisplay)
    {
        pos.y += 0.4f;
        pos.z -= 5f;
        //---------------------------
        // TODO: 发起一个energy增长事件
        //---------------------------

        //吃草
        if (type >= Type.grass && type <= Type.flower_white)
        {
            GameManager.instance.AddEnergy(5);
            GameManager.instance.AddScore(5);

            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = 5;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        }
        //吃兔子
        else if (type == Type.rabbit)
        {
            GameManager.instance.AddEnergy(20);
            GameManager.instance.AddScore(20);
            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = 20;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        }
        //吃蛇
        else if (type == Type.snake)
        {
            GameManager.instance.AddEnergy(40);
            GameManager.instance.AddScore(40);
            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = 40;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        }
        //吃羊
        else if (type == Type.sheep)
        {
            GameManager.instance.AddEnergy(50);
            GameManager.instance.AddScore(50);
            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = 50;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        }
        //吃尸体
        else if (type == Type.DeadBody)
        {
            if (_race_Animal != Race_Animal.mouse)
                Debug.LogError("Can not eat dead-things");
            int tmp = (int)(type - Type.DeadBody);
            GameManager.instance.AddEnergy(tmp * 10);
            GameManager.instance.AddScore(tmp * 10);
            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = tmp * 10;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        } // 吃残支
        else if (type == Type.CanZhi)
        {
            if (_race_Animal != Race_Animal.mouse)
                Debug.LogError("Can not eat dead-things");
            int tmp = (int)(type - Type.DeadBody);
            GameManager.instance.AddEnergy(tmp * 5);
            GameManager.instance.AddScore(tmp * 5);
            var num = GameObject.Instantiate(numberDisplay, pos, Quaternion.identity);
            num.displayNumberTest = tmp * 5;
            num.UpdateNumber(num.displayNumberTest, 0.5f);
        }
        else if (type == Type.Bone)
        {
            if (_race_Animal != Race_Animal.mouse)
                Debug.LogError("Can not eat dead-things");
            int tmp = (int)(type - Type.DeadBody);
        }

        return Eat();
    }
    public override void Breeding()
    {
        base.Breeding();
        //---------------------------
        // TODO: 发起一个breeding事件
        //---------------------------
    }
    public override void Die()
    {
        base.Die();
        _is_alive = false;
        //---------------------------
        // TODO: 发起一个die事件
        //---------------------------
    }

}

public class Plant : Card
{
    public Plant()
    {
        _race = Race.plant;
    }
    public Plant(Race_Plant race_Plant)
    {
        _race = Race.plant;
        _race_Plant = race_Plant;
    }
    public override void Die()
    {
        base.Die();
        _is_alive = false;
        //---------------------------
        // TODO: 发起一个die事件
        //---------------------------
    }
}

public class Other_card : Card
{
    public Other_card()
    {
        _race = Race.plant;
    }
    public Other_card(Race_Other race_Other)
    {
        _race = Race.plant;
        _race_Other = race_Other;
    }
    public override void Die()
    {
        base.Die();
        _is_alive = false;
        //---------------------------
        // TODO: 发起一个die事件
        //---------------------------
    }
}


