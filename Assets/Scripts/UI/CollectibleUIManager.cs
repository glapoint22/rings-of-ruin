using UnityEngine;
using TMPro;

public class CollectibleUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gemCountText;
    [SerializeField] private TMP_Text coinCountText;



    private void OnEnable() {
        GameEvents.OnCollectionUpdate += OnCollectionUpdate;
    }



    private void OnCollectionUpdate(PlayerState state) {
        gemCountText.text = state.gems.ToString();
        coinCountText.text = state.coins.ToString();
    }
}