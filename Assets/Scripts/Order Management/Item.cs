using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
        [System.Serializable]
        public class Identity
        {
            public string iD;
            public string name;
            public int price;
            public Sprite icon;
            [HideInInspector]
            public int quantity;
            [HideInInspector]
            public Vector3 pickPoint;

            public Identity(string iD, string name, int price, int quantity, Sprite icon)
            {
                this.iD = iD;
                this.name = name;
                this.price = quantity * price;
                this.quantity = quantity;
                this.icon = icon;
            }
        }
    [System.Serializable]
    public class ItemSet
    {
        public Vector2Int quantityRaneg = new Vector2Int(1, 2);
        public Identity identity;
    }
    
    [SerializeField] private ItemSet itemSet;

    public static List<ItemSet> availables = new List<ItemSet>();

    private void Start()
    {
        for (int i = 0; i < availables.Count; i++)
            if (availables[i].identity.iD == itemSet.identity.iD)
                return;

        availables.Add(itemSet);
    }

    private void OnDestroy()
    {
        if (availables.Contains(itemSet))
            availables.Remove(itemSet);
    }

    public void SetMinQuantity(int min)
    {
        itemSet.quantityRaneg.x = min;
    }
    public void SetMaxQuantity(int max)
    {
        itemSet.quantityRaneg.y = max;
    }
}