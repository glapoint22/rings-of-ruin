public class UISpellPoolManager : UIPoolManager
{
    private void OnEnable() {
        GameEvents.OnAddSpell += AddIcon;
        GameEvents.OnRemoveSpell += RemoveIcon;
    }


    private void AddIcon(PickupType spell) {
        base.AddIcon(spell);
    }

    private void RemoveIcon(PickupType spell) {
        base.RemoveIcon(spell);
    }
}