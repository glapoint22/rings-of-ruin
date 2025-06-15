using UnityEngine;
using System.Collections.Generic;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField] private BuffUIPool buffUIPool;
    [SerializeField] private UIIconLibrary hudIconLibrary;

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

    private void AddBuff(PickupType buffType)
    {
        if (activeBuffs.ContainsKey(buffType))
        {
            // Buff already active, maybe refresh or stack it
            return;
        }

        UIIcon buffUI = buffUIPool.Get();
        if (buffUI != null)
        {
            buffUI.SetIcon(hudIconLibrary.GetIcon(buffType));
            activeBuffs.Add(buffType, buffUI);
        }
    }

    private void RemoveBuff(PickupType buffType)
    {
        if (activeBuffs.TryGetValue(buffType, out UIIcon buffUI))
        {
            buffUIPool.Return(buffUI);
            activeBuffs.Remove(buffType);
        }
    }
} 