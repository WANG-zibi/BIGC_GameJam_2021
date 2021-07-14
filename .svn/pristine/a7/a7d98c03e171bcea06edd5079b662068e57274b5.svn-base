using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSlot : MonoBehaviour
{
    public Card_Prefab card;
    public void OnSlotClick()
    {
        Debug.Log("cc");
        if (GameManager.instance.GetWorldState() == WorldState.InStore)
        {
            Store store = transform.parent.GetComponent<Store>();
            store.OnCardClick(card);
        }
    }
}
