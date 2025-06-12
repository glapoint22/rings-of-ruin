using UnityEngine;
using System.Collections.Generic;

public class BuffHUDManager : MonoBehaviour
{
    [SerializeField] private BuffUIPool buffUIPool;
    [SerializeField] private HudIconLibrary hudIconLibrary;

    private Dictionary<PickupType, BuffUI> activeBuffs = new Dictionary<PickupType, BuffUI>();

    private void OnEnable()
    {
        PlayerState.OnBuffActivated += AddBuff;
        PlayerState.OnBuffDeactivated += RemoveBuff;
    }

    private void OnDisable()
    {
        PlayerState.OnBuffActivated -= AddBuff;
        PlayerState.OnBuffDeactivated -= RemoveBuff;
    }

    private void AddBuff(PickupType buffType)
    {
        if (activeBuffs.ContainsKey(buffType))
        {
            // Buff already active, maybe refresh or stack it
            return;
        }

        BuffUI buffUI = buffUIPool.Get();
        if (buffUI != null)
        {
            buffUI.SetIcon(hudIconLibrary.GetIcon(buffType));
            activeBuffs.Add(buffType, buffUI);
        }
    }

    private void RemoveBuff(PickupType buffType)
    {
        if (activeBuffs.TryGetValue(buffType, out BuffUI buffUI))
        {
            buffUIPool.Return(buffUI);
            activeBuffs.Remove(buffType);
        }
    }
} 