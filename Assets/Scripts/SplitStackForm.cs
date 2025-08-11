using UnityEngine;
using TMPro;

public class SplitStackForm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private GameObject modelBackground;
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


    public void OnSplitButtonClick()
    {
        ItemTransferState.SetSplitQuantity(quantity);
        modelBackground.SetActive(false);
    }


    public void OnCancelButtonClick()
    {
        modelBackground.SetActive(false);
        ItemTransferState.Clear();
    }

    public void SetQuantity(int quantity)
    {
        maxQuantity = quantity;
        this.quantity = 1;
        quantityText.text = this.quantity.ToString();
    }
}
