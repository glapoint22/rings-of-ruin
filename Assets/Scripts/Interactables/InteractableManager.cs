// using UnityEngine;
// using System;
// using System.Collections;

// public class InteractableManager : MonoBehaviour
// {
//     public static event Action<int> OnGemCollected;
//     public static event Action<int> OnCoinCollected;
//     public static event Action<PickupType> OnBuffActivated;
//     public static event Action<PickupType> OnBuffDeactivated;
//     public static event Action<PickupType> OnSpellActivated;
//     // public static event Action<PickupType> OnSpellDeactivated;

//     // private void OnEnable()
//     // {
//     //     InteractableBase.OnCollect += OnCollect;
//     //     InteractableBase.OnPickup += OnPickup;
//     // }


//     private void OnPickup(PickupType pickupType)
//     {
//         switch (pickupType)
//         {
//             case PickupType.Health:
//                 OnHeal(10);
//                 break;
//             case PickupType.Shield:
//                 OnShieldPickup();
//                 break;
//             case PickupType.Key:
//                 OnKeyPickup();
//                 break;
//             case PickupType.TimeDilation:
//                 OnTimeDilationPickup();
//                 break;
//             case PickupType.Cloak:
//                 OnCloakPickup();
//                 break;
//             case PickupType.Bloodroot:
//                 OnBloodrootPickup();
//                 break;
//             case PickupType.Fireball:
//                 OnFireballPickup();
//                 break;
//             case PickupType.Stormbolt:
//                 OnStormboltPickup();
//                 break;
//         }
//     }

//     private void OnCollect(CollectibleType collectibleType, int count)
//     {
//         switch (collectibleType)
//         {
//             case CollectibleType.Gem:
//                 OnGemCollect();
//                 break;
//             case CollectibleType.Coin:
//                 OnCoinCollect(count);
//                 break;
//             case CollectibleType.TreasureChest:
//                 OnTreasureChestCollect(count);
//                 break;
//         }
//     }


//     private void OnBloodrootPickup()
//     {
//         OnSpellActivated?.Invoke(PickupType.Bloodroot);
//     }



//     private void OnFireballPickup()
//     {
//         OnSpellActivated?.Invoke(PickupType.Fireball);
//     }

//     private void OnStormboltPickup()
//     {
//         OnSpellActivated?.Invoke(PickupType.Stormbolt);
//     }


//     private void OnTreasureChestCollect(int count)
//     {
//         OnCoinCollect(count);
//         OnBuffDeactivated?.Invoke(PickupType.Key);
//     }


//     private void OnShieldPickup()
//     {
//         Player.Instance.UpdateShield(Player.Instance.ShieldHealth + 35);
//         OnBuffActivated?.Invoke(PickupType.Shield);
//     }

//     private void OnKeyPickup()
//     {
//         OnBuffActivated?.Invoke(PickupType.Key);
//     }

//     private void OnTimeDilationPickup()
//     {
//         OnBuffActivated?.Invoke(PickupType.TimeDilation);
//         StartCoroutine(PickupTimer(PickupType.TimeDilation));
//     }

//     private void OnCloakPickup()
//     {
//         OnBuffActivated?.Invoke(PickupType.Cloak);
//         StartCoroutine(PickupTimer(PickupType.Cloak));
//     }


//     private void OnGemCollect()
//     {
//         int gems = Player.Instance.Gems + 1;
//         Player.Instance.UpdateGems(gems);
//         OnGemCollected?.Invoke(gems);
//     }

//     private void OnCoinCollect(int count)
//     {
//         int coins = Player.Instance.Coins + count;
//         Player.Instance.UpdateCoins(coins);
//         OnCoinCollected?.Invoke(coins);
//     }

//     private void OnHeal(int amount)
//     {
//         float health = Mathf.Min(100f, Player.Instance.Health + amount);
//         Player.Instance.UpdateHealth(health);
//     }


//     private IEnumerator PickupTimer(PickupType pickupType)
//     {
//         yield return new WaitForSeconds(10);
//         OnBuffDeactivated?.Invoke(pickupType);
//     }
// }