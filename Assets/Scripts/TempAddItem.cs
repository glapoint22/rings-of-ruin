using UnityEngine;

public class TempAddItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private ItemCollection itemCollection;

    public void AddItem()
    {
        itemCollection.AddItem(item);
    }
}
