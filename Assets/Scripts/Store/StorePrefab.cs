
//using UnityEngine;



public class StorePrefab : Card_Prefab
{
    
    public void OnMouseDown()
    {
        if (GameManager.instance.GetWorldState() == WorldState.InStore)
        {
            Store store = transform.parent.GetComponent<Store>();
            store.OnCardClick(this);
        }
    }
    
}