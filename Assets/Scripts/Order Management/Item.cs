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
        [HideInInspector]
        public int quantity;

        public Identity(string iD, string name, int price, int quantity)
        {
            this.iD = iD;
            this.name = name;
            this.price = quantity * price;
            this.quantity = quantity;
        }
    }

    [SerializeField] private Identity identity;
    public static List<Identity> availables = new List<Identity>();

    private void Awake()
    {
        availables.Clear();
    }
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