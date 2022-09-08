[System.Serializable]
public class Item
{
    public string iD;
    public string name;
    public int price;
    [UnityEngine.HideInInspector]
    public int quantity;

    public Item(string iD, string name, int price, int quantity)
    {
        this.iD = iD;
        this.name = name;
        this.price = quantity * price;
        this.quantity = quantity;
    }
}