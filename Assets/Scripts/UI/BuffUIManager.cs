using UnityEngine;
using System.Collections.Generic;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField] private BuffUIPool buffUIPool;
    [SerializeField] private UIIconLibrary UIIconLibrary;

    private Dictionary<PickupType, UIIcon> activeBuffs = new Dictionary<PickupType, UIIcon>();

    private void OnEnable()
    {
        InteractableManager.OnBuffActivated += AddBuff;
        InteractableManager.OnBuffDeactivated += RemoveBuff;
    }

    private void OnDisable()
    {
        InteractableManager.OnBuffActivated -= AddBuff;
        InteractableManager.OnBuffDeactivated -= RemoveBuff;
    }

    private void AddBuff(PickupType pickupType)
    {
        if (activeBuffs.ContainsKey(pickupType))
        {
            // Buff already active, maybe refresh or stack it
            return;
        }

        UIIcon buffUI = buffUIPool.Get();
        if (buffUI != null)
        {
            buffUI.SetIcon(UIIconLibrary.GetIcon(pickupType));
            activeBuffs.Add(pickupType, buffUI);
        }
    }

    private void RemoveBuff(PickupType pickupType)
    {
        if (activeBuffs.TryGetValue(pickupType, out UIIcon buffUI))
        {
            buffUIPool.Return(buffUI);
            activeBuffs.Remove(pickupType);
        }
    }
} 