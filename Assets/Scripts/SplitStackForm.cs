using UnityEngine;
using TMPro;

public class SplitStackForm : MonoBehaviour
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


    public void OnSplitButtonClick()
    {
        GameEvents.RaiseStackSplit(quantity);
        gameObject.SetActive(false);
    }


    public void OnCancelButtonClick()
    {
        gameObject.SetActive(false);
        GameEvents.RaiseStackSplitCancel();
    }

    public void SetQuantity(int quantity)
    {
        maxQuantity = quantity;
        this.quantity = 1;
        quantityText.text = this.quantity.ToString();
    }
}
