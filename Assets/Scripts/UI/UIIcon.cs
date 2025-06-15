using UnityEngine;
using UnityEngine.UI;

public class UIIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }
}