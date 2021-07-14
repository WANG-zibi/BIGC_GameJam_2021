using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Store : MonoBehaviour
{
    private GameManager gm;
    public GameObject storePanel;

    public Card_Prefab universalPrefab;
    public Transform soldOutPrefab;
    public Transform insuffiPrefab;
    
    public Type[] storeTypes;
    private List<Type> remainTypes;
    public Transform[] cardSlots;
    public Text[] priceText;

    public NumberDisplay[] numDisplays;
    // public Transform[] propPrefabs;

    // public Transform propParent;
    // public List<Card_Prefab> storePrefabs;
    
    // these indices count from 1, 0 stand for uninitialized
    private int[] typeIndicesForSale;
    private List<Transform> storeCards;
    private int canvasSortingOrder;
    
    
    private void RemoveObtainedTypesFromStore()
    {
        for (int i = remainTypes.Count - 1; i >= 0; i--)
        {
            if (gm.CheckObtainedTypes(remainTypes[i]))
            {
                remainTypes.Remove(remainTypes[i]);
            }
        }
    }
    
    private void FillTypeIndexByRand(int slotIndex)
    {
        if (typeIndicesForSale[slotIndex] == -1)
        {
            return;
        }
        // Evade duplication
        int index = Random.Range(1, remainTypes.Count + 1);
        int count = 0;
        while (typeIndicesForSale.Contains(index))
        {
            // reroll the index
            index = Random.Range(1, remainTypes.Count + 1);
            count++;
            if (count > 10)
            {
                index = -1;
                break;
            }
        }

        typeIndicesForSale[slotIndex] = index;
    }

    private void SpawnCards()
    {
        RemoveObtainedTypesFromStore();
        if (remainTypes.Count == 0)
        {
            return;
        }

        if (remainTypes.Count < cardSlots.Length)
        {
            FillSoldOutIndices();
        } 
        
        for (int i = 0; i < cardSlots.Length; i++)
        {
            FillTypeIndexByRand(i);
            if (typeIndicesForSale[i] == -1)
            {
                Transform soldOut = Instantiate(soldOutPrefab, cardSlots[i].position, Quaternion.identity, transform);
                soldOut.GetComponent<SpriteRenderer>().sortingLayerName = "Shop";
                soldOut.GetComponent<SpriteRenderer>().sortingOrder = canvasSortingOrder + 1;
                priceText[i].text = "";
                numDisplays[i].gameObject.SetActive(false);
                storeCards.Add(soldOut);
            }
            else
            {
                Type type = remainTypes[typeIndicesForSale[i] - 1];
                Card_Prefab card = Instantiate(universalPrefab, cardSlots[i].position, Quaternion.identity, transform);
                card.Initialize(type);
                card.is_Shop_Animal = true;
                card.gameObject.SetActive(true);
                // card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, -100f);
                card.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Shop";
                card.GetComponentInChildren<SpriteRenderer>().sortingOrder = canvasSortingOrder + 1;
                priceText[i].text = card.price.ToString();
                if (!gm.CheckEnergy(card.price))
                {
                    priceText[i].color = Color.red;
                }
                numDisplays[i].gameObject.SetActive(true);
                numDisplays[i].UpdateNumber(card.price);
                
                // cardSlots[i].GetComponent<StoreSlot>().card = card;
                storeCards.Add(card.transform);
            }
        }
        Debug.Log(typeIndicesForSale);
    }

    private void FillSoldOutIndices()
    {
        int count = remainTypes.Count;
        while (count < cardSlots.Length)
        {
            typeIndicesForSale[count] = -1;
            count++;
        }
    }

    private void ClearCards()
    {
        foreach (Transform storeCard in storeCards)
        {
            if (storeCard)
            {
                Destroy(storeCard.gameObject);
            }
        }
        storeCards = new List<Transform>();
        foreach (Text text in priceText)
        {
            text.color = Color.white;
        }
    }

    public void OnCardClick(Card_Prefab card)
    {
        if (GameManager.instance.SpendEnergy(card.price))
        {
            AudioManager.instance.PlayEffectById(2);
            gm.AddObtainedTypes(card.curType);
            Transform soldOut = Instantiate(soldOutPrefab, card.transform.position, Quaternion.identity, transform);
            soldOut.GetComponent<SpriteRenderer>().sortingLayerName = "Shop";
            soldOut.GetComponent<SpriteRenderer>().sortingOrder = canvasSortingOrder + 1;
            priceText[storeCards.IndexOf(card.transform)].text = "";
            numDisplays[storeCards.IndexOf(card.transform)].gameObject.SetActive(false);
            storeCards.Add(soldOut);
            Destroy(card.gameObject);
        }
        else
        {
            // Inform player insufficient balance 
            AudioManager.instance.PlayEffectById(1);
            Transform insuffi = Instantiate(insuffiPrefab, card.transform.position, Quaternion.identity, transform);
            insuffi.GetComponent<SpriteRenderer>().sortingLayerName = "Shop";
            insuffi.GetComponent<SpriteRenderer>().sortingOrder = canvasSortingOrder + 1;
            priceText[storeCards.IndexOf(card.transform)].text = "";
            numDisplays[storeCards.IndexOf(card.transform)].gameObject.SetActive(false);
            storeCards.Add(insuffi);
            Destroy(card.gameObject);
        }
    }

    public void OnContinueBtn()
    {
        AudioManager.instance.PlayEffectById(0);
        gm.inStoreExitEvent.Invoke();
        gm.waitForActionEvent.Invoke();
    }
    
    #region Event Listener

    private void OnInStore()
    {
        SpawnCards();
        storePanel.SetActive(true);
    }
    
    private void OnInStoreExit()
    {
        storePanel.SetActive(false);
        ClearCards();
    }

    #endregion
    

    private void Start()
    {
        gm = GameManager.instance;
        gm.inStoreEvent.AddListener(OnInStore);
        gm.inStoreExitEvent.AddListener(OnInStoreExit);

        storePanel.SetActive(false);
        canvasSortingOrder = storePanel.GetComponentInChildren<Canvas>().sortingOrder;

        
        remainTypes = new List<Type>(storeTypes);
        storeCards = new List<Transform>();
        typeIndicesForSale = new int[cardSlots.Length];
    }
}
