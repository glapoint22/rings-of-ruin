using UnityEngine;
using TMPro;

public class SpitterForm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityText;
    // public int Quantity => int.Parse(quantityText.text);
    private int maxQuantity;
    private int quantity;

    public void IncreaseQuantity()
    {
        quantity = Mathf.Clamp(quantity += 1, 1, maxQuantity);
        quantityText.text = quantity.ToString();
    }

    public void DecreaseQuantity()
    {
        quantity = Mathf.Clamp(quantity -= 1, 1, maxQuantity);
        quantityText.text = quantity.ToString();
    }


    public void OnClick()
    {
        GameEvents.RaiseItemCollectionClick(quantity);
        gameObject.SetActive(false);
    }

    public void SetQuantity(int quantity)
    {
        maxQuantity = quantity;
        this.quantity = 1;
        quantityText.text = this.quantity.ToString();
    }
}
