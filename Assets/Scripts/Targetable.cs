using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private Canvas targetUI; // Reference to UI Canvas
    
    public Vector3 TargetPosition => transform.position;
    
    // UI management methods
    public void ShowUI() { targetUI.gameObject.SetActive(true); }
    public void HideUI() { targetUI.gameObject.SetActive(false); }
    
    private void Start()
    {
        GameEvents.RaiseTargetRegistered(this);
    }
}