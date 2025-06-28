using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject activePanel;
    [SerializeField] private GameObject mainMenuPanel;



    private void Awake()
    {
        ShowPanel(mainMenuPanel);
    }



    public void ShowPanel(GameObject panel)
    {
        // Deactivate the currently active panel if it exists
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        // Activate the new panel
        panel.SetActive(true);

        // Update the active panel reference
        activePanel = panel;
    }
}