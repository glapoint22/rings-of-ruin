using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject achievementsPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    private GameObject[] allPanels;

    private void Awake()
    {
        // Store all panels in an array for easy toggling
        allPanels = new GameObject[]
        {
            mainMenuPanel,
            pauseMenuPanel,
            levelSelectPanel,
            achievementsPanel,
            statsPanel,
            shopPanel,
            settingsPanel
        };

        ShowMainMenu(); // Set default visible panel on startup
    }

    // Deactivate all panels, then activate the one we want
    private void ShowPanel(GameObject panelToShow)
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(panel == panelToShow);
        }
    }

    // Public methods for buttons or code to call
    public void ShowMainMenu() => ShowPanel(mainMenuPanel);
    public void ShowPauseMenu() => ShowPanel(pauseMenuPanel);
    public void ShowLevelSelect() => ShowPanel(levelSelectPanel);
    public void ShowAchievements() => ShowPanel(achievementsPanel);
    public void ShowStats() => ShowPanel(statsPanel);
    public void ShowShop() => ShowPanel(shopPanel);
    public void ShowSettings() => ShowPanel(settingsPanel);



    public void StartGameplay()
    {
        playerController.StartGame(); // Start movement
        mainMenuPanel.SetActive(false); // Hide menu
    }
}
