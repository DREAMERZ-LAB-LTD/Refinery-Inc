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

    [SerializeField] private Identity identity;
    public static List<Identity> availables = new List<Identity>();

    private void Start()
    {
        for (int i = 0; i < availables.Count; i++)
            if (availables[i].iD == identity.iD)
                return;

        availables.Add(identity);
    }

    private void OnDestroy()
    {
        if (availables.Contains(identity))
            availables.Remove(identity);
    }
}