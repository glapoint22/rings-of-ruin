using UnityEngine;

public class TempAddItem : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private ItemCollection itemCollection;

    public void AddItem()
    {
        foreach (var item in items)
        {
            itemCollection.AddItem(item);
        }
    }
}
