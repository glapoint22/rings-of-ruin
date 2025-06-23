using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState state;

    private void Awake() {
        state = new PlayerState();
    }

    private void OnEnable() {
        GameEvents.OnCollect += OnCollect;
    }


    private void OnCollect(IPlayerState state) {
        this.state = state.UpdateState(this.state);
        GameEvents.RaiseCollectionUpdate(this.state);
    }
}