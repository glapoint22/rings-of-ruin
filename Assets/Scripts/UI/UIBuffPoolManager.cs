public class UIBuffPoolManager : UIPoolManager
{
    private void OnEnable() {
        GameEvents.OnAddBuff += AddIcon;
        GameEvents.OnRemoveBuff += RemoveIcon;
    }

    private void AddIcon(BuffType buff) {
        base.AddIcon(buff);
    }

    private void RemoveIcon(BuffType buff) {
        base.RemoveIcon(buff);
    }
}
