public class UISpellPoolManager : UIPoolManager
{
    private void OnEnable() {
        GameEvents.OnAddSpell += AddIcon;
        GameEvents.OnRemoveSpell += RemoveIcon;
    }


    private void AddIcon(SpellType spell) {
        base.AddIcon(spell);
    }

    private void RemoveIcon(SpellType spell) {
        base.RemoveIcon(spell);
    }
}